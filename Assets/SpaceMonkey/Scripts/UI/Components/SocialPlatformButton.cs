using System;
using R3;
using SpaceMonkey.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    [RequireComponent(typeof(Button))]
    public class SocialPlatformButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private SocialPlatform platform;

        public Observable<SocialPlatform> Selected => button.OnClickAsObservable().Select(_ => platform);

#if UNITY_EDITOR
        private void OnValidate()
        {
            button = GetComponent<Button>();
        }
#endif
    }
}