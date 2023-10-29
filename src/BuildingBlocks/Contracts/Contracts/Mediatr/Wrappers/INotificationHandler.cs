using MediatR;

namespace Contracts.Mediatr.Wrappers;

public interface INotificationHandler<in T> : MediatR.INotificationHandler<T>
    where T : INotification { }