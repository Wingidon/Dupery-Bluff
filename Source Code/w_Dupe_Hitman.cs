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
public class w_Dupe_Hitman : w_DupeZ_RoleBase
{
    int killTimer = 0;
    public override string Description
    {
        get
        {
            return "Stabs people every couple of hours.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Hitman initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Start)
        {
            RemoveNightActors();
            MarkClocktower();
        }
        if (trigger == wx_SavedScripts.w_AnyRevealPatch.AnyReveal)
        {
            CheckClockTimer();
            if (charRef.state == ECharacterState.Dead) return;
            killTimer++;
            if (killTimer > 1)
            {
                int killDamage = System.Int32.Parse(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Hitman_Damage").GetValueAsString());
                Il2CppSystem.Collections.Generic.List<string> trueResults = new();
                trueResults.Add("true");
                trueResults.Add("True");
                trueResults.Add("TRUE");
                bool selfAllowed = trueResults.Contains(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Hitman_SelfAllowed").GetValueAsString());
                bool evilAllowed = trueResults.Contains(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Hitman_EvilAllowed").GetValueAsString());
                bool revealedAllowed = trueResults.Contains(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Hitman_RevealedAllowed").GetValueAsString());
                killTimer -= 2;
                wx_SavedScripts sharedScripts = new wx_SavedScripts();
                Il2CppSystem.Collections.Generic.List<Character> killTargets = Characters.Instance.FilterAliveCharacters(Gameplay.CurrentCharacters);
                if (!selfAllowed) killTargets.Remove(charRef);
                if (!evilAllowed)
                {
                    killTargets = Characters.Instance.FilterAlignmentCharacters(killTargets, EAlignment.Good);
                    killTargets = Characters.Instance.FilterRealAlignmentCharacters(killTargets, EAlignment.Good);
                }
                if (!revealedAllowed) killTargets = Characters.Instance.FilterHiddenCharacters(killTargets);
                if (killTargets.Count != 0)
                {
                    Character target = killTargets[UnityEngine.Random.RandomRangeInt(0, killTargets.Count)];
                    sharedScripts.DebugMessage($"Hitman at #{charRef.id} chose to shoot #{target.id}");
                    target.KillByDemon(charRef);
                    if (MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry<bool>("DisableRedText").Value) target.statuses.AddStatus(MainMod.HiddenRoleStatus.hiddenRole, charRef);
                }
                else
                {
                    sharedScripts.DebugMessage($"Hitman at #{charRef.id} couldn't find anyone to shoot!");
                }
                PlayerController.PlayerInfo.health.Damage(killDamage);
            }
        }
    }
    /*
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Hitman Bluff-Act initialised at #{charRef.id}");
        }
        else if (!charRef.statuses.Contains(ECharacterStatus.Corrupted) && !charRef.statuses.Contains(ECharacterStatus.BrokenAbility)) Act(trigger, charRef); // Hitman was only calling its Bluff Act for some reason.
    }
    */
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        wx_SavedScripts sharedScripts = new();
        CharacterData bluff = sharedScripts.GetOverrideNotInPlayBluff(charRef, true);
        sharedScripts.DebugMessage($"Hitman at #{charRef.id} chose {bluff.characterName} as bluff");
        Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);
        return bluff;
    }
    public w_Dupe_Hitman() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Hitman>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Hitman(System.IntPtr ptr) : base(ptr)
    {
    }
}