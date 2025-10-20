using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace SpaceMonkey.Scripts.Analytics.Service
{
    public interface IAnalyticsProvider
    {
        public UniTask SendEvent(string eventName, Dictionary<string, string> parameters);
    }
}