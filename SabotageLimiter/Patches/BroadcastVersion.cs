using HarmonyLib;

namespace SabotageLimiter
{
    [HarmonyPatch]
    
    public class Protocol
    {
        [HarmonyPatch(typeof(Constants), nameof(Constants.GetBroadcastVersion))]
        [HarmonyPostfix]

        public static void Postfix(ref int __result)
        {
            if (__result % 50 >= 25) return;
            __result += 25;
        }
    }
}