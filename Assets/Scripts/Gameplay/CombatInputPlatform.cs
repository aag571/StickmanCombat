using UnityEngine;

namespace StickmanCombat.Gameplay
{
    public static class CombatInputPlatform
    {
        public static bool UsesTouchControls => Application.isMobilePlatform;
    }
}
