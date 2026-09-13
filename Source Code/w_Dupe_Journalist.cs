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
public class w_Dupe_Journalist : w_DupeZ_RoleBase
{
    public override ActedInfo GetInfo(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override string Description
    {
        get
        {
            return "Confessorn't";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Journalist initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Journalist at #{charRef.id} acting.");
        Il2CppSystem.Collections.Generic.List<Character> self = new();
        self.Add(charRef);
        OnActed(ETriggerPhase.Day, charRef, new ActedInfo(ConjureInfo(charRef, false)));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Journalist initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Journalist at #{charRef.id} bluff-acting.");
        Il2CppSystem.Collections.Generic.List<Character> self = new();
        self.Add(charRef);
        OnActed(ETriggerPhase.Day, charRef, new ActedInfo(ConjureInfo(charRef, true)));
    }
    private string ConjureInfo(Character charRef, bool lying)
    {
        string an = "a";
        int healthAmount = GetHealth();
        Il2CppSystem.Collections.Generic.List<Character> aliveCharacters = Characters.Instance.FilterAliveCharacters(Gameplay.CurrentCharacters);
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.dataRef.characterId == "WING_Dupery_Critic" && aliveCharacters.Contains(character))
            {
                healthAmount = 5;
                break;
            }
        }
        if (healthAmount.ToString().Contains("8") || healthAmount == 11) an = "an";
        if (!lying) return $"People think you are {an} {healthAmount}/10 Executioner";
        else
        {
            int falseHealth = new wx_SavedScripts().MakeNumberWrongByRange(healthAmount, healthAmount, 1, 10, 10, 10);
            an = "a";
            if (falseHealth.ToString().Contains("8") || falseHealth == 11) an = "an";
            return $"People think you are {an} {falseHealth}/10 Executioner";
        }
    }
    private int GetHealth()
    {
        Health health = PlayerController.PlayerInfo.health;
        int healthCount = health.value.GetValue();
        return healthCount;
    }
    public w_Dupe_Journalist() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Journalist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Journalist(System.IntPtr ptr) : base(ptr)
    {
    }
}