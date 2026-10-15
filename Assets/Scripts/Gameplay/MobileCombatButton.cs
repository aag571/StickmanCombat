using UnityEngine;
using UnityEngine.EventSystems;

namespace StickmanCombat.Gameplay
{
    public sealed class MobileCombatButton : MonoBehaviour, IPointerDownHandler
    {
        public int side;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (InputManager.instance != null) InputManager.instance.PressDirection(side);
        }
    }
}
