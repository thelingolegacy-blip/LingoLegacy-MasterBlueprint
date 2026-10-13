#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
namespace CrazyWeasol.Editor {
 public static class BuildScript {
  public static void PerformBuild() {
   const string output = "build/CrazyWeasol-CW-B01.x86_64";
   Directory.CreateDirectory("build");
   var report = BuildPipeline.BuildPlayer(new[] { "Assets/Scenes/CW_B01_GnarlLabs.unity" }, output, BuildTarget.StandaloneLinux64, BuildOptions.StrictMode);
   Debug.Log($"[CW-B01][BUILD] result={report.summary.result} errors={report.summary.totalErrors} warnings={report.summary.totalWarnings} size={report.summary.totalSize}");
   if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded) throw new System.Exception("CW-B01 Unity build failed.");
  }
 }
}
#endif
