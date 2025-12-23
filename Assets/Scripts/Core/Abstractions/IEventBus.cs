
using System;

public interface IEventBus
{
    public void Invoke<T>(T evt) where T : IEvent;
    public void Subscribe<T>(Action<T> handler) where T : IEvent;
    public void Unsubscribe<T>(Action<T> handler) where T : IEvent;
}
