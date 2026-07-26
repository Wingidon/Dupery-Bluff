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
public class w_Dupe_PrivateEye : Role
{
    int infoTimer = 0;
    bool haveActed = false;
    bool shouldAct = false;
    ActedInfo info = new ActedInfo("");
    public override ActedInfo GetInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> unrevealedChars = Characters.Instance.FilterHiddenCharacters(Gameplay.CurrentCharacters);
        unrevealedChars.Remove(charRef);
        unrevealedChars = Characters.Instance.FilterCharacterMissingStatus(unrevealedChars, wx_SavedScripts.w_AnyRevealPatch.JustRevealed);
        Il2CppSystem.Collections.Generic.List<Character> unrevealedMinions = Characters.Instance.FilterCharacterType(unrevealedChars, ECharacterType.Minion);
        string info = "";
        if (unrevealedMinions.Count == 0) return new ActedInfo("I got nothing", unrevealedChars);
        Character target = unrevealedMinions[UnityEngine.Random.RandomRangeInt(0, unrevealedMinions.Count)];
        unrevealedMinions.Clear();
        unrevealedMinions.Add(target);
        return new ActedInfo(ConjureInfo(target), unrevealedMinions);
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> unrevealedChars = Characters.Instance.FilterHiddenCharacters(Gameplay.CurrentCharacters);
        unrevealedChars.Remove(charRef);
        unrevealedChars = Characters.Instance.FilterCharacterMissingStatus(unrevealedChars, wx_SavedScripts.w_AnyRevealPatch.JustRevealed);
        Il2CppSystem.Collections.Generic.List<Character> unrevealedMinions = new();
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (unrevealedChars.Contains(character) && character.GetRegisterAs().type != ECharacterType.Minion) unrevealedMinions.Add(character);
        }
        string info = "";
        if (unrevealedMinions.Count == 0) return new ActedInfo("I got nothing", unrevealedChars);
        Character target = unrevealedMinions[UnityEngine.Random.RandomRangeInt(0, unrevealedMinions.Count)];
        unrevealedMinions.Clear();
        unrevealedMinions.Add(target);
        return new ActedInfo(ConjureInfo(target), unrevealedMinions);
    }
    public override string Description
    {
        get
        {
            return "Learns an unrevealed Minion on the 10th hour";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Private Eye initialised at #{charRef.id}");
            infoTimer = 0;
            haveActed = false;
            shouldAct = false;
        }
        if (trigger == wx_SavedScripts.w_AnyRevealPatch.AnyReveal)
        {
            if (charRef.state == ECharacterState.Dead) return;
            infoTimer++;
            if (infoTimer.ToString() == MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("PrivateEye_InfoHour").GetValueAsString())
            {
                new wx_SavedScripts().DebugMessage($"Private Eye at #{charRef.id} ready to act!");
                if (haveActed) OnActed(ETriggerPhase.Day, charRef, GetInfo(charRef));
                else shouldAct = true;
            }
        }
        if (trigger == ETriggerPhase.Day)
        {
            if (charRef.state == ECharacterState.Dead) return;
            haveActed = true;
            if (shouldAct) OnActed(ETriggerPhase.Day, charRef, GetInfo(charRef));
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Lying Private Eye initialised at #{charRef.id}");
            infoTimer = 0;
            haveActed = false;
            shouldAct = false;
        }
        if (trigger == wx_SavedScripts.w_AnyRevealPatch.AnyReveal)
        {
            if (charRef.state == ECharacterState.Dead) return;
            infoTimer++;
            if (infoTimer.ToString() == MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("PrivateEye_InfoHour").GetValueAsString())
            {
                new wx_SavedScripts().DebugMessage($"Lying Private Eye at #{charRef.id} ready to act!");
                if (haveActed) OnActed(ETriggerPhase.Day, charRef, GetBluffInfo(charRef));
                else shouldAct = true;
            }
        }
        if (trigger == ETriggerPhase.Day)
        {
            if (charRef.state == ECharacterState.Dead) return;
            haveActed = true;
            if (shouldAct) OnActed(ETriggerPhase.Day, charRef, GetBluffInfo(charRef));
        }
    }
    private string ConjureInfo(Character target)
    {
        if (target == charRef) return "I got nothing";
        else return $"#{target.id} is a Minion";
    }
    public w_Dupe_PrivateEye() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_PrivateEye>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_PrivateEye(System.IntPtr ptr) : base(ptr)
    {
    }
}