using UnityEngine;

namespace StickmanCombat.Gameplay
{
    public static class EquipmentPresentation
    {
        public static Sprite Icon(Weapon weapon)
        {
            return Resources.Load<Sprite>("iconShop/" + weapon.id) ?? weapon.sprite;
        }

        public static string Description(Weapon weapon)
        {
            WeaponCombatProfile profile = WeaponCombatProfile.For(weapon);
            return "攻击距离 +" + (profile.reach - 0.2f).ToString("0.0") + "\n" + profile.label;
        }
    }
}
