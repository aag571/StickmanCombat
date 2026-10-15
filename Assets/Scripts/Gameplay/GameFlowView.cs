using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanCombat.Gameplay
{
    public sealed class GameFlowView : MonoBehaviour
    {
        public Font font;
        public RectTransform safeArea;
        public CanvasScaler scaler;
        public GameObject main;
        public GameObject settings;
        public GameObject equipment;
        public GameObject result;
        public GameObject unlocks;
        public GameObject mobileControls;
        public Text hud;
        public Text equipped;
        public Text rankLabel;
        public Text resultTitle;
        public Text resultDetail;
        public Text unlockDetail;
        public Button settingsButton;
        public Button tutorialButton;
        public Button rankedButton;
        public Button endlessButton;
        public Button equipmentButton;
        public Button equipmentClose;
        public Button settingsClose;
        public Button settingsExit;
        public Button replayButton;
        public Button menuButton;
        public Button unlockClose;
        public Button[] toggleButtons;
        public Slider musicVolume;
        public Slider soundVolume;
        public Text musicPercent;
        public Text soundPercent;
        public EquipmentCardView[] cards;
        private GameFlowController controller;

        public void Initialize(GameFlowController flow)
        {
            controller = flow;
            if (safeArea == null) throw new InvalidOperationException("GameFlow UI 尚未保存。请运行 Tools/Stickman Combat/Create Static UI。");
            settingsButton.onClick.AddListener(flow.OpenSettings);
            tutorialButton.onClick.AddListener(() => flow.EnterMode(GameMode.Tutorial));
            rankedButton.onClick.AddListener(() => flow.EnterMode(GameMode.Ranked));
            endlessButton.onClick.AddListener(() => flow.EnterMode(GameMode.Endless));
            equipmentButton.onClick.AddListener(flow.OpenEquipment);
            equipmentClose.onClick.AddListener(() => { equipment.SetActive(false); RefreshEquipped(); });
            settingsClose.onClick.AddListener(flow.CloseSettings);
            settingsExit.onClick.AddListener(flow.ReturnToMenu);
            replayButton.onClick.AddListener(flow.Replay);
            menuButton.onClick.AddListener(flow.ReturnToMenu);
            unlockClose.onClick.AddListener(() => unlocks.SetActive(false));
            for (int index = 0; index < toggleButtons.Length; index++)
            {
                int toggleIndex = index;
                toggleButtons[index].onClick.AddListener(() => { GetToggles()[toggleIndex].OnToggle(); PlayerPrefs.Save(); RefreshSettings(); });
            }
            musicVolume.onValueChanged.AddListener(value => { AudioManager.instance.SetMusicVolume(value); musicPercent.text = Mathf.RoundToInt(value * 100) + "%"; });
            soundVolume.onValueChanged.AddListener(value => { AudioManager.instance.SetSoundVolume(value); soundPercent.text = Mathf.RoundToInt(value * 100) + "%"; });
            settingsButton.GetComponent<RectTransform>().anchorMin = settingsButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, flow.IsMainMenu ? 0 : 1);
            settingsButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-90, flow.IsMainMenu ? 48 : -42);
            main.SetActive(false);
            settings.SetActive(false);
            equipment.SetActive(false);
            result.SetActive(false);
            unlocks.SetActive(false);
            mobileControls.SetActive(false);
        }
        private void Update()
        {
            if (controller == null || safeArea == null || Screen.width <= 0 || Screen.height <= 0) return;
            var safe = Screen.safeArea;
            safeArea.anchorMin = new Vector2(safe.xMin / Screen.width, safe.yMin / Screen.height);
            safeArea.anchorMax = new Vector2(safe.xMax / Screen.width, safe.yMax / Screen.height);
            safeArea.offsetMin = safeArea.offsetMax = Vector2.zero;
            scaler.matchWidthOrHeight = Screen.width < Screen.height ? 0f : 1f;
            mobileControls.SetActive(!controller.IsMainMenu && CombatInputPlatform.UsesTouchControls
                && controller.Session != null && controller.Session.State == SessionState.Playing && !settings.activeSelf);
        }
        private MenuToggle[] GetToggles() => new[] { AudioManager.instance.musicToggle, AudioManager.instance.soundToggle, NpcManager.instance.bloodToggler, FindObjectOfType<EnvironmentManager>().weatherToggler };
        private void RefreshEquipped() { equipped.text = "当前武器：" + ItemManager.instance.currentWeapon.GetName(); }
        public void ShowMainMenu()
        {
            main.SetActive(true);
            rankLabel.text = controller.Repository.Profile.rank.DisplayName;
            hud.text = "最高积分 " + controller.Repository.Profile.endlessBest;
            RefreshEquipped();
        }
        public void ShowGame() { main.SetActive(false); equipped.gameObject.SetActive(false); }
        public void UpdateHud(GameSession session, string rank)
        {
            hud.text = session.Mode == GameMode.Ranked ? rank + "\n" + session.Rules.GetObjective(session.Kills, session.Score) : session.Rules.GetObjective(session.Kills, session.Score);
        }
        public void ShowResult(GameSession session, string rank)
        {
            result.SetActive(true);
            result.transform.SetAsLastSibling();
            resultTitle.text = session.Mode == GameMode.Endless ? "本局积分 " + session.Score : session.Won ? "挑战成功" : "挑战失败";
            resultDetail.text = session.Mode == GameMode.Ranked ? rank : session.Mode == GameMode.Tutorial ? "教程可在主菜单随时重玩" : "最高积分 " + controller.Repository.Profile.endlessBest;
        }
        private void RefreshSettings()
        {
            string[] names = { "音乐", "音效", "血液效果", "天气" };
            var toggles = GetToggles();
            for (int index = 0; index < toggleButtons.Length; index++)
                toggleButtons[index].GetComponentInChildren<Text>().text = names[index] + "：" + (toggles[index].isOn ? "开" : "关");
            musicVolume.SetValueWithoutNotify(AudioManager.instance.MusicVolume);
            soundVolume.SetValueWithoutNotify(AudioManager.instance.SoundVolume);
            musicPercent.text = Mathf.RoundToInt(musicVolume.value * 100) + "%";
            soundPercent.text = Mathf.RoundToInt(soundVolume.value * 100) + "%";
        }
        public void ShowSettings()
        {
            RefreshSettings();
            settingsExit.gameObject.SetActive(!controller.IsMainMenu);
            settings.SetActive(true);
            settings.transform.SetAsLastSibling();
        }
        public void HideSettings() { PlayerPrefs.Save(); settings.SetActive(false); }
        public void ShowEquipment(EquipmentService service)
        {
            var weapons = ItemManager.instance.weaponsById.Values.Where(WeaponCombatProfile.IsSelectable).OrderBy(weapon => weapon.totalIndex).ToArray();
            for (int index = 0; index < cards.Length; index++)
            {
                cards[index].gameObject.SetActive(index < weapons.Length);
                if (index < weapons.Length) cards[index].Bind(weapons[index], service, () => { ShowEquipment(service); RefreshEquipped(); });
            }
            equipment.SetActive(true);
            equipment.transform.SetAsLastSibling();
        }
        public void ShowUnlocks(string names)
        {
            unlockDetail.text = names + "\n前往武器装备选择使用";
            unlocks.SetActive(true);
            unlocks.transform.SetAsLastSibling();
        }
    }
}
