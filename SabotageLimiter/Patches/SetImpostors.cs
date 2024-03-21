using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace SabotageLimiter
{
    [HarmonyPatch]
    
    public class SetImpostors
    {
        public static Dictionary<byte, int> SabCountsRemaining { get; set; } = new();

        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Begin))]
        [HarmonyPostfix]

        public static void Postfix()
        {
            SabCountsRemaining.Clear();
            PlayerControl.AllPlayerControls.ToArray()
                                           .Where(x => x.Data.Role.TeamType == RoleTeamTypes.Impostor)
                                           .Do(x => SabCountsRemaining.Add(x.PlayerId, SabotageLimiterPlugin.MaxSabCount.Value));
        }
    }
}