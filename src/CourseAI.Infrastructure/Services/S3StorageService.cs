using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Amazon.S3.Model;
using CourseAI.Application.Options;
using CourseAI.Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NeerCore.DependencyInjection;

namespace CourseAI.Infrastructure.Services;

[Service]
public class StorageService(IOptions<S3Options> options, ILogger<IStorageService> logger) : IStorageService
{
    private readonly IAmazonS3 _s3Client = new AmazonS3Client(
        options.Value.AccessKey,
        options.Value.SecretKey,
        new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(options.Value.Region)
        }
    );

    private readonly string _bucketName = options.Value.BucketName;
    private const int MaxVideoSize = 5 * 1024 * 1024; // 5MB

    public async Task<string> SaveVideoAsync(byte[] videoBytes, string fileName, string path)
    {
        try
        {
            // Ensure the filename has a video extension
            if (!fileName.EndsWith(".mp4"))
            {
                fileName = $"{fileName}.mp4";
            }

            // Prepare upload request
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{path}/{fileName}",
                InputStream = new MemoryStream(videoBytes),
                ContentType = "video/mp4",
            };

            // Add metadata using the Add method
            putRequest.Metadata.Add("ContentEncoding", "binary");
            putRequest.Metadata.Add("x-amz-meta-originalname", fileName);

            // For larger files, consider enabling multipart upload
            if (videoBytes.Length > MaxVideoSize) // 5MB threshold
            {
                putRequest.StreamTransferProgress += (sender, args) =>
                {
                    var progress = (double)args.TransferredBytes / args.TotalBytes * 100;
                    logger.LogInformation($"Upload progress: {progress:F2}%");
                };
            }

            // Upload to S3
            await _s3Client.PutObjectAsync(putRequest);

            // Return the URL
            return $"https://{_bucketName}.s3.amazonaws.com/{path}/{fileName}";
        }
        catch (Exception ex)
        {
            throw new StorageException($"Failed to save video to S3: {ex.Message}", ex);
        }
    }

    public async Task<string> SaveImageAsync(string base64Image, string fileName, string path)
    {
        try
        {
            // Convert base64 to bytes
            var imageBytes = Convert.FromBase64String(base64Image);

            // Prepare upload request
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{path}/{fileName}",
                InputStream = new MemoryStream(imageBytes),
                ContentType = "image/png", // Adjust based on your image type
                // CannedACL = S3CannedACL.PublicRead // Makes the image publicly accessible
            };

            // Upload to S3
            await _s3Client.PutObjectAsync(putRequest);

            // Return the public URL
            return $"https://{_bucketName}.s3.amazonaws.com/{path}/{fileName}";
        }
        catch (Exception ex)
        {
            throw new StorageException($"Failed to save image to S3: {ex.Message}", ex);
        }
    }

    public async Task DeleteImageAsync(string fileName)
    {
        try
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = $"thumbnails/{fileName}"
            };

            await _s3Client.DeleteObjectAsync(deleteRequest);
        }
        catch (Exception ex)
        {
            throw new StorageException($"Failed to delete image from S3: {ex.Message}", ex);
        }
    }
}

public class StorageException : Exception
{
    public StorageException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}