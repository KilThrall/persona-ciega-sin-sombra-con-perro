using System;
using System.Collections.Generic;
using UnityEngine;

public static class ActionsManager
{
    private static Dictionary<string, Action<object>> actions = new Dictionary<string, Action<object>>();

    public static void InvokeAction(string name, object obj)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to invoke {name}, but no action was found");
            return;
        }

        actions[name]?.Invoke(obj);
    }

    public static void RegisterAction(string name)
    {
        if (actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to register {name}, but it is already registered");
            return;
        }

        actions.Add(name, new Action<object>((object a) => { }));
    }

    public static void SubscribeToAction(string name, Action<object> action)
    {
        if (!actions.ContainsKey(name))
        {
            RegisterAction(name);
        }

        actions[name] += action;
    }

    public static void UnsubscribeToAction(string name, Action<object> action)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to unsubscribe to {name}, but no action was found");
            return;
        }

        actions[name] -= action;
    }

    public static void DeleteAction(string name)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to delete {name}, but no action was found");
            return;
        }

        actions[name] = null;
        actions.Remove(name);
    }

    public static void DeleteAllActions()
    {
        foreach (var item in actions)
        {
            DeleteAction(item.Key);
        }
    }
}

