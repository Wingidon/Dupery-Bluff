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
public class w_Dupe_Critic : Role
{
    public override string Description
    {
        get
        {
            return "Lose most of your health.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Critic initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            new wx_SavedScripts().DebugMessage($"Critic at #{charRef.id} acting.");
            wx_SavedScripts sharedScripts = new wx_SavedScripts();
            Health health = PlayerController.PlayerInfo.health;
            if (health.value.GetValue() > 5) health.AddMaxHp(-5);
        }
    }
    public override void ActOnDied(Character charRef)
    {
        Health health = PlayerController.PlayerInfo.health;
        health.AddMaxHp(5);
        health.Damage(5);
        health.Heal(health.value.GetValue());
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        wx_SavedScripts sharedScripts = new();
        CharacterData bluff = sharedScripts.GetOverrideNotInPlayBluff(charRef, true);
        sharedScripts.DebugMessage($"Critic at #{charRef.id} chose {bluff.characterName} as bluff");
        Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);
        return bluff;
    }
    public w_Dupe_Critic() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Critic>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Critic(System.IntPtr ptr) : base(ptr)
    {
    }
}