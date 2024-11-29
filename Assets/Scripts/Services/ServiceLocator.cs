using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static Dictionary<string, Service> _localServices;
    private static Dictionary<string, OnRegisterComplete> _waiter;
    public delegate void OnRegisterComplete(Service service);
    static ServiceLocator()
    {
        _localServices = new Dictionary<string, Service>();
        _waiter = new Dictionary<string, OnRegisterComplete>();
    }

    public static void Register<T>(T service) where T : Service
    {
        string serviceName = service.GetType().Name;
        if (_localServices.ContainsKey(serviceName))
        {
            throw new Exception("Service {0} was register!".FormatHelper(serviceName));
        }
        service.Init();
        _localServices.Add(serviceName, service);
        if (_waiter.ContainsKey(serviceName))
        {
            _waiter[serviceName]?.Invoke(service);
            _waiter.Remove(serviceName);
        }
    }

    public static void Resolve<T>(OnRegisterComplete onCompleted) where T : Service
    {
        string serviceName = typeof(T).Name;
        if (!_localServices.ContainsKey(serviceName))
        {
            if (!_waiter.ContainsKey(serviceName))
            {
                _waiter.Add(serviceName, onCompleted);
            }
            else
            {
                _waiter[serviceName] += onCompleted;
            }
            return;
        }
        onCompleted.Invoke(_localServices[serviceName]);
    }

    public static T Resolve<T>() where T : Service
    {
        string serviceName = typeof(T).Name;
        if (!_localServices.ContainsKey(serviceName))
        {
            return default(T);
        }
        try
        {
            return (T)Convert.ChangeType(_localServices[serviceName], typeof(T));
        }
        catch (InvalidCastException)
        {
            return default(T);
        }
    }
}
