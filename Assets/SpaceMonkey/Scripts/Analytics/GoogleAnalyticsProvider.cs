using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Analytics.Service;

namespace SpaceMonkey.Scripts.Analytics
{
    public class GoogleAnalyticsProvider : IAnalyticsProvider
    {
        public UniTask SendEvent(string eventName, Dictionary<string, string> parameters)
        {
            Firebase.Analytics.Parameter[] parametersArray = parameters.Select(p =>
                new Firebase.Analytics.Parameter(p.Key, p.Value)).ToArray();
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, parametersArray);
            return UniTask.CompletedTask;
        }
    }
}