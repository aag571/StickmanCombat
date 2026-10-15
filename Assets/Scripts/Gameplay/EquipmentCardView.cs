using System;
using UnityEngine;
using UnityEngine.UI;

namespace StickmanCombat.Gameplay
{
    public sealed class EquipmentCardView : MonoBehaviour
    {
        public Button button;
        public Image icon;
        public Text title;
        public Text description;
        public Text status;
        public void Bind(Weapon weapon, EquipmentService service, Action refresh)
        {
            bool owned = weapon.IsOwned();
            icon.sprite = EquipmentPresentation.Icon(weapon);
            title.text = weapon.GetName();
            description.text = EquipmentPresentation.Description(weapon);
            status.text = owned ? weapon == ItemManager.instance.currentWeapon ? "已装备" : "点击装备" : RankProgress.Names[(int)service.RequiredRank(weapon)] + "解锁";
            button.interactable = owned;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { if (service.Equip(weapon.id)) refresh(); });
        }
    }
}
