using System;
using System.Collections.Generic;

public class UiStateService : IUiStateService
{
    private readonly Dictionary<Type, object> _states = new Dictionary<Type, object>();

    public void Set<T>(T state) where T : class
    {
        _states[typeof(T)] = state;
    }

    public T Get<T>() where T : class
    {
        object state;
        return _states.TryGetValue(typeof(T), out state) ? (T)state : null;
    }

    public void Clear<T>() where T : class
    {
        _states.Remove(typeof(T));
    }
}