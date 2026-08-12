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
public class w_Dupe_Surgeon : w_DupeZ_RoleBase
{
    int timer = 0;
    bool haveStabbed = false;
    public override string Description
    {
        get
        {
            return "Sometimes kills a Villager at 12h";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (charRef.state == ECharacterState.Dead) return;
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Surgeon initialised at #{charRef.id}");
            timer = 0;
            haveStabbed = false;
        }
        if (trigger != wx_SavedScripts.w_AnyRevealPatch.AnyReveal) return;
        int surgeonDamage = System.Int32.Parse(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Surgeon_Damage").GetValueAsString());
        int surgeonKillChance = System.Int32.Parse(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Surgeon_KillChance").GetValueAsString());
        int surgeonKillHour = System.Int32.Parse(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Surgeon_KillHour").GetValueAsString());
        timer++;
        if (timer >= surgeonKillHour && !haveStabbed)
        {
            haveStabbed = true;
            wx_SavedScripts sharedScripts = new wx_SavedScripts();
            sharedScripts.DebugMessage($"Surgeon at #{charRef.id} acting.");
            if (UnityEngine.Random.RandomRange(0.0f, 100.0f) <= surgeonKillChance)
            {
                Il2CppSystem.Collections.Generic.List<Character> validTargets = Characters.Instance.FilterCharacterType(Gameplay.CurrentCharacters, ECharacterType.Villager);
                validTargets = Characters.Instance.FilterAlignmentCharacters(validTargets, EAlignment.Good);
                validTargets = Characters.Instance.FilterRealAlignmentCharacters(validTargets, EAlignment.Good); // Also prevent the Surgeon from stabbing an Evil Registering as Good.
                validTargets = Characters.Instance.FilterAliveCharacters(validTargets);
                Health health = PlayerController.PlayerInfo.health;
                if (validTargets.Count != 0)
                {
                    Character target = validTargets[UnityEngine.Random.RandomRangeInt(0, validTargets.Count)];
                    sharedScripts.DebugMessage($"Surgeon chose to stab #{target.id}");
                    target.KillByDemon(charRef);
                    if (surgeonDamage != 0) health.Damage(surgeonDamage);
                }
                else
                {
                    sharedScripts.DebugMessage("Surgeon wanted to stab, but couldn't find any Villagers!");
                }
            }
            else
            {
                sharedScripts.DebugMessage($"Surgeon didn't botch their surgery.");
            }
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Surgeon initialised at #{charRef.id}");
    }
    public w_Dupe_Surgeon() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Surgeon>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Surgeon(System.IntPtr ptr) : base(ptr)
    {
    }
}