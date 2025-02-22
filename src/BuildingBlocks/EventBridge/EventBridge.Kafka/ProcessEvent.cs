using Confluent.Kafka;

namespace EventBridge.Kafka;

internal delegate Task ProcessEvent(ConsumeResult<string,string> consumeResult, IServiceProvider serviceProvider, CancellationToken cancellationToken);