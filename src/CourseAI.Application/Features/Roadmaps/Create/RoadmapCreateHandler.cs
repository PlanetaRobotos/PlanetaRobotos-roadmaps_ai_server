using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Services;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities;
using CourseAI.Domain.Entities.Roadmaps;
using Mapster;
using Newtonsoft.Json;
using OneOf;

namespace CourseAI.Application.Features.Roadmaps.Create
{
    public class RoadmapCreateHandler(AppDbContext dbContext, IContentGenerator aiContentGenerator)
        : IHandler<RoadmapCreateRequest, RoadmapModel>
    {
        public async ValueTask<OneOf<RoadmapModel, Error>> Handle(RoadmapCreateRequest request, CancellationToken ct)
        {
            var roadmap = request.ToEntity();

            dbContext.Roadmaps.Add(roadmap);
            await dbContext.SaveChangesAsync(ct);

            var aiResponse = await GenerateModulesAndLessonsAsync(roadmap);

            if (string.IsNullOrWhiteSpace(aiResponse))
            {
                return Error.ServerError("Failed to generate roadmap modules and lessons.");
            }

            var updated = await AttachModulesAndLessonsAsync(roadmap, aiResponse, ct);

            if (!updated)
            {
                return Error.ServerError("Failed to attach generated modules and lessons to the roadmap.");
            }

            return roadmap.Adapt<RoadmapModel>();
        }

        private async Task<string?> GenerateModulesAndLessonsAsync(Roadmap roadmap)
        {
            var userPrompt = $"Add modules and lessons for the roadmap '{roadmap.Title}', estimated duration in minutes: {roadmap.EstimatedDuration}.";

            var aiResponse = await aiContentGenerator.GenerateContentAsync(userPrompt,
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
          'order': 1
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
}
