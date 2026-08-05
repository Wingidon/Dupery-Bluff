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
public class w_Dupe_GoodCop : Role
{
    Character chRef;
    private Il2CppSystem.Action action1;
    private Il2CppSystem.Action action2;
    private Il2CppSystem.Action action3;
    public override string Description
    {
        get
        {
            return "Turn another character's alignment to my own.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Good Cop initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Day)
        {
            new wx_SavedScripts().DebugMessage($"Good Cop at #{charRef.id} acting");
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
            new wx_SavedScripts().DebugMessage($"Lying Good Cop initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Day)
        {
            new wx_SavedScripts().DebugMessage($"Lying Good Cop at #{charRef.id} acting");
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
        if (chRef.alignment != chars[0].alignment)
        {
            bool lying = CharacterHelper.CheckLying(chars[0]);
            chars[0].ChangeAlignment(chRef.alignment);
            if (chRef.alignment == EAlignment.Good) chars[0].statuses.AddStatus(GoodCopBadCop.w_dupe_CopGood, charRef);
            if (chRef.alignment == EAlignment.Evil) chars[0].statuses.AddStatus(GoodCopBadCop.w_dupe_CopBad, charRef);
            if (!lying) chars[0].statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
        }
        OnActed(ETriggerPhase.Day, chRef, new ActedInfo(ConjureInfo(chars[0]), chars));
        Il2CppSystem.Collections.Generic.List<Character> aliveEvils = Characters.Instance.FilterAliveCharacters(Gameplay.CurrentCharacters);
        aliveEvils = Characters.Instance.FilterRealAlignmentCharacters(aliveEvils, EAlignment.Evil);
        bool evilLives = aliveEvils.Count != 0;
        if (!evilLives) chars[0].KillByDemon(chRef);
    }
    private void CharacterPickedLiar()
    {
        CharacterPicker.OnCharactersPicked -= action3;
        CharacterPicker.OnStopPick -= action2;
        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        chars.Add(CharacterPicker.PickedCharacters[0]);
        if (chars[0] == charRef) return;
        OnActed(ETriggerPhase.Day, chRef, new ActedInfo(ConjureInfo(chars[0]), chars));
    }
    private string ConjureInfo(Character target)
    {
        return $"I befriended #{target.id}";
    }
    private void StopPick()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        CharacterPicker.OnCharactersPicked -= action3;
    }
    public w_Dupe_GoodCop() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_GoodCop>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
    public w_Dupe_GoodCop(System.IntPtr ptr) : base(ptr)
    {
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
}
public static class GoodCopBadCop
{
    public static ECharacterStatus w_dupe_CopGood = (ECharacterStatus)315167151;
    public static ECharacterStatus w_dupe_CopBad = (ECharacterStatus)31516214;

    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(w_dupe_CopGood))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#8BC6E4><size=18>\n<Befriended></color></size>";
            }
            if (__instance.statuses.Contains(w_dupe_CopBad))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#BA4848><size=18>\n<Evil></color></size>";
            }
        }
    }
}