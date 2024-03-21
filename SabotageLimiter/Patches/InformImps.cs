using System.Linq;
using HarmonyLib;
using Hazel;

namespace SabotageLimiter
{
    [HarmonyPatch]
    
    public class InformImpostors
    {
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
        [HarmonyPostfix]

        public static void Postfix()
        {
            if (AmongUsClient.Instance.AmHost)
            {
                foreach (var id in SetImpostors.SabCountsRemaining.Keys)
                {
                    var imp = PlayerControl.AllPlayerControls.ToArray()
                                                             .FirstOrDefault(x => x.PlayerId == id && !x.Data.Disconnected);
                    
                    if (imp != null)
                    {
                        var text =  $"Attention, this lobby utilises a mod."
                                  + $"You have {SetImpostors.SabCountsRemaining[id]} non-door sabotage uses remaining."
                                  + $"You {CanSab()}."
                                  + "Do not talk about this message in chat.";
                        
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 13, SendOption.Reliable, imp.OwnerId);
                        writer.Write(text);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                    }
                }
            }
        }

        public static string CanSab()
        {
            if (SabotageLimiterPlugin.DeadCanSab.Value) return "can sabotage anything when dead.";
            return "can not sabotage anything except doors when dead.";
        }
    }
}