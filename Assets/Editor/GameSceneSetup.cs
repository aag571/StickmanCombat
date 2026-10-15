using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using StickmanCombat.Gameplay;

public static class GameSceneSetup
{
    public static void CreateScenes()
    {
        var source = EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
        var container = new GameObject("CombatSceneRoot");
        foreach (GameObject root in source.GetRootGameObjects()) if (root != container) root.transform.SetParent(container.transform, true);
        foreach (Canvas canvas in container.GetComponentsInChildren<Canvas>(true))
            if (canvas.name == "UI" || canvas.name == "ItemsMenuUI" || canvas.name == "ScreenshotItems") canvas.gameObject.SetActive(false);
        foreach (Tutorial tutorial in container.GetComponentsInChildren<Tutorial>(true)) tutorial.enabled = false;
        PrefabUtility.SaveAsPrefabAsset(container, "Assets/Resources/CombatSceneRoot.prefab");
        foreach (string sceneName in new[] { "MainMenu", "Game" })
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/CombatSceneRoot.prefab"));
            var flowObject = new GameObject("GameFlow");
            var flow = flowObject.AddComponent<GameFlowController>();
            flow.view = flowObject.AddComponent<GameFlowView>();
            flow.view.font = AssetDatabase.LoadAssetAtPath<Font>("Assets/Font/FONT.ttf");
            EditorSceneManager.SaveScene(scene, "Assets/Scenes/" + sceneName + ".unity");
        }
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene("Assets/Scenes/MainMenu.unity", true), new EditorBuildSettingsScene("Assets/Scenes/Game.unity", true) };
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
        AssetDatabase.SaveAssets();
    }
}
