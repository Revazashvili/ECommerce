using Microsoft.Extensions.Hosting;
using Serilog;

namespace BuildingBlocks.Setup;

public static class SerilogHostBuilderExtensions
{
    public static IHostBuilder UseSerilogLogging(this IHostBuilder builder)
    {
        builder.UseSerilog((context, configuration) =>
        {
            configuration.WriteTo.Console();
        });

        return builder;
    }
}