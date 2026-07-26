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
public class w_Dupe_Tailor : Role
{
    Character chRef;
    private Il2CppSystem.Action action1;
    private Il2CppSystem.Action action2;
    private Il2CppSystem.Action action3;
    public override string Description
    {
        get
        {
            return "Picks 2 and confirms their alignments";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Tailor initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Day)
        {
            new wx_SavedScripts().DebugMessage($"Tailor at #{charRef.id} acting");
            chRef = charRef;
            CharacterPicker.Instance.StartPickCharacters(2, charRef);
            CharacterPicker.OnCharactersPicked += action1;
            CharacterPicker.OnStopPick += action2;
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Lying Tailor initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Day)
        {
            new wx_SavedScripts().DebugMessage($"Lying Tailor at #{charRef.id} acting");
            chRef = charRef;
            CharacterPicker.Instance.StartPickCharacters(2, charRef);
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
        chars.Add(CharacterPicker.PickedCharacters[1]);
        bool aligned = false;
        if (chars[0].GetRegisterAlignment() == chars[1].GetRegisterAlignment()) aligned = true;
        OnActed(ETriggerPhase.Day, chRef, new ActedInfo(ConjureInfo(chars, aligned), chars));
    }
    private void CharacterPickedLiar()
    {
        CharacterPicker.OnCharactersPicked -= action3;
        CharacterPicker.OnStopPick -= action2;
        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        chars.Add(CharacterPicker.PickedCharacters[0]);
        chars.Add(CharacterPicker.PickedCharacters[1]);
        bool aligned = false;
        if (chars[0].GetRegisterAlignment() == chars[1].GetRegisterAlignment()) aligned = true;
        OnActed(ETriggerPhase.Day, chRef, new ActedInfo(ConjureInfo(chars, !aligned), chars));
    }
    private string ConjureInfo(Il2CppSystem.Collections.Generic.List<Character> targets, bool aligned)
    {
        Il2CppSystem.Collections.Generic.List<Character> mentionTargets = new wx_SavedScripts().SortList(targets);
        if (aligned) return $"#{mentionTargets[0].id} and #{mentionTargets[1].id} are the same alignment";
        return $"#{mentionTargets[0].id} and #{mentionTargets[1].id} are not the same alignment";
    }
    private void StopPick()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        CharacterPicker.OnCharactersPicked -= action3;
    }
    public w_Dupe_Tailor() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Tailor>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
    public w_Dupe_Tailor(System.IntPtr ptr) : base(ptr)
    {
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
}