#if UNITY_IOS && UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;

namespace SpaceMonkey.Editor.BuildTool.iOS
{
    public class iOSExemptFromEncryption : IPostprocessBuildWithReport // Will execute after XCode project is built
    {
        public int callbackOrder => 1;

        public void OnPostprocessBuild(BuildReport report)
        {
            UnityEngine.Debug.Log($"iOSExemptFromEncryption: OnPostprocessBuild: Platform: {report.summary.platform}");
            if (report.summary.platform != BuildTarget.iOS) return; // Check if the build is for iOS 
            string plistPath = Path.Combine(report.summary.outputPath, "Info.plist");
            UnityEngine.Debug.Log($"iOSExemptFromEncryption: OnPostprocessBuild: plistPath: {plistPath}");
            PlistDocument plist = new PlistDocument(); // Read Info.plist file into memory
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDict = plist.root;
            rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);
            UnityEngine.Debug.Log(
                "iOSExemptFromEncryption: OnPostprocessBuild: ITSAppUsesNonExemptEncryption set to false");
            File.WriteAllText(plistPath, plist.WriteToString()); // Override Info.plist
            UnityEngine.Debug.Log("iOSExemptFromEncryption: OnPostprocessBuild: Info.plist updated");
        }
    }
}
#endif