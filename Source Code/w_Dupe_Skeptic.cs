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
public class w_Dupe_Skeptic : Role
{
    public override ActedInfo GetInfo(Character charRef)
    {
        Health health = PlayerController.PlayerInfo.health;
        int healthCount = health.value.GetValue();
        if (healthCount <= 6)
        {
            return new ActedInfo(ConjureInfo(charRef, false, false));
        }
        Il2CppSystem.Collections.Generic.List<Character> selection = new();
        Il2CppSystem.Collections.Generic.List<Character> validTargets = new();
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.id != charRef.id) validTargets.Add(character);
        }
        selection.Add(validTargets[UnityEngine.Random.RandomRangeInt(0, validTargets.Count)]);
        bool targetGood = selection[0].GetRegisterAlignment() == EAlignment.Good;
        return new ActedInfo(ConjureInfo(selection[0], targetGood, true), selection);
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> selection = new();
        Il2CppSystem.Collections.Generic.List<Character> validTargets = new();
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.id != charRef.id) validTargets.Add(character);
        }
        selection.Add(validTargets[UnityEngine.Random.RandomRangeInt(0, validTargets.Count)]);
        bool targetGood = selection[0].GetRegisterAlignment() == EAlignment.Good;
        return new ActedInfo(ConjureInfo(selection[0], !targetGood, true), selection);
    }
    public override string Description
    {
        get
        {
            return "Alignment Cop";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Skeptic initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Skeptic at #{charRef.id} acting.");
        OnActed(ETriggerPhase.Day, charRef, GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Skeptic initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Skeptic at #{charRef.id} bluff-acting.");
        OnActed(ETriggerPhase.Day, charRef, GetBluffInfo(charRef));
    }
    private string ConjureInfo(Character target, bool targetGood, bool trust)
    {
        if (!trust) return "Sorry, you can't be trusted";
        if (targetGood) return $"#{target.id} is Good";
        else return $"#{target.id} is Evil";
    }
    public w_Dupe_Skeptic() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Skeptic>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Skeptic(System.IntPtr ptr) : base(ptr)
    {
    }
}