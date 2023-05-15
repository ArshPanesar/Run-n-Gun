using System;
using System.Collections.Generic;
using UnityEngine;

// Events For This Game
public static class GameEvents
{
    public static string CollectCoins = "Collect Coins";
    public static string DestroyBullet = "Destroy Bullet";
    public static string PlayerDead = "Player Dead";
};

public class EventManager
{
    // Key:     Name of Event
    // Value:   Table of Parameters (Name, Value)
    private Dictionary<string, Action<Dictionary<string, object>>> eventDictionary;

    // Singleton
    private static EventManager instance = null;
    public static EventManager GetInstance()
    {
        if (instance == null)
        {
            instance = new EventManager();
            instance.eventDictionary = new Dictionary<string, Action<Dictionary<string, object>>>();
        }
        return instance;
    }

    public void AddListener(string name, Action<Dictionary<string, object>> listener)
    {
        Action<Dictionary<string, object>> currEvent;
        if (eventDictionary.TryGetValue(name, out currEvent))
        {
            currEvent += listener;
            eventDictionary[name] = currEvent;
        }
        else
        {
            currEvent += listener;
            eventDictionary.Add(name, currEvent);
        }
    }

    public void RemoveListener(string name, Action<Dictionary<string, object>> listener)
    {
        Action<Dictionary<string, object>> currEvent;
        if (eventDictionary.TryGetValue(name, out currEvent))
        {
            currEvent -= listener;
            eventDictionary[name] = currEvent;
        }
    }

    public void TriggerEvent(string name, Dictionary<string, object> args)
    {
        Action<Dictionary<string, object>> newEvent;
        if (eventDictionary.TryGetValue(name, out newEvent))
        {
            if (newEvent.GetInvocationList().Length > 0)
            {
                newEvent.Invoke(args);
            }
        }
    }
}
