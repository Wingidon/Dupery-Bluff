using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;
using Il2CppTMPro;
using HarmonyLib;

public class wx_KeywordPatch // Code by Skill Cycler, absolute legend!
{

    public string PatchTooltip(string value)
    {
        if (value != null)
        {
            if (value.Contains("Clock Tower"))
            {
                value = value.Replace(
                    "Clock Tower",
                    "<link=\"ClockTower\"><color=#F6D88D>Clock Tower</color></link>"
                );
            }
            if (value.Contains("Underlings"))
            {
                value = value.Replace(
                    "Underlings",
                    "key1"
                );
            }
            if (value.Contains("Underling"))
            {
                value = value.Replace(
                    "Underling",
                    "<link=\"Underling\"><color=#BA4848>Underling</color></link>"
                );
            }
            if (value.Contains("key1"))
            {
                value = value.Replace(
                    "key1",
                    "<link=\"Underling\"><color=#BA4848>Underlings</color></link>"
                );
            }
            if (value.Contains("Traitors"))
            {
                value = value.Replace(
                    "Traitors",
                    "key1"
                );
            }
            if (value.Contains("Traitor"))
            {
                value = value.Replace(
                    "Traitor",
                    "<link=\"Traitor\"><color=#9B4BD0>Traitor</color></link>"
                );
            }
            if (value.Contains("key1"))
            {
                value = value.Replace(
                    "key1",
                    "<link=\"Traitor\"><color=#9B4BD0>Traitors</color></link>"
                );
            }
            if (value.Contains("run away with her lover"))
            {
                value = value.Replace(
                    "run away with her lover",
                    "<link=\"CasanovaAbility\"><color=#BA4848>run away with her lover</color></link>"
                );
            }
            if (value.Contains("lose faith in you"))
            {
                value = value.Replace(
                    "lose faith in you",
                    "<link=\"SkepticSelfConfirm\"><color=#8BC6E4>lose faith in you</color></link>"
                );
            }
            if (value.Contains("Locked"))
            {
                value = value.Replace(
                    "Locked",
                    "<link=\"Lockout\"><color=#BA4848>Locked</color></link>"
                );
            }
        }
        return value;
    }
    [HarmonyPatch(typeof(TextTooltipRecognizer), "GetTooltipInfo")]
    public static class TooltipPatch
    {
        static void Postfix(string linkID, ref TooltipInfo __result)
        {
            wx_KeywordPatch patcher = new();
            if (linkID == "ClockTower")
            {
                __result = new TooltipInfo(
                    patcher.PatchTooltip("The Clock Tower going off is indicated by Night falling out of nowhere.\n\nOnce per game, the Clock Tower will usually go off naturally after a certain number of Reveals, but certain characters may cause it to go off at unexpected times through their abilities."),
                    "Clock Tower",
                    new Color32(246, 216, 141, 255)
                );
            }
            if (linkID == "Underling")
            {
                __result = new TooltipInfo(
                    patcher.PatchTooltip("Another word uses to refer to the Minion character type.\n\nUnderlings are the backbone of the Evil team. There will usually be 1-4 Underlings per village. Their job is deceive you and support the Traitors in taking down the village.\n\nUnderlings will usually Lie and Disguise as a Villager or Outcast."),
                    "Underling",
                    new Color32(186, 72, 72, 255)
                );
            }
            if (linkID == "Traitor")
            {
                __result = new TooltipInfo(
                    patcher.PatchTooltip("Another word used to refer to the Demon character type.\nTraitors are essentially Demons, but with slightly different Disguise rules.\n\nTraitors will usually Lie and Disguise as a not-in-play Villager or Outcast.\n\nTraitors are powerful Evil roles, generally much more powerful than their Underling counterparts. There will usually only be one per village, if any at all."),
                    "Traitor",
                    new Color32(155, 75, 208, 255)
                );
            }
            if (linkID == "CasanovaAbility")
            {
                __result = new TooltipInfo(
                    patcher.PatchTooltip("The Casanova has the ability to charm the Romantic's lover and run away with them. The Romantic, dumbfounded, will be unable to say anything other than \"My lover ran away with somebody else?!\" as a result.\n\nLying Romantics are unaffected."),
                    "Charm",
                    new Color32(186, 72, 72, 255)
                );
            }
            if (linkID == "SkepticSelfConfirm")
            {
                __result = new TooltipInfo(
                    patcher.PatchTooltip("If your Health falls too low, Truthful Skeptics will say \"Sorry, you can't be trusted\" instead of their usual info.\n\nLying Skeptics will never lose faith in you."),
                    "Faith",
                    new Color32(139, 198, 228, 255)
                );
            }
            if (linkID == "Lockout")
            {
                __result = new TooltipInfo(
                    patcher.PatchTooltip("This character has been Locked and cannot be Revealed until the culprit's ability has been deactivated.\n\nThey can still be Executed, and doing so will Reveal them in spite of the Lockout."),
                    "Lockout",
                    new Color32(186, 72, 72, 255)
                );
            }
        }
    }
}
