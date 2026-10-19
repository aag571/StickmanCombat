using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

[Preserve]
public static class SkipSplashScreen
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void Initialize()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
#endif
    }
}
