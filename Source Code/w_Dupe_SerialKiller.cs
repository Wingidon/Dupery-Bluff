using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace DuperyBluff;

[RegisterTypeInIl2Cpp]
public class w_Dupe_SerialKiller : Role
{
    int killTimer = 0;
    public override string Description
    {
        get
        {
            return "Stabs people every few hours.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Serial Killer initialised at #{charRef.id}");
        }
        if (trigger == wx_SavedScripts.w_AnyRevealPatch.AnyReveal)
        {
            if (charRef.state == ECharacterState.Dead) return;
            killTimer++;
            if (killTimer > 3)
            {
                int killRange = System.Int32.Parse(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("SerialKiller_Range").GetValueAsString());
                int killDamage = System.Int32.Parse(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("SerialKiller_Damage").GetValueAsString());
                killTimer -= 4;
                wx_SavedScripts sharedScripts = new wx_SavedScripts();
                Il2CppSystem.Collections.Generic.List<Character> killTargets = sharedScripts.GetCharactersWithinRange(charRef, killRange);
                killTargets = Characters.Instance.FilterAliveCharacters(killTargets);
                killTargets = Characters.Instance.FilterAlignmentCharacters(killTargets, EAlignment.Good);
                killTargets = Characters.Instance.FilterRealAlignmentCharacters(killTargets, EAlignment.Good);
                if (killTargets.Count != 0)
                {
                    Character target = killTargets[UnityEngine.Random.RandomRangeInt(0, killTargets.Count)];
                    sharedScripts.DebugMessage($"Serial Killer at #{charRef.id} chose to stab #{target.id}");
                    target.KillByDemon(charRef);
                }
                else
                {
                    sharedScripts.DebugMessage($"Serial Killer at #{charRef.id} couldn't find anyone to stab!");
                }
                PlayerController.PlayerInfo.health.Damage(killDamage);
            }
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        wx_SavedScripts sharedScripts = new();
        CharacterData bluff = sharedScripts.GetOverrideDuplicateBluff(charRef);
        if (new wx_SavedScripts().PercentChance(50)) bluff = sharedScripts.GetOverrideNotInPlayBluff(charRef, true);
        sharedScripts.DebugMessage($"Serial Killer at #{charRef.id} chose {bluff.characterName} as bluff");
        Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);
        return bluff;
    }
    public w_Dupe_SerialKiller() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_SerialKiller>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_SerialKiller(System.IntPtr ptr) : base(ptr)
    {
    }
}