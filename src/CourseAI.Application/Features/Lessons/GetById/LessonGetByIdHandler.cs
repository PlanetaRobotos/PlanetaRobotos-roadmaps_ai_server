using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Services;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Roadmaps;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OneOf;

namespace CourseAI.Application.Features.Lessons.GetById;

public class LessonGetByIdHandler(AppDbContext dbContext, IContentGenerator contentGenerator) : IHandler<LessonGetByIdRequest, LessonModel>
{
    public async ValueTask<OneOf<LessonModel, Error>> Handle(LessonGetByIdRequest request, CancellationToken ct)
    {
        var lesson = await dbContext.Lessons
            .Where(e => e.Id == request.Id)
            .FirstOrDefaultAsync(ct);

        if (lesson is null)
        {
            return Error.NotFound<Lesson>();
        }

        // If content already exists, return it
        if (lesson.Content is not null)
        {
            return lesson.Adapt<LessonModel>();
        }

        // Content not generated yet, let's generate it
        var generatedContent = await GenerateLessonContentAsync(lesson);

        if (generatedContent == null)
        {
            return Error.NotFound<LessonContent>();
        }

        lesson.Content = generatedContent;
        await dbContext.SaveChangesAsync(ct);

        return lesson.Adapt<LessonModel>();
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
        return @"
You are a lesson-generation assistant. Return valid JSON using double quotes:
{
  'mainContent': '<h1>Lesson Title</h1>Content...',
  'resources': ['https://example.com'],
  'examples': [],
  'quizzes': []
}

If you cannot generate suitable lesson content, do not just say you're unable. Instead, provide a brief fallback explanation and at least one resource link where the user can learn more.
";
    }
}
