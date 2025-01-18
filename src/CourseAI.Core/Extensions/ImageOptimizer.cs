using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace CourseAI.Core.Extensions;

public static class ImageOptimizer
{
    public static byte[] OptimizeImage(byte[] imageBytes, int maxWidth = 300, int maxHeight = 300)
    {
        using var image = Image.Load(imageBytes);

        // Resize if needed
        if (image.Width > maxWidth || image.Height > maxHeight)
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(maxWidth, maxHeight)
            }));
        }

        // Optimize and save
        using var ms = new MemoryStream();
        var encoder = new PngEncoder
        {
            CompressionLevel = PngCompressionLevel.BestCompression,
            FilterMethod = PngFilterMethod.Adaptive
        };

        image.Save(ms, encoder);
        return ms.ToArray();
    }
}