namespace StickmanCombat.Gameplay
{
    public enum WeaponEffect { None, Dodge, DoubleKill, Knockback }

    public struct WeaponCombatProfile
    {
        public float reach;
        public float recovery;
        public WeaponEffect effect;
        public string label;
        public static bool IsSelectable(Weapon weapon)
        {
            if (weapon.totalIndex == 0) return true;
            int rank = (int)RequiredRank(weapon);
            int firstIndex = 1;
            while (1 + (firstIndex - 1) * 7 / (ItemManager.instance.weaponsById.Count - 1) < rank) firstIndex++;
            return weapon.totalIndex - firstIndex < 3;
        }

        public static RankTier RequiredRank(Weapon weapon)
        {
            if (weapon.totalIndex == 0) return RankTier.Bronze;
            return (RankTier)(1 + (weapon.totalIndex - 1) * 7 / (ItemManager.instance.weaponsById.Count - 1));
        }

        public static WeaponCombatProfile For(Weapon weapon)
        {
            if (weapon == null || weapon.id == "fist0")
                return new WeaponCombatProfile { reach = 0.2f, recovery = 0.2f, effect = WeaponEffect.None, label = "徒手" };
            int rank = (int)RequiredRank(weapon);
            int firstIndex = 1;
            while (1 + (firstIndex - 1) * 7 / (ItemManager.instance.weaponsById.Count - 1) < rank) firstIndex++;
            int variant = (weapon.totalIndex - firstIndex) % 3;
            return new WeaponCombatProfile
            {
                reach = 0.2f + rank * 0.2f,
                recovery = 0.2f,
                effect = (WeaponEffect)(variant + 1),
                label = variant == 0 ? "被处决前自动后撤闪避，冷却 45 秒" : variant == 1 ? "出手范围内有两名敌人时，同时斩杀两名" : "击杀时将同方向的下一名敌人击退 2 米"
            };
        }
    }
}
