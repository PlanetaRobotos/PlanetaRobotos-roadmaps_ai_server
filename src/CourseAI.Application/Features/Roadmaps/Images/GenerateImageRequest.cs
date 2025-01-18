using System.ComponentModel.DataAnnotations;

namespace CourseAI.Application.Features.Roadmaps.Images;

public class GenerateImageRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(1000)]
    public string Prompt { get; set; } = string.Empty;

    // Optional parameters you might want to add:
    public int? Width { get; set; } // = 512 by default in service
    public int? Height { get; set; } // = 512 by default in service
    public string? StylePreset { get; set; } // = "digital-art" by default in service
    public string? FileName { get; set; }
    public string? SavePath { get; set; }
}