using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using StickmanCombat.Gameplay;

[InitializeOnLoad]
public sealed class StaticGameFlowUI
{
    private RectTransform root;
    private Font font;
    private Sprite paper;
    private Sprite brushFrame;
    private Sprite brushButton;
    static StaticGameFlowUI() { EditorApplication.delayCall += EnsureLayout; }
    private static void EnsureLayout()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating)
        {
            EditorApplication.delayCall += EnsureLayout;
            return;
        }
        if (AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UI/GameFlowUI.prefab") == null) Create();
    }
    [MenuItem("Tools/Stickman Combat/Create Static UI")]
    public static void Create()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        if (AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UI/GameFlowUI.prefab") != null
            && !EditorUtility.DisplayDialog("重新生成静态 UI", "将覆盖 GameFlowUI 预制体及两个场景中该 UI 的布局修改。是否继续？", "重新生成", "取消")) return;
        var builder = new StaticGameFlowUI();
        builder.Build();
    }
    private void Build()
    {
        font = AssetDatabase.LoadAssetAtPath<Font>("Assets/Font/FONT.ttf");
        paper = ImportArt("Paper");
        brushFrame = ImportArt("BrushFrame");
        brushButton = ImportArt("BrushButton");
        var holder = new GameObject("GameFlowUI", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = holder.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        var scaler = holder.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280, 720);
        scaler.matchWidthOrHeight = .5f;
        root = Rect(holder.transform, "SafeArea", new Vector2(.5f,.5f), Vector2.zero, Vector2.zero);
        root.anchorMin = Vector2.zero; root.anchorMax = Vector2.one;
        root.offsetMin = root.offsetMax = Vector2.zero;
        var view = holder.AddComponent<GameFlowView>();
        view.font = font; view.safeArea = root; view.scaler = scaler;
        view.hud = Label(root,"最高积分 0",new Vector2(0,1),new Vector2(28,-24),new Vector2(820,130),28);
        view.hud.alignment = TextAnchor.UpperLeft;
        view.equipped = Label(root,"当前武器",Vector2.zero,new Vector2(28,48),new Vector2(850,65),22);
        view.equipped.alignment = TextAnchor.MiddleLeft;
        view.settingsButton = Button(root,"设置",new Vector2(1,1),new Vector2(-90,-42),new Vector2(140,54),null,"SettingsButton");
        view.main = Panel("MainMenu",new Vector2(450,550));
        Label(view.main.transform,"暗影格斗",new Vector2(.5f,1),new Vector2(0,-56),new Vector2(420,75),44);
        view.rankLabel = Label(view.main.transform,"青铜 1 星",new Vector2(.5f,1),new Vector2(0,-125),new Vector2(410,50),27);
        view.tutorialButton = Button(view.main.transform,"新手教程",new Vector2(.5f,1),new Vector2(0,-205),new Vector2(310,62),null,"TutorialButton");
        view.rankedButton = Button(view.main.transform,"正式游戏",new Vector2(.5f,1),new Vector2(0,-285),new Vector2(310,62),null,"RankedButton");
        view.endlessButton = Button(view.main.transform,"无尽游戏",new Vector2(.5f,1),new Vector2(0,-365),new Vector2(310,62),null,"EndlessButton");
        view.equipmentButton = Button(view.main.transform,"武器装备",new Vector2(.5f,1),new Vector2(0,-445),new Vector2(310,62),null,"EquipmentButton");
        view.mobileControls = Rect(root,"MobileControls",new Vector2(.5f,.5f),Vector2.zero,Vector2.zero).gameObject;
        var mobileRect = view.mobileControls.GetComponent<RectTransform>();
        mobileRect.anchorMin=Vector2.zero; mobileRect.anchorMax=Vector2.one; mobileRect.offsetMin=mobileRect.offsetMax=Vector2.zero;
        foreach(int side in new[]{-1,1})
        {
            var button = Button(view.mobileControls.transform,side == -1 ? "◀ 左" : "右 ▶",new Vector2(side == -1 ? 0 : 1,0),new Vector2(side == -1 ? 105 : -105,90),new Vector2(165,125),null,side == -1 ? "MoveLeftButton" : "MoveRightButton");
            button.gameObject.AddComponent<MobileCombatButton>().side=side;
        }
        view.result = Panel("Result",new Vector2(620,370));
        view.resultTitle=Label(view.result.transform,"挑战成功",new Vector2(.5f,1),new Vector2(0,-65),new Vector2(580,70),38);
        view.resultDetail=Label(view.result.transform,"结算信息",new Vector2(.5f,1),new Vector2(0,-145),new Vector2(580,60),28);
        view.replayButton=Button(view.result.transform,"再来一局",new Vector2(.5f,0),new Vector2(-140,80),new Vector2(230,65),null,"ReplayButton");
        view.menuButton=Button(view.result.transform,"返回主菜单",new Vector2(.5f,0),new Vector2(140,80),new Vector2(230,65),null,"MainMenuButton");
        view.settings=Panel("Settings",new Vector2(600,550));
        Label(view.settings.transform,"设置",new Vector2(.5f,1),new Vector2(0,-45),new Vector2(500,60),34);
        view.toggleButtons=new Button[4];
        string[] names={"音乐","音效","血液效果","天气"};
        string[] toggleNames={"MusicToggle","SoundToggle","BloodToggle","WeatherToggle"};
        for(int index=0;index<4;index++) view.toggleButtons[index]=Button(view.settings.transform,names[index]+"：开",new Vector2(.5f,1),new Vector2(index<2 ? -175 : 0,-125-index*70),new Vector2(index<2 ? 180 : 380,56),null,toggleNames[index]);
        view.musicVolume=Volume(view.settings.transform,new Vector2(75,-125),"MusicVolume",out view.musicPercent);
        view.soundVolume=Volume(view.settings.transform,new Vector2(75,-195),"SoundVolume",out view.soundPercent);
        view.settingsClose=Button(view.settings.transform,"关闭 / 继续",new Vector2(.5f,0),new Vector2(-125,65),new Vector2(220,58),null,"CloseSettingsButton");
        view.settingsExit=Button(view.settings.transform,"退出本局",new Vector2(.5f,0),new Vector2(125,65),new Vector2(220,58),null,"ExitGameButton");
        view.equipment=Panel("Equipment",new Vector2(1000,620));
        Label(view.equipment.transform,"武器装备",new Vector2(.5f,1),new Vector2(0,-40),new Vector2(850,60),34);
        view.equipmentClose=Button(view.equipment.transform,"关闭",new Vector2(1,1),new Vector2(-65,-40),new Vector2(100,48),null,"CloseEquipmentButton");
        var viewport=Rect(view.equipment.transform,"Viewport",new Vector2(.5f,.5f),new Vector2(0,-25),new Vector2(940,490));
        viewport.gameObject.AddComponent<Image>().color=new Color(0,0,0,.15f);
        viewport.gameObject.AddComponent<RectMask2D>();
        var scroll=viewport.gameObject.AddComponent<ScrollRect>(); scroll.horizontal=false; scroll.viewport=viewport;
        var content=Rect(viewport,"Weapons",new Vector2(.5f,1),Vector2.zero,new Vector2(940,8*230));
        content.pivot=new Vector2(.5f,1); scroll.content=content;
        view.cards=new EquipmentCardView[22];
        for(int index=0;index<view.cards.Length;index++)
        {
            var button=Button(content,"EquipmentSlot"+index,new Vector2(0,1),new Vector2(12+index%3*310,-12-index/3*230),new Vector2(295,215),null);
            UnityEngine.Object.DestroyImmediate(button.GetComponentInChildren<Text>().gameObject);
            var card=button.gameObject.AddComponent<EquipmentCardView>(); card.button=button;
            card.icon=Rect(button.transform,"EquipmentIcon",new Vector2(0,1),new Vector2(12,-12),new Vector2(70,70)).gameObject.AddComponent<Image>();
            card.icon.preserveAspect=true;card.icon.raycastTarget=false;
            card.title=Label(button.transform,"武器名称",new Vector2(0,1),new Vector2(92,-15),new Vector2(195,60),20);card.title.name="Title";card.title.alignment=TextAnchor.UpperLeft;
            card.description=Label(button.transform,"攻击距离与特效",new Vector2(0,1),new Vector2(12,-88),new Vector2(270,86),17);card.description.name="Description";card.description.alignment=TextAnchor.UpperLeft;
            card.status=Label(button.transform,"解锁状态",new Vector2(.5f,0),new Vector2(0,22),new Vector2(270,35),20);card.status.name="Status";
            view.cards[index]=card;
        }
        view.unlocks=Panel("Unlocks",new Vector2(720,400));
        Label(view.unlocks.transform,"新武器已解锁",new Vector2(.5f,1),new Vector2(0,-55),new Vector2(660,70),36);
        view.unlockDetail=Label(view.unlocks.transform,"解锁武器名称",new Vector2(.5f,.5f),Vector2.zero,new Vector2(660,180),25);
        view.unlockClose=Button(view.unlocks.transform,"知道了",new Vector2(.5f,0),new Vector2(0,55),new Vector2(260,60),null,"CloseUnlocksButton");
        foreach(var panel in new[]{view.settings,view.equipment,view.result,view.unlocks,view.mobileControls}) panel.SetActive(false);
        var prefab=PrefabUtility.SaveAsPrefabAsset(holder,"Assets/UI/GameFlowUI.prefab");
        UnityEngine.Object.DestroyImmediate(holder);
        foreach(string sceneName in new[]{"MainMenu","Game"})
        {
            var scene=EditorSceneManager.OpenScene("Assets/Scenes/"+sceneName+".unity",OpenSceneMode.Additive);
            GameFlowController flow=null;
            foreach(var sceneRoot in scene.GetRootGameObjects())
            {
                flow=sceneRoot.GetComponentInChildren<GameFlowController>(true);
                if(flow!=null) break;
            }
            if(flow==null) throw new InvalidOperationException("Missing GameFlow: "+sceneName);
            var oldView=flow.GetComponent<GameFlowView>();
            if(oldView!=null) UnityEngine.Object.DestroyImmediate(oldView);
            var existing=flow.transform.Find("GameFlowUI");
            if(existing!=null) UnityEngine.Object.DestroyImmediate(existing.gameObject);
            var instance=(GameObject)PrefabUtility.InstantiatePrefab(prefab,scene);
            instance.transform.SetParent(flow.transform,false);
            flow.view=instance.GetComponent<GameFlowView>();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            EditorSceneManager.CloseScene(scene,true);
        }
        AssetDatabase.SaveAssets();
        Debug.Log("Static GameFlow UI created and saved in MainMenu and Game.");
    }
    private Sprite ImportArt(string name)
    {
        string path="Assets/Resources/CombatUI/"+name+".png";
        var importer=(TextureImporter)AssetImporter.GetAtPath(path);
        importer.textureType=TextureImporterType.Sprite;
        importer.spriteImportMode=SpriteImportMode.Single;
        importer.mipmapEnabled=false;
        importer.SaveAndReimport();
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }
    private Slider Volume(Transform parent,Vector2 position,string name,out Text percent)
    {
        var rect=Rect(parent,name,new Vector2(.5f,1),position,new Vector2(240,48));
        rect.gameObject.AddComponent<Image>().color=new Color(0,0,0,.01f);
        var track=Rect(rect,"Track",new Vector2(.5f,.5f),Vector2.zero,new Vector2(220,10));
        track.gameObject.AddComponent<Image>().color=new Color(.08f,.1f,.13f);
        var fill=Rect(track,"Fill",new Vector2(.5f,.5f),Vector2.zero,Vector2.zero);
        fill.anchorMin=Vector2.zero;fill.anchorMax=Vector2.one;fill.offsetMin=fill.offsetMax=Vector2.zero;
        fill.gameObject.AddComponent<Image>().color=new Color(.85f,.62f,.3f);
        var area=Rect(rect,"HandleArea",new Vector2(.5f,.5f),Vector2.zero,new Vector2(220,32));
        var handle=Rect(area,"Handle",new Vector2(.5f,.5f),Vector2.zero,new Vector2(24,32));
        var image=handle.gameObject.AddComponent<Image>();image.color=new Color(.98f,.88f,.68f);
        var slider=rect.gameObject.AddComponent<Slider>();slider.fillRect=fill;slider.handleRect=handle;slider.targetGraphic=image;slider.value=1;
        percent=Label(parent,"100%",new Vector2(.5f,1),position+new Vector2(155,0),new Vector2(65,48),20);
        return slider;
    }
        private RectTransform Rect(Transform parent, string name, Vector2 anchor, Vector2 position, Vector2 size)
        {
            var obj = new GameObject(name, typeof(RectTransform));
            obj.transform.SetParent(parent, false);
            var rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = anchor;
            rect.pivot = anchor == new Vector2(0, 1) ? anchor : new Vector2(.5f, .5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            return rect;
        }
        private Sprite LoadArt(string name)
        {
            var texture = Resources.Load<Texture2D>("CombatUI/" + name);
            return texture == null ? null : Sprite.Create(texture, new UnityEngine.Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), 100f);
        }
        private void Decoration(Transform parent, string name, Sprite sprite, Vector2 size, Color color)
        {
            var image = Rect(parent, name, new Vector2(.5f, .5f), Vector2.zero, size).gameObject.AddComponent<Image>();
            image.sprite = sprite;
            image.color = color;
            image.raycastTarget = false;
        }
        private Text Label(Transform parent, string text, Vector2 anchor, Vector2 position, Vector2 size, int fontSize)
        {
            var label = Rect(parent, "Text", anchor, position, size).gameObject.AddComponent<Text>();
            label.font = font;
            label.text = text;
            label.fontSize = fontSize;
            label.color = Color.white;
            label.alignment = TextAnchor.MiddleCenter;
            label.raycastTarget = false;
            return label;
        }
        private Button Button(Transform parent, string text, Vector2 anchor, Vector2 position, Vector2 size, Action action, string objectName = null)
        {
            var rect = Rect(parent, objectName ?? text, anchor, position, size);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = brushButton;
            image.color = new Color(.2f, .24f, .28f, 1);
            var outline = rect.gameObject.AddComponent<Outline>();
            outline.effectColor = new Color(.7f, .51f, .26f, .85f);
            outline.effectDistance = new Vector2(1, -1);
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            var colors = button.colors;
            colors.highlightedColor = new Color(1.25f, 1.15f, .95f);
            colors.pressedColor = new Color(.65f, .6f, .5f);
            colors.disabledColor = new Color(.45f, .45f, .45f, .7f);
            button.colors = colors;
            
            Label(rect, text, new Vector2(.5f, .5f), Vector2.zero, size, 25);
            return button;
        }
        private GameObject Panel(string name, Vector2 size)
        {
            var rect = Rect(root, name, new Vector2(.5f, .5f), Vector2.zero, size);
            var background = rect.gameObject.AddComponent<Image>();
            background.sprite = paper;
            background.color = new Color(.12f, .15f, .2f, 1);
            Decoration(rect, "BrushFrame", brushFrame, size, new Color(.85f, .62f, .3f, 1));
            Decoration(rect, "HeaderBrush", brushButton, new Vector2(size.x - 70, 6), new Color(.85f, .62f, .3f, .6f));
            rect.GetChild(rect.childCount - 1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, size.y * .5f - 83);
            return rect.gameObject;
        }

}
