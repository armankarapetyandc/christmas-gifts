#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SpaceMonkey.Scripts.Cloud.Tool
{
    public static class CloudMenuTools
    {
        private const string EditorCloudPrefKey = "use_remote";

        [MenuItem("Space Monkey/Cloud/Use Remote", true)]
        private static bool UseRemoteMenuValidation() => !GetState();

        [MenuItem("Space Monkey/Cloud/Use Remote")]
        private static void UseRemoteMenu()
        {
            EditorPrefs.SetBool(EditorCloudPrefKey, true);
            Debug.Log("Using remote config");
        }

        [MenuItem("Space Monkey/Cloud/Use Local", true)]
        private static bool UseLocalMenuValidation() => GetState();

        [MenuItem("Space Monkey/Cloud/Use Local")]
        private static void UseLocalMenu()
        {
            EditorPrefs.SetBool(EditorCloudPrefKey, false);
            Debug.Log("Using local config");
        }

        public static bool GetState() => EditorPrefs.GetBool(EditorCloudPrefKey, false);
    }
}
#endif