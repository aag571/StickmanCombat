using System.Collections;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace StickmanCombat.Gameplay
{
    [DefaultExecutionOrder(-1000)]
    public sealed class GameFlowController : MonoBehaviour
    {
        public static GameFlowController instance;
        public static GameMode SelectedMode = GameMode.Ranked;
        public GameFlowView view;
        public PlayerProfileRepository Repository { get; private set; }
        public EquipmentService Equipment { get; private set; }
        public GameSession Session { get; private set; }
        public bool IsMainMenu => UnitySceneManager.GetActiveScene().name == "MainMenu";
        public bool AllowsContinuousSpawning => Session != null && Session.Rules.AllowsContinuousSpawning && Session.State == SessionState.Playing;
        private bool loadingScene;
        private bool tutorialSpawning;
        public bool TutorialAwaitingInput { get; private set; }
        private int tutorialSide;
        private float sessionStartTime;
        private float nextDodgeTime;
        public float DodgeCooldownRemaining => Mathf.Max(0f, nextDodgeTime - Time.time);
        private Npc secondaryTarget;
        public CombatDifficulty CurrentDifficulty => CombatDifficulty.Evaluate(SelectedMode, Repository.Profile.rank.tier, Time.time - sessionStartTime);

        private void Awake() { instance = this; Repository = new PlayerProfileRepository(); Time.timeScale = 1f; }
        private IEnumerator Start()
        {
            yield return null;
            Equipment = new EquipmentService(ItemManager.instance, Repository);
            foreach (MenuToggle toggle in FindObjectsOfType<MenuToggle>(true))
                toggle.isOn = PlayerPrefsX.GetBool(toggle.playerPrefString, toggle.startingState);
            Equipment.UnlockForRank();
            var combat = global::SceneManager.instance;
            combat.isEndless = true;
            combat.firstStart = false;
            combat.isBeltExamReady = false;
            combat.gameStarted = false;
            combat.inputStarted = false;
            combat.currentState = global::SceneManager.State.waiting;
            combat.trainMover.enabled = false;
            combat.chaserObject.transform.SetParent(combat.playerObject.transform);
            combat.chaserObject.transform.localPosition = Vector3.zero;
            MenuManager.instance.blur.enabled = false;
            TutorialManager.instance.tutorialIndex = 5;
            view.Initialize(this);
            if (IsMainMenu)
            {
                NpcManager.instance.paused = true;
                view.ShowMainMenu();
                string unlocked = Repository.ConsumeUnlocks();
                if (!string.IsNullOrEmpty(unlocked)) view.ShowUnlocks(unlocked);
            }
            else BeginSession();
        }

        private void BeginSession()
        {
            nextDodgeTime = 0f;
            secondaryTarget = null;
            sessionStartTime = Time.time;
            Session = new GameSession(SelectedMode, Repository.Profile.rank.tier);
            Session.Completed += OnCompleted;
            Session.Changed += RefreshHud;
            var combat = global::SceneManager.instance;
            combat.currentState = global::SceneManager.State.gameStarted;
            combat.gameStarted = true;
            combat.inputStarted = true;
            CharacterManager.instance.Activate();
            NpcManager.instance.OnGameStart();
            NpcManager.instance.paused = false;
            EndlessManager.instance.OnStarted();
            MonetizationManager.instance.OnGameStart();
            AudioManager.instance.OnGameStart();
            view.ShowGame();
            RefreshHud();
        }

        private void Update()
        {
            if (Session == null || Session.State != SessionState.Playing) return;
            if (Input.GetKeyDown(KeyCode.Escape)) OpenSettings();
            if (Session.Mode == GameMode.Tutorial && !TutorialAwaitingInput && CharacterManager.instance.currentState == CharacterManager.State.Idle && CharacterManager.instance.inputQueue.Count == 0)
            {
                Npc target = NpcManager.instance.leftNpcs.Count > 0 ? NpcManager.instance.leftNpcs.First.Value : NpcManager.instance.rightNpcs.Count > 0 ? NpcManager.instance.rightNpcs.First.Value : null;
                if (target != null && target.markedForKill && target.hitSides.Count > 0)
                {
                    tutorialSide = target.hitSides[0];
                    TutorialAwaitingInput = true;
                    Time.timeScale = 0f;
                }
            }
            if (Session.Mode == GameMode.Tutorial && !tutorialSpawning && NpcManager.instance.leftNpcs.Count + NpcManager.instance.rightNpcs.Count == 0) StartCoroutine(SpawnTutorialEnemy());
        }
        private IEnumerator SpawnTutorialEnemy()
        {
            tutorialSpawning = true;
            yield return new WaitForSeconds(0.8f);
            if (Session.State == SessionState.Playing)
            {
                int step = Session.Kills;
                NpcManager.instance.SpawnEnemy(step == 0 || step == 3 ? 1 : -1, 9f, 2f, step < 2 ? 0 : step);
            }
            tutorialSpawning = false;
        }
        public void EnemyDefeated(int points) { Session?.EnemyDefeated(points); }
        public bool TryDodge(Npc attacker)
        {
            if (Session == null || Session.State != SessionState.Playing || Session.Mode == GameMode.Tutorial
                || Time.time < nextDodgeTime || WeaponCombatProfile.For(ItemManager.instance.currentWeapon).effect != WeaponEffect.Dodge) return false;
            nextDodgeTime = Time.time + 45f;
            CharacterManager.instance.BeginAutoDodge(attacker.side);
            return true;
        }
        public void PrepareWeaponHit(Npc primary, int side, float threatPosition)
        {
            secondaryTarget = null;
            if (primary == null || Session.Mode == GameMode.Tutorial
                || WeaponCombatProfile.For(ItemManager.instance.currentWeapon).effect != WeaponEffect.DoubleKill) return;
            var enemies = side == -1 ? NpcManager.instance.leftNpcs : NpcManager.instance.rightNpcs;
            var next = enemies.First?.Next;
            if (next != null && (next.Value.transform.position.x - threatPosition) * side <= 0f)
                secondaryTarget = next.Value;
        }
        public void ResolveWeaponHit(Npc primary, int side, Vector2 impact, bool defeated)
        {
            if (Session == null || Session.State != SessionState.Playing || Session.Mode == GameMode.Tutorial) return;
            WeaponEffect effect = WeaponCombatProfile.For(ItemManager.instance.currentWeapon).effect;
            var enemies = side == -1 ? NpcManager.instance.leftNpcs : NpcManager.instance.rightNpcs;
            if (effect == WeaponEffect.DoubleKill && secondaryTarget != null)
            {
                Npc secondary = secondaryTarget;
                secondaryTarget = null;
                if (!defeated)
                {
                    primary.hitSides.Clear();
                    primary.GotHit(impact);
                }
                if (Session.State == SessionState.Playing && enemies.First != null && enemies.First.Value == secondary)
                {
                    secondary.hitSides.Clear();
                    secondary.GotHit(impact);
                }
            }
            else if (effect == WeaponEffect.Knockback && defeated) PushEnemies(side, 2f);
        }
        private void PushEnemies(int side, float distance)
        {
            var enemies = side == -1 ? NpcManager.instance.leftNpcs : NpcManager.instance.rightNpcs;
            foreach (Npc enemy in enemies)
            {
                enemy.transform.position += Vector3.right * side * distance;
                enemy.UnMarkForKill();
            }
        }
        public void TutorialAttack(int side)
        {
            if (!TutorialAwaitingInput || side != tutorialSide || Session.State != SessionState.Playing) return;
            TutorialAwaitingInput = false;
            Time.timeScale = 1f;
            CharacterManager.instance.inputQueue.Enqueue(side);
        }
        public void PlayerDefeated() { Session?.Finish(false); }
        public void QueueUnlock(string name) { Repository.QueueUnlock(name); Repository.Save(); }
        private void RefreshHud() { view.UpdateHud(Session, Repository.Profile.rank.DisplayName); }
        private void OnCompleted(bool won)
        {
            global::SceneManager.instance.gameStarted = false;
            global::SceneManager.instance.inputStarted = false;
            global::SceneManager.instance.currentState = global::SceneManager.State.gameOver;
            NpcManager.instance.paused = true;
            CharacterManager.instance.inputQueue.Clear();
            MonetizationManager.instance.HideCoin();
            MonetizationManager.instance.SaveCoins();
            if (Session.Rules.AffectsRank) { Repository.Profile.rank.ApplyResult(won); Equipment.UnlockForRank(); }
            else if (Session.Mode == GameMode.Endless) Repository.Profile.endlessBest = Mathf.Max(Repository.Profile.endlessBest, Session.Score);
            else if (won) Repository.Profile.tutorialCompleted = true;
            Repository.Save();
            Time.timeScale = 0f;
            view.ShowResult(Session, Repository.Profile.rank.DisplayName);
        }
        public void EnterMode(GameMode mode) { if (!IsMainMenu) return; SelectedMode = mode; LoadScene("Game"); }
        public void Replay() { LoadScene("Game"); }
        public void ReturnToMenu() { if (Session != null && Session.State != SessionState.Completed) Session.Finish(false); LoadScene("MainMenu"); }
        private void LoadScene(string scene)
        {
            if (loadingScene) return;
            loadingScene = true;
            MonetizationManager.instance.SaveCoins();
            Repository.Save();
            Time.timeScale = 1f;
            UnitySceneManager.LoadSceneAsync(scene);
        }
        public void OpenEquipment() { if (IsMainMenu) view.ShowEquipment(Equipment); }
        public void OpenSettings()
        {
            Session?.SetPaused(true);
            MenuManager.instance.isPaused = !IsMainMenu;
            if (!IsMainMenu) Time.timeScale = 0f;
            view.ShowSettings();
        }
        public void CloseSettings()
        {
            PlayerPrefs.Save();
            view.HideSettings();
            if (Session != null && Session.State != SessionState.Completed) { Session.SetPaused(false); MenuManager.instance.isPaused = false; Time.timeScale = TutorialAwaitingInput ? 0f : 1f; }
        }
        private void OnDestroy()
        {
            if (Session != null) { Session.Completed -= OnCompleted; Session.Changed -= RefreshHud; }
            if (instance == this) instance = null;
            Time.timeScale = 1f;
        }
    }
}
