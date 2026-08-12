using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;
using HarmonyLib;

namespace DuperyBluff;

[RegisterTypeInIl2Cpp]
public class w_Dupe_Landlord : w_DupeZ_RoleBase
{
    Character lockoutTarget = null;
    bool haveDeathActed = false;
    public override string Description
    {
        get
        {
            return "Witch sidegrade";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Landlord initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.AfterRoundStart && !charRef.statuses.Contains(ECharacterStatus.BrokenAbility))
        {
            wx_SavedScripts sharedScripts = new();
            int lockRange = System.Int32.Parse(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Landlord_Range").GetValueAsString());
            sharedScripts.DebugMessage($"Landlord at #{charRef.id} acting AfterRoundStart");
            Il2CppSystem.Collections.Generic.List<Character> possibleTargets = new();
            possibleTargets = sharedScripts.GetCharactersWithinRange(charRef, lockRange);
            possibleTargets = Characters.Instance.FilterCharacterMissingStatus(possibleTargets, LockedOut.lockedOut);
            if (possibleTargets.Count == 0) sharedScripts.DebugMessage($"Landlord at #{charRef.id} found no possible targets!");
            else
            {
                lockoutTarget = possibleTargets[UnityEngine.Random.RandomRangeInt(0, possibleTargets.Count)];
                lockoutTarget.statuses.AddStatus(LockedOut.lockedOut, charRef);
                sharedScripts.DebugMessage($"Landlord at #{charRef.id} locking #{lockoutTarget.id}");
            }
        }
        if (trigger == ETriggerPhase.OnDied || trigger == ETriggerPhase.OnExecuted)
        {
            new wx_SavedScripts().DebugMessage($"Landlord at #{charRef.id} acting {trigger.ToString()}");
            ActOnDied(charRef);
        }
    }
    public override void ActOnDied(Character charRef)
    {
        if (haveDeathActed) return;
        haveDeathActed = true;
        new wx_SavedScripts().DebugMessage($"Landlord at #{charRef.id} acting OnDied");
        if (lockoutTarget)
        {
            lockoutTarget.statuses.statuses.Remove(LockedOut.lockedOut);
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        return GrabDisguise(charRef, false);
    }
    public w_Dupe_Landlord() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Landlord>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Landlord(System.IntPtr ptr) : base(ptr)
    {
    }


    public static class LockedOut
    {
        public static ECharacterStatus lockedOut = (ECharacterStatus)1215311152;
        [HarmonyPatch(typeof(Character), nameof(Character.ShowDescription))]
        public static class LockedOutHint // Code taken from Power Play by That Town Of Salem Player
        {
            public static void Postfix(Character __instance)
            {
                /*
                if (__instance.statuses.Contains(lockedOut))
                
                    HintInfo info = new HintInfo();
                    info.text = "I am <color=#BA4848>Locked</color> and cannot be Revealed!";
                    UIEvents.OnShowHint.Invoke(info, __instance.hintPivot);
                
                }
                */

            }
        }
    }
    [HarmonyPatch(typeof(Character), nameof(Character.OnClick))]
    public static class LockoutPatch // Code taken from Power Play by That Town Of Salem Player
    {
        static bool Prefix(Character __instance)
        {

            if (__instance == null)
                return true;

            if (__instance.statuses.Contains(LockedOut.lockedOut) && !Gameplay.GameplayState.Equals(EGameplayState.Killing))
            {
                return false;
            }
            return true;
        }
    }
}