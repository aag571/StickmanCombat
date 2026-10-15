using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using StickmanCombat.Gameplay;

public static class GameObjectNaming
{
    [MenuItem("Tools/Stickman Combat/Normalize UI Object Names")]
    public static void Normalize()
    {
        var prefab = PrefabUtility.LoadPrefabContents("Assets/UI/GameFlowUI.prefab");
        Rename(prefab.GetComponent<GameFlowView>());
        PrefabUtility.SaveAsPrefabAsset(prefab, "Assets/UI/GameFlowUI.prefab");
        PrefabUtility.UnloadPrefabContents(prefab);
        foreach (string sceneName in new[] { "MainMenu", "Game" })
        {
            var scene = EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity");
            foreach (var view in scene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<GameFlowView>(true))) Rename(view);
            foreach (var root in scene.GetRootGameObjects())
                foreach (var child in root.GetComponentsInChildren<Transform>(true))
                    if (child.name.Any(character => character > 127)) throw new InvalidOperationException("Non-English GameObject name: " + child.name);
            EditorSceneManager.SaveScene(scene);
        }
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
        AssetDatabase.SaveAssets();
        Debug.Log("GameObject names normalized. UI text is independent of object names.");
    }

    private static void Rename(GameFlowView view)
    {
        view.settingsButton.name = "SettingsButton";
        view.tutorialButton.name = "TutorialButton";
        view.rankedButton.name = "RankedButton";
        view.endlessButton.name = "EndlessButton";
        view.equipmentButton.name = "EquipmentButton";
        view.equipmentClose.name = "CloseEquipmentButton";
        view.settingsClose.name = "CloseSettingsButton";
        view.settingsExit.name = "ExitGameButton";
        view.replayButton.name = "ReplayButton";
        view.menuButton.name = "MainMenuButton";
        view.unlockClose.name = "CloseUnlocksButton";
        string[] toggleNames = { "MusicToggle", "SoundToggle", "BloodToggle", "WeatherToggle" };
        for (int index = 0; index < view.toggleButtons.Length; index++) view.toggleButtons[index].name = toggleNames[index];
        foreach (var button in view.mobileControls.GetComponentsInChildren<MobileCombatButton>(true))
            button.name = button.side < 0 ? "MoveLeftButton" : "MoveRightButton";
    }
}
