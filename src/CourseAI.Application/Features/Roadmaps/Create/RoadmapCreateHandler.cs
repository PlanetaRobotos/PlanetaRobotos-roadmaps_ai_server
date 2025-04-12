using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Options;
using CourseAI.Application.Services;
using CourseAI.Core.Enums;
using CourseAI.Core.Security;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Identity;
using CourseAI.Domain.Entities.Roadmaps;
using CourseAI.Domain.Entities.Transactions;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OneOf;

namespace CourseAI.Application.Features.Roadmaps.Create;

public class RoadmapCreateHandler(
    AppDbContext dbContext,
    IContentGenerator contentGenerator,
    UserManager<User> userManager,
    IUserService userService,
    IOptions<StabilityAIOptions> stabilityOptions,
    ILogger<IHandler<RoadmapCreateRequest, RoadmapModel>> logger)
    : IHandler<RoadmapCreateRequest, RoadmapModel>
{
    public async ValueTask<OneOf<RoadmapModel, Error>> Handle(RoadmapCreateRequest request, CancellationToken ct)
    {
        var roadmap = request.ToEntity();
        var coursePrice = request.Price;

        await using var transaction = await dbContext.Database.BeginTransactionAsync(ct);
        
        logger.LogInformation("Inside: Creating roadmap with title {Title}, price {Price}, isTest {IsTest}", request.Title, request.Price, request.IsTest);

        try
        {
            if (!request.IsTest)
            {
                var userResult = await userService.GetUser();
                var user = userResult.Match(
                    user => user,
                    error => throw new Exception(error.Message)
                );
                
                logger.LogInformation("User found with email {Email}", user.Email);

                var roles = await userManager.GetRolesAsync(user);

                logger.LogInformation("User roles: {Roles}", roles);

                if (!roles.Contains(Roles.creator.ToString()) && 
                    !roles.Contains(Roles.AppSumo_1.ToString()) && 
                    !roles.Contains(Roles.studio.ToString()))
                {
                    logger.LogInformation("User roles: {Roles}", string.Join(", ", roles));
                    
                    // Operate on tokens
                    if (user.Tokens < coursePrice)
                        return Error.ServerError("Insufficient tokens to create the course.");

                    user.Tokens -= coursePrice;

                    var result = await userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                        return Error.ServerError("Failed to update user tokens.");

                    var tokenTransaction = new TokenTransaction
                    {
                        UserId = Convert.ToInt64(user.Id),
                        Amount = -coursePrice,
                        TransactionType = TransactionType.CourseGeneration,
                    };

                    dbContext.TokenTransactions.Add(tokenTransaction);
                    await dbContext.SaveChangesAsync(ct);
                }
            }
            
            logger.LogInformation("User has sufficient tokens to create the course.");

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

                var lessonAI = await contentGenerator.GenerateContentAsync(prompt, GetLessonSystemMessage());

                if (string.IsNullOrWhiteSpace(lessonAI))
                    return Error.ServerError(
                        $"Error generating content for lesson. Title: '{lesson.Title}'. Content: '{lesson.Content}, Quizzes: '{lesson.Quizzes}'.");

                var attached = await AttachLessonContentAsync(lesson, lessonAI, ct);

                if (!attached)
                    return Error.ServerError($"Error attaching generated content for lesson '{lesson.Title}'.");

                await dbContext.SaveChangesAsync(ct);
            }
            
            logger.LogInformation("Roadmap created and visual is {isEnabled}", stabilityOptions.Value.IsEnabled);

            if (stabilityOptions.Value.IsEnabled)
            {
                var thumbnailResult = await contentGenerator.GenerateImageAsync(roadmap.Title, true);
                if (!thumbnailResult.Success)
                    return Error.ServerError($"{thumbnailResult.Error}");

                string? fileName = Path.GetFileNameWithoutExtension(thumbnailResult.FileName);

                roadmap.ThumbnailUrl = fileName;
                await dbContext.SaveChangesAsync(ct);
            }
            
            await transaction.CommitAsync(ct);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            return Error.ServerError(ex.Message);
        }

        return roadmap.Adapt<RoadmapModel>();
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
            $"Add modules and lessons for the course '{roadmap.Title}', estimated duration in minutes: {roadmap.EstimatedDuration}.";

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

    private static string GetLessonSystemMessage()
    {
        return """
               You are a professional course creation assistant. Your task is to generate educational content in the following JSON format:
               {
                 "content": {
                   "mainContent": "Write natural educational content using HTML tags only when meaningful. Use <h2> for titles, <strong> for important terms, <blockquote> A complete insight about the topic goes here. For short technical code examples, wrap in:</p><pre><code class='language-javascript'>code example</code></pre>, <ul> OR <ol> for lists (never mix, use <ul> for features, <ol> for steps). Do noy use links or images.",
                   "resources": ["Title 1 | Description 1 | URL 1", "Title 2 | Description 2 | URL 2"],
                   "examples": []
                 },
                 "quizzes": [
                   "question": "Clear, focused question testing key concept",
                   "answers": ["Option 1", "Option 2", "Option 3", "Option 4"],
                   "correctAnswerIndex": 0
                 ]
               }

               Guidelines:
               - mainContent length: 200-500 words. Natural writing style, tags only when meaningful
               - Keep content focused and practical
               - Avoid special characters or complex Unicode
               - Use Tiptap-compatible HTML for mainContent
               - Use code blocks only for technical examples. If topic is not technical, do not use code blocks. Code format <pre><code class='language-X'>
               - No \\n or escaped characters
               - Proper tag closing. Do not use self-closing tags and do not use <h2> with other tags. Be careful with line breaking lines, double check the format to avoid errors
               - If you use blockquote. Each should contain the complete insights/key points, not just labels
               - Include 1-2 relevant resources with titles. Format resources as: "Title | short Description | URL", all three parts required, use vertical bar (|) as separator in resources
               - Create 1 quiz question with 4 clear options. Be careful to set the correct index as answer for correctAnswerIndex field
               """;
    }

    private static string GetRoadmapSystemMessage()
    {
        return @"
You are a course-generation assistant. Return a JSON object with single-quoted keys and strings:
{
  'description': 'A concise summary of the course topic',
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

- If the course is generated successfully, provide a meaningful **short description** for the 'description' field. Do not use ""Course generated successfully.""
- If you encounter an issue generating the course, use the 'description' field to explain the issue.
- Generate at least 2 modules with at least 2 lessons, scaling up the number of modules and lessons based on 'EstimatedDuration' (2-6 modules, 2-3 lessons per module).
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