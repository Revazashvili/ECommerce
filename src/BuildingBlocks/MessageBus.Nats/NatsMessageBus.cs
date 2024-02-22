using System.Text;
using System.Text.Json;
using NATS.Client;

namespace MessageBus.Nats;

internal class NatsMessageBus : IMessageBus
{
    private static readonly SemaphoreSlim ConnectionLock = new(initialCount: 1, maxCount: 1);
    private readonly NatsOptions _natsOptions;
    private IConnection? _connection;
    public NatsMessageBus(NatsOptions natsOptions)
    {
        _natsOptions = natsOptions;
        
        Connect();
    }

    public Task PublishAsync(string subject, object? payload = null)
    {
        Connect();

        var bytes = payload is null ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));
        _connection!.Publish(subject, bytes);
        
        return Task.CompletedTask;
    }

    private void Connect()
    {
        if (_connection is not null)
            return;

        ConnectionLock.Wait();

        try
        {
            if(_connection is not null)
                return;
            
            var defaultOptions = ConnectionFactory.GetDefaultOptions(_natsOptions.ServerUrl);
            defaultOptions.NoEcho = true;
            defaultOptions.NoRandomize = false;
            
            var cf = new ConnectionFactory();
            _connection = cf.CreateConnection(defaultOptions);
        }
        finally
        {
            ConnectionLock.Release();
        }

    }

    public void Dispose()
    {
        _connection?.DrainAsync();
        _connection?.Dispose();
    }
}