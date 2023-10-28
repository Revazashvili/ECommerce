using System.Reflection;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProducerConfig = Confluent.Kafka.ProducerConfig;

namespace EventBus.Kafka;

public static class KafkaServiceCollectionExtensions
{
    public static IServiceCollection AddKafka(this IServiceCollection services,IConfiguration configuration)
    {
        var kafkaOptionsSection = configuration.GetSection("KafkaOptions");
        var bootstrapServers = kafkaOptionsSection["BootstrapServers"];

        var producerConfiguration = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            AllowAutoCreateTopics = true
        };
        
        var schemaRegistryConfiguration = new SchemaRegistryConfig
        {
            Url = kafkaOptionsSection["SchemaRegistryUrl"],
            RequestTimeoutMs = Convert.ToInt32(kafkaOptionsSection["SchemaRegistryTimeoutMs"]),
            MaxCachedSchemas = Convert.ToInt32(kafkaOptionsSection["SchemaRegistryMaxCachedSchemas"])
        };

        var consumerConfiguration = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = Assembly.GetExecutingAssembly().GetName().Name,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        //Set up the event bus
        services.AddSingleton(new KafkaConnection(producerConfiguration, consumerConfiguration,schemaRegistryConfiguration));
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