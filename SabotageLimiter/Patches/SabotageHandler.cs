using HarmonyLib;
using Reactor.Utilities;

namespace SabotageLimiter
{
    [HarmonyPatch]

    public class SabHandler
    {
        [HarmonyPatch(typeof(SabotageSystemType), nameof(SabotageSystemType.UpdateSystem))]
        [HarmonyPrefix]

        public static bool Prefix(ref PlayerControl player, out PlayerControl __state)
        {
            __state = null;
            if (!AmongUsClient.Instance.AmHost) return true;
            if (SetImpostors.SabCountsRemaining[player.PlayerId] == 0) return false;
            if (player.Data.IsDead && !SabotageLimiterPlugin.DeadCanSab.Value) return false;
            __state = player;
            return true;
        }

        [HarmonyPatch(typeof(SabotageSystemType), nameof(SabotageSystemType.UpdateSystem))]
        [HarmonyPostfix]

        public static void Postfix(PlayerControl __state)
        {
            if (__state)
            {
                var current = SetImpostors.SabCountsRemaining[__state.PlayerId];
                current--;
                SetImpostors.SabCountsRemaining[__state.PlayerId] = current;
            }
        }
    }
}