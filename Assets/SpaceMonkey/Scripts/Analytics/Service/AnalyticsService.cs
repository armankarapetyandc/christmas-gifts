using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Analytics.Service
{
    public class AnalyticsService : IInitializable, IDisposable
    {
        private static Subject<(string eventName, Dictionary<string, string> parameters, Type[] types)>
            _callEventSubject;

        private readonly List<IAnalyticsProvider> _providers;
        private CompositeDisposable _compositeDisposable;

        public AnalyticsService(List<IAnalyticsProvider> providers)
        {
            _providers = providers;
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }

        public void Initialize()
        {
            _compositeDisposable = new CompositeDisposable();
            _callEventSubject = new Subject<(string eventName, Dictionary<string, string> parameters, Type[] types)>();
            _callEventSubject.Subscribe(SendEvent).AddTo(_compositeDisposable);
        }

        public static void SendEvent(string eventName, Dictionary<string, string> parameters, Type[] types)
        {
            _callEventSubject.OnNext((eventName, parameters, types));
        }

        private void SendEvent((string eventName, Dictionary<string, string> parameters, Type[] types) data)
        {
            try
            {
#if UNITY_EDITOR
                if (data.types?.Length > 0)
                {
                    Type interfaceType = typeof(IAnalyticsProvider);
                    foreach (var dataType in data.types)
                    {
                        if (!dataType.GetInterfaces()
                                .Any(i => i == interfaceType
                                          || i.IsGenericType
                                          && i.GetGenericTypeDefinition() == interfaceType))
                        {
                            Debug.LogError($"Type {dataType} is not IAnalyticsProvider");
                        }
                    }
                }
#endif

                foreach (var provider in _providers)
                {
                    if (data.types?.Length > 0 && !data.types.Contains(provider.GetType()))
                    {
                        continue;
                    }

                    Debug.Log($"Sending event with name: {data.eventName} to provider: {provider.GetType()}");
                    provider.SendEvent(data.eventName, data.parameters);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}