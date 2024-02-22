namespace MessageBus.Nats;

public class NatsOptions(string serverUrl)
{
    public string ServerUrl { get; } = serverUrl;
}