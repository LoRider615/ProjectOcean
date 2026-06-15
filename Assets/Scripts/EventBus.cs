using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<Type, List<Delegate>> subs = new();

    public static void Subscribe<T>(Action<T> handler) where T : IGameEvent
    {
        Type t = typeof(T);
        if (!subs.TryGetValue(t, out List<Delegate> list))
        {
            list = new();
            subs[t] = list;
        }
        list.Add(handler);
    }
}
