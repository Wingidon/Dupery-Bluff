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
public class w_Dupe_Copycat : w_DupeZ_RoleBase
{
    public override string Description
    {
        get
        {
            return "Doppelgangern't";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Copycat initialised at #{charRef.id}");
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        charRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
        charRef.statuses.AddStatus(ECharacterStatus.WorkingAbility, charRef);
        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        CharacterData bluff = new wx_SavedScripts().GetOverrideDuplicateBluff(charRef);
        sharedScripts.DebugMessage($"Copycat at #{charRef.id} chose bluff of {bluff.characterName}");
        return bluff;
    }
    public override bool CheckIfCanBeKilled(Character charRef)
    {
        if (charRef.statuses.statuses.Contains(ECharacterStatus.HealthyBluff) && charRef.bluff) return charRef.bluff.role.CheckIfCanBeKilled(charRef);
        return true;
    }
    public w_Dupe_Copycat() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Copycat>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Copycat(System.IntPtr ptr) : base(ptr)
    {
    }
}