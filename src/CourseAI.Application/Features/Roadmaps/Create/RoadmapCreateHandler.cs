using System.Security.Claims;
using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Services;
using CourseAI.Core.Enums;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Identity;
using CourseAI.Domain.Entities.Roadmaps;
using CourseAI.Domain.Entities.Transactions;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OneOf;

namespace CourseAI.Application.Features.Roadmaps.Create;

public class RoadmapCreateHandler(
    AppDbContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    IContentGenerator contentGenerator,
    UserManager<User> userManager,
    IUserService userService)
    : IHandler<RoadmapCreateRequest, RoadmapModel>
{
    public async ValueTask<OneOf<RoadmapModel, Error>> Handle(RoadmapCreateRequest request, CancellationToken ct)
    {
        var roadmap = request.ToEntity();
        var courseCost = GetCourseCost(roadmap);

        if (courseCost <= 0)
            return Error.ServerError("Invalid course token cost.");

        await using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            var userResult = await userService.GetUser();
            var user = userResult.Match(
                user => user,
                error => throw new Exception(error.Message)
            );

            if (user.Tokens < courseCost)
                return Error.ServerError("Insufficient tokens to create the course.");

            user.Tokens -= courseCost;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Error.ServerError("Failed to update user tokens.");

            var tokenTransaction = new TokenTransaction
            {
                UserId = Convert.ToInt64(user.Id),
                Amount = -courseCost,
                TransactionType = TransactionType.CourseGeneration,
            };

            dbContext.TokenTransactions.Add(tokenTransaction);
            await dbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            // Course Generation
            dbContext.Roadmaps.Add(roadmap);
            await dbContext.SaveChangesAsync(ct);

            var roadmapAI = await GenerateModulesAndLessonsAsync(roadmap);

            if (string.IsNullOrWhiteSpace(roadmapAI))
            {
                return Error.ServerError("Failed to generate roadmap modules and lessons.");
            }

            var updated = await AttachModulesAndLessonsAsync(roadmap, roadmapAI, ct);

            if (!updated)
            {
                return Error.ServerError("Failed to attach generated modules and lessons to the roadmap.");
            }

            //get all lesson ids from roadmap.Modules.Lessons
            var lessonIds = roadmap.Modules.SelectMany(m => m.Lessons).Select(l => l.Id).ToList();

            for (int i = 0; i < lessonIds.Count; i++)
            {
                var lesson = await dbContext.Lessons
                    .Where(e => e.Id == lessonIds[i])
                    .Include(l => l.Quizzes) // Include quizzes if needed
                    .FirstOrDefaultAsync(ct);

                if (lesson is null)
                    return Error.NotFound<Lesson>();

                // Check if lesson content and quizzes are missing
                if (lesson.Content != null && lesson.Quizzes.Any())
                {
                    continue;
                }

                var prompt = $"Generate detailed lesson content for '{lesson.Title}' as per the system format.";

                var lessonAI = await contentGenerator.GenerateContentAsync(prompt, GetSystemMessage());

                if (string.IsNullOrWhiteSpace(lessonAI))
                    return Error.ServerError($"Error generating content for lesson '{lesson.Title}'.");

                var attached = await AttachLessonContentAsync(lesson, lessonAI, ct);

                if (!attached)
                    return Error.ServerError($"Error attaching generated content for lesson '{lesson.Title}'.");

                await dbContext.SaveChangesAsync(ct);
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            return Error.ServerError(ex.Message);
        }

        return roadmap.Adapt<RoadmapModel>();
    }

    private static int GetCourseCost(Roadmap roadmap)
    {
        return roadmap.EstimatedDuration switch
        {
            15 => 10,
            30 => 15,
            60 => 20,
            _ => 0
        };
    }

    private static async Task<bool> AttachLessonContentAsync(Lesson lesson, string aiResponse, CancellationToken ct)
    {
        try
        {
            var dto = JsonConvert.DeserializeObject<LessonAiResponseDto>(aiResponse);

            if (dto == null)
                return false;

            // Assign LessonContent from the DTO
            lesson.Content = new LessonContent
            {
                MainContent = dto.Content?.MainContent,
                Resources = dto.Content?.Resources,
                Examples = dto.Content?.Examples
            };

            // Assign Quizzes
            if (dto.Quizzes == null || dto.Quizzes.Count == 0)
            {
                return true;
            }

            lesson.Quizzes.Clear(); // If needed, ensure the list is not null

            lesson.Quizzes.AddRange(dto.Quizzes.Select(q => new Quiz
            {
                Question = q.Question,
                Answers = q.Answers,
                CorrectAnswerIndex = q.CorrectAnswerIndex
            }));

            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<string?> GenerateModulesAndLessonsAsync(Roadmap roadmap)
    {
        var userPrompt =
            $"Add modules and lessons for the roadmap '{roadmap.Title}', estimated duration in minutes: {roadmap.EstimatedDuration}.";

        var aiResponse = await contentGenerator.GenerateContentAsync(userPrompt,
            GetRoadmapSystemMessage());

        return aiResponse;
    }

    private async Task<bool> AttachModulesAndLessonsAsync(Roadmap roadmap, string aiResponse, CancellationToken ct)
    {
        try
        {
            var updatedRoadmap = JsonConvert.DeserializeObject<Roadmap>(aiResponse);

            if (updatedRoadmap == null)
            {
                return false;
            }

            roadmap.Description = updatedRoadmap.Description;
            roadmap.Tags = updatedRoadmap.Tags;
            roadmap.Modules.Clear();

            foreach (var aiModule in updatedRoadmap.Modules)
            {
                var module = new RoadmapModule
                {
                    Title = aiModule.Title,
                    Order = aiModule.Order,
                    Lessons = aiModule.Lessons.Select(l => new Lesson
                    {
                        Title = l.Title,
                        Description = l.Description,
                        Content = l.Content,
                        Order = l.Order,
                        Quizzes = l.Quizzes,
                    }).ToList(),
                };

                roadmap.Modules.Add(module);
            }

            await dbContext.SaveChangesAsync(ct);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<LessonContent?> GenerateLessonContentAsync(Lesson lesson)
    {
        var prompt = $"Generate detailed lesson content for '{lesson.Title}' as per the system format.";

        var aiResponse = await contentGenerator.GenerateContentAsync(prompt, GetSystemMessage());

        try
        {
            return JsonConvert.DeserializeObject<LessonContent>(aiResponse);
        }
        catch (JsonException ex)
        {
            await Console.Error.WriteLineAsync($"Failed to parse AI response: {ex.Message}");
            return null;
        }
    }

    private static string GetSystemMessage()
    {
        return """
               You are a lesson-generation assistant. Return valid JSON:
               {
                 "content": {
                   "mainContent": "Content...",
                   "resources": ["https://example.com"],
                   "examples": []
                 },
                 "quizzes": [
                   {
                     "question": "Question...",
                     "answers": ["Option 1", "Option 2", "Option 3", "Option 4"],
                     "correctAnswerIndex": correctAnswerIndex
                   }
                 ]
               }

               If you cannot generate suitable lesson content, do not just say you're unable and explain error. 
               On the topic of the lesson add: 1-2 resource links; 1 quiz question with 4 options and the correct answer index.
               """;
    }

    private static string GetRoadmapSystemMessage()
    {
        return @"
You are a roadmap-generation assistant. Return a JSON object with single-quoted keys and strings:
{
  'description': 'A concise summary of the roadmap topic',
  'tags': [
    'tag1',
    'tag2',
    ...
  ],
  'modules': [
    {
      'title': 'Module Title',
      'order': 1,
      'lessons': [
        {
          'title': 'Lesson Title',
          'description': 'Short summary',
          'content': null,
          'example': null,
          'resources': [],
          'order': 1,
          'quizzes': null,
        }
      ]
    }
  ]
}

- If the roadmap is generated successfully, provide a meaningful **short description** for the 'description' field. Do not use ""Roadmap generated successfully.""
- If you encounter an issue generating the roadmap, use the 'description' field to explain the issue.
- Generate at least 1 module with at least 2 lessons, scaling up the number of modules and lessons based on 'EstimatedDuration' (1-3 modules, 2+ lessons per module).
- Keep the output concise and clear.
";
    }
}

public class LessonAiResponseDto
{
    [JsonProperty("content")] public LessonContentDto Content { get; set; }

    [JsonProperty("quizzes")] public List<QuizDto> Quizzes { get; set; }
}

public class LessonContentDto
{
    [JsonProperty("mainContent")] public string MainContent { get; set; }

    [JsonProperty("resources")] public List<string> Resources { get; set; }

    [JsonProperty("examples")] public List<string> Examples { get; set; }
}

public class QuizDto
{
    [JsonProperty("question")] public string Question { get; set; }

    [JsonProperty("answers")] public List<string> Answers { get; set; }

    [JsonProperty("correctAnswerIndex")] public int CorrectAnswerIndex { get; set; }
}