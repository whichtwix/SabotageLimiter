using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using Reactor.Networking.Attributes;

namespace SabotageLimiter;

[BepInAutoPlugin("com.sabotagelimiter.whichtwix", "SabotageLimiter", "1.0.0")]
[ReactorModFlags(Reactor.Networking.ModFlags.RequireOnHost)]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]

public partial class SabotageLimiterPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public static ConfigEntry<int> MaxSabCount { get; set; }

    public static ConfigEntry<bool> DeadCanSab { get; set; }

    public override void Load()
    {
        MaxSabCount = Config.Bind("Settings", "Max critical sabotage uses", int.MaxValue);
        DeadCanSab = Config.Bind("Settings", "Dead impostor can do critical sabs", true);
        Harmony.PatchAll();
    }
}
