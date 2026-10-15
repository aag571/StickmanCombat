using System;

namespace StickmanCombat.Gameplay
{
    public enum GameMode { Tutorial, Ranked, Endless }
    public enum SessionState { Playing, Paused, Completed }

    public interface IGameModeRules
    {
        int TargetKills { get; }
        bool AllowsContinuousSpawning { get; }
        bool AffectsRank { get; }
        string GetObjective(int kills, int score);
        bool IsComplete(int kills);
    }

    public sealed class RankedRules : IGameModeRules
    {
        public int TargetKills { get; }
        public bool AllowsContinuousSpawning => true;
        public bool AffectsRank => true;
        public RankedRules(RankTier tier) { TargetKills = 15 + (int)tier * 10; }
        public string GetObjective(int kills, int score) => "击败敌人 " + kills + " / " + TargetKills;
        public bool IsComplete(int kills) => kills >= TargetKills;
    }

    public sealed class EndlessRules : IGameModeRules
    {
        public int TargetKills => 0;
        public bool AllowsContinuousSpawning => true;
        public bool AffectsRank => false;
        public string GetObjective(int kills, int score) => "积分 " + score;
        public bool IsComplete(int kills) => false;
    }

    public sealed class TutorialRules : IGameModeRules
    {
        private static readonly string[] Steps = {
            "第一课：白点亮起后，按 D / → 向右移动攻击。",
            "第二课：按 A / ← 向左移动攻击。",
            "第三课：观察方向提示，连续按两次 A / ←。",
            "第四课：敌人会换边，先按 D / →，再按 A / ←。"
        };
        private static readonly string[] TouchSteps = {
            "第一课：白点亮起后，点击右下角按钮向右移动攻击。",
            "第二课：点击左下角按钮向左移动攻击。",
            "第三课：观察方向提示，连续点击两次左下角按钮。",
            "第四课：敌人会换边，先点右下角，再点左下角按钮。"
        };
        public int TargetKills => Steps.Length;
        public bool AllowsContinuousSpawning => false;
        public bool AffectsRank => false;
        public string GetObjective(int kills, int score) => (CombatInputPlatform.UsesTouchControls ? TouchSteps : Steps)[Math.Min(kills, Steps.Length - 1)];
        public bool IsComplete(int kills) => kills >= TargetKills;
    }

    public sealed class GameSession
    {
        public GameMode Mode { get; }
        public IGameModeRules Rules { get; }
        public SessionState State { get; private set; } = SessionState.Playing;
        public int Kills { get; private set; }
        public int Score { get; private set; }
        public bool Won { get; private set; }
        public event Action Changed;
        public event Action<bool> Completed;

        public GameSession(GameMode mode, RankTier tier)
        {
            Mode = mode;
            switch (mode)
            {
                case GameMode.Ranked: Rules = new RankedRules(tier); break;
                case GameMode.Tutorial: Rules = new TutorialRules(); break;
                default: Rules = new EndlessRules(); break;
            }
        }

        public void EnemyDefeated(int points)
        {
            if (State != SessionState.Playing) return;
            Kills++;
            Score += points;
            Changed?.Invoke();
            if (Rules.IsComplete(Kills)) Finish(true);
        }

        public void SetPaused(bool paused)
        {
            if (State == SessionState.Completed) return;
            State = paused ? SessionState.Paused : SessionState.Playing;
            Changed?.Invoke();
        }

        public void Finish(bool won)
        {
            if (State == SessionState.Completed) return;
            Won = won;
            State = SessionState.Completed;
            Completed?.Invoke(won);
            Changed?.Invoke();
        }
    }
}
