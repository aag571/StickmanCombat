using System.Linq;
using UnityEngine;

namespace StickmanCombat.Gameplay
{
    public sealed class EquipmentService
    {
        private readonly ItemManager items;
        private readonly PlayerProfileRepository repository;
        public EquipmentService(ItemManager items, PlayerProfileRepository repository)
        {
            this.items = items;
            this.repository = repository;
        }

        public RankTier RequiredRank(Weapon weapon)
        {
            return WeaponCombatProfile.RequiredRank(weapon);
        }

        public void UnlockForRank()
        {
            foreach (Weapon weapon in items.weaponsById.Values.OrderBy(weapon => weapon.totalIndex))
            {
                if (!WeaponCombatProfile.IsSelectable(weapon)) continue;
                if (!weapon.isOwned && RequiredRank(weapon) <= repository.Profile.rank.highestTier)
                {
                    weapon.Own();
                    repository.QueueUnlock(weapon.GetName());
                }
            }
            repository.Save();
        }

        public bool Equip(string id)
        {
            if (!items.weaponsById.TryGetValue(id, out Weapon weapon) || !weapon.IsOwned() || !WeaponCombatProfile.IsSelectable(weapon)) return false;
            items.currentWeapon = weapon;
            items.EquipCurrentWeapon(true);
            PlayerPrefs.Save();
            return true;
        }
    }
}
