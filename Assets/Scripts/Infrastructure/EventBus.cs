using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EventBus : IEventBus
{
    private readonly Dictionary<Type, List<Delegate>> _subs = new();
    public void Invoke<T>(T evt) where T : IEvent
    {
        if (_subs.TryGetValue(typeof(T), out var list))
            for (int i = 0; i < list.Count; i++)
                (list[i] as Action<T>)?.Invoke(evt);
    }

    public void Subscribe<T>(Action<T> handler) where T : IEvent
    {
        var t = typeof(T);
        if(!_subs.TryGetValue(t,out var list))
        {
            list = new();
            _subs[t] = list;
        }
        list.Add(handler);
    }

    public void Unsubscribe<T>(Action<T> handler) where T : IEvent
    {
        if (_subs.TryGetValue(typeof(T), out var list)) list.Remove(handler);
    }
}
