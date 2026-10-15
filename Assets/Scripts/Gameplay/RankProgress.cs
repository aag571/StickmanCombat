using System;

namespace StickmanCombat.Gameplay
{
    public enum RankTier { Bronze, Silver, Gold, Platinum, Diamond, Master, King, God }

    [Serializable]
    public sealed class RankProgress
    {
        public RankTier tier;
        public int stars = 1;
        public RankTier highestTier;

        public string DisplayName => Names[(int)tier] + " " + stars + "星";
        public static readonly string[] Names = { "青铜", "白银", "黄金", "铂金", "钻石", "大师", "王者", "神" };

        public void Normalize()
        {
            tier = (RankTier)Math.Max(0, Math.Min(7, (int)tier));
            stars = Math.Max(0, Math.Min(5, stars));
            highestTier = (RankTier)Math.Max((int)tier, Math.Min(7, Math.Max(0, (int)highestTier)));
        }

        public void ApplyResult(bool won)
        {
            Normalize();
            if (won)
            {
                if (stars == 5 && tier < RankTier.God)
                {
                    tier++;
                    stars = 1;
                }
                else stars = Math.Min(5, stars + 1);
            }
            else if (stars == 0 && tier > RankTier.Bronze)
            {
                tier--;
                stars = 4;
            }
            else stars = Math.Max(0, stars - 1);
            highestTier = (RankTier)Math.Max((int)highestTier, (int)tier);
        }
    }
}
