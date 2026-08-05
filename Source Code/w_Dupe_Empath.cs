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
public class w_Dupe_Empath : Role
{
    Character chRef;
    private Il2CppSystem.Action action1;
    private Il2CppSystem.Action action2;
    private Il2CppSystem.Action action3;
    public override string Description
    {
        get
        {
            return "Picks 1 and learns if they're a Villager.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Researcher initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Day)
        {
            new wx_SavedScripts().DebugMessage($"Researcher at #{charRef.id} acting");
            chRef = charRef;
            CharacterPicker.Instance.StartPickCharacters(1, charRef);
            CharacterPicker.OnCharactersPicked += action1;
            CharacterPicker.OnStopPick += action2;
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Lying Researcher initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Day)
        {
            new wx_SavedScripts().DebugMessage($"Lying Researcher at #{charRef.id} acting");
            chRef = charRef;
            CharacterPicker.Instance.StartPickCharacters(1, charRef);
            CharacterPicker.OnCharactersPicked += action3;
            CharacterPicker.OnStopPick += action2;
        }
    }
    private void CharacterPicked()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        chars.Add(CharacterPicker.PickedCharacters[0]);
        bool confused = CharacterHelper.CheckLyingAppearance(chars[0]) == (chars[0].GetRegisterAlignment() == EAlignment.Good); // If Lying & Good OR Truthful & Evil
        OnActed(ETriggerPhase.Day, chRef, new ActedInfo(ConjureInfo(chars[0], confused), chars));
    }
    private void CharacterPickedLiar()
    {
        CharacterPicker.OnCharactersPicked -= action3;
        CharacterPicker.OnStopPick -= action2;
        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        chars.Add(CharacterPicker.PickedCharacters[0]);
        bool confused = CharacterHelper.CheckLyingAppearance(chars[0]) == (chars[0].GetRegisterAlignment() == EAlignment.Good); // If Lying & Good OR Truthful & Evil
        OnActed(ETriggerPhase.Day, chRef, new ActedInfo(ConjureInfo(chars[0], !confused), chars));
    }
    private string ConjureInfo(Character target, bool confused)
    {
        if (confused) return $"#{target.id} is confused";
        else return $"#{target.id} is committed";
    }
    private void StopPick()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        CharacterPicker.OnCharactersPicked -= action3;
    }
    public w_Dupe_Empath() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Empath>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
    public w_Dupe_Empath(System.IntPtr ptr) : base(ptr)
    {
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
}