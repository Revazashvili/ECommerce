namespace Report.API.Endpoints;

internal static class ApiEndpointsMapper
{
    internal static IEndpointRouteBuilder MapApi(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var apiRouteGroupBuilder = endpointRouteBuilder.MapGroup("api");
        return apiRouteGroupBuilder;
    }
}

internal static class ReportEndpointsMapper
{
    internal static void MapReport(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var basketRouteGroupBuilder = endpointRouteBuilder.MapGroup("report");
    }
}