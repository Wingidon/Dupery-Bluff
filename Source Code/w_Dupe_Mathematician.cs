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
public class w_Dupe_Mathematician : w_DupeZ_RoleBase
{
    public override ActedInfo GetInfo(Character charRef)
    {
        int evilSum = 0;
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.GetRegisterAlignment() == EAlignment.Evil) evilSum += character.id;
        }
        return new ActedInfo(ConjureInfo(evilSum));
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        int evilSum = 0;
        int evilCount = 0;
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.GetRegisterAlignment() == EAlignment.Evil)
            {
                evilSum += character.id;
                evilCount++;
            }
        }
        int minNumberAdd = 0;
        for (int i = 0; i < evilCount; i++)
        {
            minNumberAdd += (i + 1);
        }

        float maxModifierFloat = 4;
        maxModifierFloat = Mathf.Max(4, evilSum * 0.35f);
        maxModifierFloat = Mathf.Min(maxModifierFloat, 10);
        int maxModifierInt = Mathf.RoundToInt(maxModifierFloat);
        int highestAllowedAmount = 1000;
        if (evilCount == 1) highestAllowedAmount = Gameplay.CurrentCharacters.Count;
        /*
        Il2CppSystem.Collections.Generic.List<int> modifiers = new();
        if (evilSum - 4 >= minNumberAdd) modifiers.Add(-4); // Avoid saying impossibly low numbers
        if (evilSum - 3 >= minNumberAdd) modifiers.Add(-3);
        if (evilSum - 2 >= minNumberAdd) modifiers.Add(-2);
        if (evilSum - 1 >= minNumberAdd) modifiers.Add(-1);
        if (evilCount != 1 || Gameplay.CurrentCharacters.Count > evilSum + 1) modifiers.Add(1); // Avoid saying impossibly high numbers in a 1-Evil game.
        if (evilCount != 1 || Gameplay.CurrentCharacters.Count > evilSum + 2) modifiers.Add(2);
        if (evilCount != 1 || Gameplay.CurrentCharacters.Count > evilSum + 3) modifiers.Add(3);
        if (evilCount != 1 || Gameplay.CurrentCharacters.Count > evilSum + 4) modifiers.Add(4);
        int chosenModifier = modifiers[UnityEngine.Random.RandomRangeInt(0, modifiers.Count)];
        evilSum += chosenModifier;
        */
        sharedScripts.DebugMessage($"Lying Mathematician at #{charRef.id} found sum of {evilSum}. Making it wrong:\nMinimum number: {minNumberAdd}\nMaximum number: {highestAllowedAmount}\nRange: {maxModifierInt}");
        evilSum = sharedScripts.MakeNumberWrongByRange(evilSum, evilSum, minNumberAdd, highestAllowedAmount, maxModifierInt, maxModifierInt);
        sharedScripts.DebugMessage($"Lying Mathematician at #{charRef.id} chose to say {evilSum}");
        return new ActedInfo(ConjureInfo(evilSum));
    }
    public override string Description
    {
        get
        {
            return "Learns the sum of all Evil.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Mathematician initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Mathematician at #{charRef.id} acting.");
        OnActed(ETriggerPhase.Day, charRef, GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Mathematician initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Mathematician at #{charRef.id} bluff-acting.");
        OnActed(ETriggerPhase.Day, charRef, GetBluffInfo(charRef));
    }
    private string ConjureInfo(int number)
    {
        return $"I have calculated the number {number}";
    }
    public w_Dupe_Mathematician() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Mathematician>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Mathematician(System.IntPtr ptr) : base(ptr)
    {
    }
}