using HarmonyLib;

namespace DuckGame.HaloWeapons
{
    [HarmonyPatch(typeof(Duck), nameof(Duck.GiveHoldable))]
    internal static class DuckGiveHoldablePatched
    {
        private static void Postfix(Holdable h)
        {
            if (h is HaloWeapon weapon)
                weapon.OnPickUp();
        }
    }
}
