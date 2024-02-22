namespace MessageBus;

public interface IMessageBus : IDisposable
{
    Task PublishAsync(string subject, object? payload = null);
}