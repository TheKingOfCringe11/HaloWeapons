using HarmonyLib;

namespace DuckGame.HaloWeapons
{
    [HarmonyPatch(typeof(Duck), nameof(Duck.Kill))]
    internal static class DuckKillPatched
    {
        [HarmonyPostfix]
        private static void Kill(Duck __instance)
        {
            if (Level.current is TeamSelect2)
                return;

            Profile localProfile = Core.LocalProfile;

            if (__instance.killedByProfile == localProfile && __instance.profile != localProfile)
                Skins.AddCredits(10);
        }
    }
}
