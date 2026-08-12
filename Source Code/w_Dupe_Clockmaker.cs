using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;

namespace DuperyBluff;

[RegisterTypeInIl2Cpp]
public class w_Dupe_Clockmaker : w_DupeZ_RoleBase
{
    public override ActedInfo GetInfo(Character charRef)
    {
        int time = -1;
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.statuses.Contains(clocktowerMarker) || character.statuses.Contains(clocktowerGoneOff))
            {
                time = character.id;
            }
        }
        return new ActedInfo(ConjureInfo(charRef, time));
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<int> possibleTimes = new();
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (!character.statuses.Contains(clocktowerMarker) && !character.statuses.Contains(clocktowerGoneOff))
            {
                possibleTimes.Add(character.id);
            }
        }
        int time = possibleTimes[UnityEngine.Random.RandomRangeInt(0, possibleTimes.Count)];
        return new ActedInfo(ConjureInfo(charRef, time));
    }
    public override string Description
    {
        get
        {
            return "Confessorn't but clocktower";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Clock Maker initialised at #{charRef.id}");
        if (trigger == ETriggerPhase.AfterRoundStart) MarkClocktower();
        if (trigger == wx_SavedScripts.w_AnyRevealPatch.AnyReveal) CheckClockTimer();
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Clock Maker at #{charRef.id} acting.");
        OnActed(ETriggerPhase.Day, charRef, GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Clock Maker initialised at #{charRef.id}");
        if (trigger == ETriggerPhase.AfterRoundStart) MarkClocktower();
        if (trigger == wx_SavedScripts.w_AnyRevealPatch.AnyReveal) CheckClockTimer();
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Clock Maker at #{charRef.id} bluff-acting.");
        OnActed(ETriggerPhase.Day, charRef, GetBluffInfo(charRef));
    }
    private string ConjureInfo(Character charRef, int time)
    {
        if (time == -1) return "The Clock Tower will not ring";
        return $"The Clock Tower will ring at {time}h";
    }
    public w_Dupe_Clockmaker() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Clockmaker>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Clockmaker(System.IntPtr ptr) : base(ptr)
    {
    }
}