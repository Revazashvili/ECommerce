namespace BuildingBlocks.Swagger;

public class SwaggerOptions
{
    public string Version { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ClientId { get; set; }
    public string AppName { get; set; }
    public string IdentityServerUrl { get; set; }
    public Dictionary<string, string> IdentityServerScopes { get; set; }
}