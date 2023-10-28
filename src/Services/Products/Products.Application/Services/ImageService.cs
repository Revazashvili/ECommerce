using BlobHelper;
using Microsoft.AspNetCore.Http;

namespace Products.Application.Services;

public class ImageService : IImageService
{
    private readonly BlobClient _blobClient;

    public ImageService(BlobClient blobClient) => _blobClient = blobClient;

    public async Task<string> UploadAsync(Guid guid,string file,CancellationToken cancellationToken)
    {
        var key = guid.ToString();
        var contentType = $"image/{GetFileExtension(file)}";
        await _blobClient.Write(key,contentType,file,cancellationToken);
        
        return _blobClient.GenerateUrl(key, cancellationToken);
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