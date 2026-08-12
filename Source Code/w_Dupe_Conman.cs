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
public class w_Dupe_Conman : w_DupeZ_RoleBase
{
    public override string Description
    {
        get
        {
            return "Truthful Evil";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Conman initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            charRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
            charRef.statuses.AddStatus(ECharacterStatus.WorkingAbility, charRef);
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        charRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
        charRef.statuses.AddStatus(ECharacterStatus.WorkingAbility, charRef);
        return new wx_SavedScripts().GetOverrideDuplicateBluff(charRef);
    }
    public w_Dupe_Conman() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Conman>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Conman(System.IntPtr ptr) : base(ptr)
    {
    }
}