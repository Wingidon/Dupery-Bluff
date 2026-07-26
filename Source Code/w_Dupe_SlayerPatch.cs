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
public class w_Dupe_SlayerPatch : Role
{
    Character chRef;
    private Il2CppSystem.Action action1;
    private Il2CppSystem.Action action2;
    private Il2CppSystem.Action action3;
    public override string Description
    {
        get
        {
            return "Pick 1. If Evil, I Execute them.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            chRef = charRef;
            CharacterPicker.Instance.StartPickCharacters(1, charRef);
            CharacterPicker.OnCharactersPicked += action1;
            CharacterPicker.OnStopPick += action2;
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
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
        if (chars[0] == chRef) return;
        if (chars[0].state == ECharacterState.Dead) return;
        if (chars[0].GetRegisterAlignment() == EAlignment.Evil)
        {
            if (chars[0].role.CheckIfCanBeKilled(chars[0])) chars[0].ExecuteAndReveal();
            OnActed(ETriggerPhase.Day, chRef, new ActedInfo($"I killed Evil\nat #{chars[0].id}", chars));
        }
        else
        {
            OnActed(ETriggerPhase.Day, chRef, new ActedInfo($"I couldn't kill #{chars[0].id}", chars));
        }
    }
    private void CharacterPickedLiar()
    {
        CharacterPicker.OnCharactersPicked -= action3;
        CharacterPicker.OnStopPick -= action2;
        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        chars.Add(CharacterPicker.PickedCharacters[0]);
        if (chars[0] == chRef) return;
        if (chars[0].state == ECharacterState.Dead) return;
        OnActed(ETriggerPhase.Day, chRef, new ActedInfo($"I couldn't kill #{chars[0].id}", chars));
    }
    private void StopPick()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        CharacterPicker.OnCharactersPicked -= action3;
    }
    public w_Dupe_SlayerPatch() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_SlayerPatch>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
    public w_Dupe_SlayerPatch(System.IntPtr ptr) : base(ptr)
    {
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
}