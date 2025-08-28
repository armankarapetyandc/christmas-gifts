using System;
using Newtonsoft.Json;
using SpaceMonkey.Editor.BuildTool.Exceptions;
using UnityEditor;
using UnityEngine;

namespace SpaceMonkey.Editor.BuildTool
{
    public class UnityCloudBuildScript
    {
        private const string APP_VERSION_KEY = "APP_VERSION";

        private static JsonSerializerSettings _serializerSettings { get; } = new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver(),
            NullValueHandling = NullValueHandling.Include, // Include null values in the output
            Formatting = Formatting.Indented // Makes the JSON output more readable
        };

        public static void Build(
#if UNITY_CLOUD_BUILD
            UnityEngine.CloudBuild.BuildManifestObject manifest
#endif
        )
        {
            try
            {
                Log("Running custom build script...");
#if UNITY_CLOUD_BUILD
                Log($"Build Manifest: {JsonConvert.SerializeObject(manifest, _serializerSettings)}");
#endif
                var version = GetEnvironmentVariable(APP_VERSION_KEY);
                Log($"Version: {version}");
                PlayerSettings.bundleVersion = version;

#if UNITY_CLOUD_BUILD
                var buildNumber = manifest.GetValue<int>("buildNumber");
                Log($"Build Number: {buildNumber}");
                ApplyBuildVersion(buildNumber);
#endif
                Log("Unity Cloud will run his build script...");
            }
            catch (Exception e)
            {
                Log(e.Message, true);
                EditorApplication.Exit(1);
            }
        }

        private static void ApplyBuildVersion(int buildNumber)
        {
            var currentTarget = EditorUserBuildSettings.activeBuildTarget;
            switch (currentTarget)
            {
                case BuildTarget.Android:
                    PlayerSettings.Android.bundleVersionCode = buildNumber;
                    Log($"Android bundle version code set to: {buildNumber}");
                    return;
                case BuildTarget.iOS:
                    PlayerSettings.iOS.buildNumber = buildNumber.ToString();
                    Log($"iOS build number set to: {buildNumber}");
                    break;
                default:
                    throw new NotImplementedException(
                        $"Apply Build Version not implemented for the current target: {currentTarget}");
            }
        }

        private static string GetEnvironmentVariable(string variableName)
        {
            string value = Environment.GetEnvironmentVariable(variableName);

            if (string.IsNullOrEmpty(value))
            {
                throw new UndefinedEnvironmentVariableException(variableName);
            }

            return value.Trim();
        }

        private static void Log(string message, bool error = false)
        {
            if (error)
            {
                Debug.LogError($"{nameof(UnityCloudBuildScript)}: {message}");
                return;
            }

            Debug.Log($"{nameof(UnityCloudBuildScript)}: {message}");
        }
    }
}