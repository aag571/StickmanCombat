using System.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public static class WebBuild
{
    public static void PrepareAndBuild()
    {
        GameObjectNaming.Normalize();
        Build();
    }

    [MenuItem("Build/Build Web Game")]
    public static void Build()
    {
        PlayerSettings.WebGL.template = "PROJECT:Fullscreen";
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
        PlayerSettings.WebGL.decompressionFallback = true;
        PlayerSettings.WebGL.dataCaching = true;
        PlayerSettings.SplashScreen.show = false;
        PlayerSettings.SplashScreen.showUnityLogo = false;
        var scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
        var report = BuildPipeline.BuildPlayer(scenes, "Builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
        if (report.summary.result != BuildResult.Succeeded)
            throw new BuildFailedException("WebGL build failed: " + report.summary.result);

        WriteVersionManifest();
    }

    private static void WriteVersionManifest()
    {
        var version = PlayerSettings.bundleVersion;
        try
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "git";
                process.StartInfo.Arguments = "rev-parse --short HEAD";
                process.StartInfo.WorkingDirectory = Directory.GetParent(Application.dataPath).FullName;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                var output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();
                if (!string.IsNullOrEmpty(output)) version = output;
            }
        }
        catch (Exception) { }

        if (string.IsNullOrEmpty(version)) version = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var json = "{\n  \"version\": \"" + version + "\",\n  \"build\": \"" + DateTime.UtcNow.ToString("o") + "\"\n}\n";
        File.WriteAllText(Path.Combine("Builds", "WebGL", "version.json"), json, new UTF8Encoding(false));
    }
}
