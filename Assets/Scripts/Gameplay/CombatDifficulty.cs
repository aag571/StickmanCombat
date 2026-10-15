using UnityEngine;

namespace StickmanCombat.Gameplay
{
    public struct CombatDifficulty
    {
        public float AdvancedChance;
        public float SpawnInterval;
        public float Speed;
        public float Gap;
        public int MaxEnemies;
        public int WeaponTier;
        public int ExtraHits;

        public static CombatDifficulty Evaluate(GameMode mode, RankTier rank, float elapsedSeconds)
        {
            float progress = mode == GameMode.Ranked
                ? Mathf.Clamp01((int)rank / 7f + Mathf.Max(0f, elapsedSeconds) / 240f)
                : Mathf.Clamp01(Mathf.Max(0f, elapsedSeconds) / 180f);
            return new CombatDifficulty
            {
                AdvancedChance = Mathf.Lerp(0.08f, 1f, progress),
                SpawnInterval = Mathf.Lerp(2.2f, 0.55f, progress),
                Speed = Mathf.Lerp(2.6f, 6f, progress),
                Gap = Mathf.Lerp(5.5f, 2.2f, progress),
                MaxEnemies = Mathf.RoundToInt(Mathf.Lerp(4f, 16f, progress)),
                WeaponTier = Mathf.Min(6, Mathf.FloorToInt(progress * 7f)),
                ExtraHits = progress >= 0.8f ? 1 : 0
            };
        }
    }
}
