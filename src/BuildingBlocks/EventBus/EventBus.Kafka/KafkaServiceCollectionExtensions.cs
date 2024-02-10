using System.Reflection;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProducerConfig = Confluent.Kafka.ProducerConfig;

namespace EventBus.Kafka;

/// <summary>
/// Extension class for <see cref="IServiceCollection"/>.
/// </summary>
public static class KafkaServiceCollectionExtensions
{
    /// <summary>
    /// Add Kafka configuration and event bus in <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
    /// <param name="configuration">Instance of <see cref="IConfiguration"/>.</param>
    /// <example>
    /// Kafka configuration example which will be retrieved using <paramref name="configuration"/>: 
    /// <code>
    /// "KafkaOptions" :{
    ///     "BootstrapServers": "localhost:9092",
    ///     "MessageTimeoutMs": 3000
    /// }
    /// </code>
    /// </example>
    /// <returns>Modified Instance of <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddKafka(this IServiceCollection services,IConfiguration configuration)
    {
        var kafkaOptionsSection = configuration.GetSection("KafkaOptions");
        var bootstrapServers = kafkaOptionsSection["BootstrapServers"];

        var producerConfiguration = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            AllowAutoCreateTopics = true,
            MessageTimeoutMs = int.Parse(kafkaOptionsSection["MessageTimeoutMs"]!)
        };
        
        var consumerConfiguration = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = Assembly.GetExecutingAssembly().GetName().Name,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            AllowAutoCreateTopics = true,
            EnableAutoCommit = true
        };

        //Set up the event bus
        services.AddSingleton(new KafkaConnection(producerConfiguration, consumerConfiguration));
        services.AddSingleton<IEventBusSubscriptionManager, InMemoryEventBusSubscriptionManager>();

        services.AddSingleton<IEventBus, KafkaEventBus>(sp =>
        {
            var kafkaConnection = sp.GetRequiredService<KafkaConnection>();
            var logger = sp.GetRequiredService<ILogger<KafkaEventBus>>();
            var eventBusSubscriptionManager = sp.GetRequiredService<IEventBusSubscriptionManager>();
            return new KafkaEventBus(eventBusSubscriptionManager, logger, kafkaConnection, sp);
        });

        return services;
    } 
}