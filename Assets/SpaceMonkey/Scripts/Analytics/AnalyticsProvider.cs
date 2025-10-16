using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using SpaceMonkey.Scripts.Analytics.Service;
using UnityEngine;

namespace SpaceMonkey.Scripts.Analytics
{
    public class AnalyticsProvider : IDisposable
    {
        private static Subject<(string eventName, Dictionary<string, string> parameters, Type[] types)>
            _callEventSubject;

        private readonly CompositeDisposable _compositeDisposable;

        public AnalyticsProvider()
        {
            _compositeDisposable = new CompositeDisposable();
            _callEventSubject = new Subject<(string eventName, Dictionary<string, string> parameters, Type[] types)>();
            _callEventSubject.Subscribe(ProcessEventSend).AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }

        private Dictionary<string, string> GetBaseParams()
        {
            return new Dictionary<string, string>
            {
                {AnalyticsParams.Version, Application.version}
            };
        }

        public void Initialize(string userId)
        {
            // Initialize the analytics service
        }

        public static void SendEvent(string eventName, Dictionary<string, string> eventParams = null,
            params Type[] types)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                Debug.LogError("Analytics event name is null or empty!");
                return;
            }

            eventParams ??= new Dictionary<string, string>();
            _callEventSubject?.OnNext((eventName, eventParams, types));
        }

        private void ProcessEventSend((string eventName, Dictionary<string, string> parameters, Type[] types) eventData)
        {
            var eventParams = eventData.Item2.ToDictionary(pair => pair.Key, pair => pair.Value);

            var baseParams = GetBaseParams();
            foreach (var baseParam in baseParams) eventParams.Add(baseParam.Key, baseParam.Value);
            AnalyticsService.SendEvent(eventData.eventName, eventParams, eventData.types);
        }
    }
}