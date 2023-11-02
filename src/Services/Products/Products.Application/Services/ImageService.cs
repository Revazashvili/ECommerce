using Amazon.S3;
using Amazon.S3.Transfer;

namespace Products.Application.Services;

public interface IImageService
{
    Task<string> UploadAsync(Guid guid,string file,CancellationToken cancellationToken);
}

public class ImageService : IImageService
{
    private readonly IAmazonS3 _amazonS3;

    public ImageService(IAmazonS3 amazonS3) => _amazonS3 = amazonS3;

    public async Task<string> UploadAsync(Guid guid,string file,CancellationToken cancellationToken)
    {
        const string bucketName = "ecommerce-microservices";
        
        var key = guid.ToString();
        var transferUtility = new TransferUtility(_amazonS3);
        var imageBytes = Convert.FromBase64String(file);
        using (var stream = new MemoryStream(imageBytes))
        {
            await transferUtility.UploadAsync(stream, bucketName, key, cancellationToken);
        }

        return $"https://{bucketName}.s3.amazonaws.com/{key}";
    }
    
    private static string GetFileExtension(string base64String)
    {
        return base64String[..5].ToUpper() switch
        {
            "IVBOR" => "png",
            "/9J/4" => "jpg",
            _ => string.Empty
        };
    }

}