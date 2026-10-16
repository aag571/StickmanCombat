using System.Linq;
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
        PlayerSettings.SplashScreen.logoStyle = UnityEditor.Rendering.SplashScreenLogoStyle.DarkOnLight;
        var scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
        var report = BuildPipeline.BuildPlayer(scenes, "Builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
        if (report.summary.result != BuildResult.Succeeded)
            throw new BuildFailedException("WebGL build failed: " + report.summary.result);
    }
}
