using System;
using SpaceMonkey.Editor.BuildTool.Exceptions;
using UnityEditor;
using UnityEngine;

namespace SpaceMonkey.Editor.BuildTool
{
    public class UnityCloudBuildScript
    {
        public static void Build()
        {
            try
            {
                Log("Running custom build script...");
                var version = GetEnvironmentVariable("APP_VERSION");
                var buildNumber = GetEnvironmentVariable("BUILD_NUMBER");
                Log($"Version: {version}, Build Number: {buildNumber}");

                PlayerSettings.bundleVersion = version;
                ApplyBuildVersion(buildNumber);
            }
            catch (Exception e)
            {
                Log(e.Message, true);
                EditorApplication.Exit(1);
            }
        }

        private static void ApplyBuildVersion(string buildNumber)
        {
            var currentTarget = EditorUserBuildSettings.activeBuildTarget;
            switch (currentTarget)
            {
                case BuildTarget.Android:
                    if (!int.TryParse(buildNumber, out int bundleVersionCode))
                        throw new InvalidBuildNumberException(buildNumber);
                    PlayerSettings.Android.bundleVersionCode = bundleVersionCode;
                    Log($"Android version code set to: {bundleVersionCode}");
                    return;
                case BuildTarget.iOS:
                    PlayerSettings.iOS.buildNumber = buildNumber;
                    Log($"iOS build number set to: {buildNumber}");
                    break;
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