using Microsoft.AspNetCore.Http;

namespace Products.Application.Services;

public interface IImageService
{
    Task<string> UploadAsync(Guid guid,string file,CancellationToken cancellationToken);
}