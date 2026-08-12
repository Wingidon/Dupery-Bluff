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
public class w_Dupe_TravelAgent : w_DupeZ_RoleBase
{
    bool haveReduced = false;
    public override string Description
    {
        get
        {
            return "Witchn't";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Travel Agent initialised at #{charRef.id}");
            haveReduced = false;
        }
        if (trigger == ETriggerPhase.AfterRoundStart && !charRef.statuses.Contains(ECharacterStatus.BrokenAbility))
        {
            PlayerController.PlayerInfo.blocks.value.Add(1);
            charRef.statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef); // Prevent Wannabe shenanigans
        }
        if (trigger == ETriggerPhase.OnExecuted)
        {
            if (!haveReduced && charRef.statuses.Contains(ECharacterStatus.BrokenAbility))
            {
                charRef.statuses.statuses.Remove(ECharacterStatus.BrokenAbility); // Prevent Wannabe shenanigans
                PlayerController.PlayerInfo.blocks.value.Reduce(1);
                haveReduced = true;
            }
        }
    }
    public override void ActOnDied(Character charRef)
    {
        if (!haveReduced)
        {
            PlayerController.PlayerInfo.blocks.value.Reduce(1);
            haveReduced = true;
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        return GrabDisguise(charRef, false);
    }
    public w_Dupe_TravelAgent() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_TravelAgent>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_TravelAgent(System.IntPtr ptr) : base(ptr)
    {
    }
}