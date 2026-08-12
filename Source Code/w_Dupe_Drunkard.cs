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
public class w_Dupe_Drunkard : w_DupeZ_RoleBase
{
    public override string Description
    {
        get
        {
            return "Turbocharged Drunk";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Drunkard initialised at #{charRef.id}");
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        charRef.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
        return GrabDisguise(charRef, true);
    }
    public override int GetDamageToYou()
    {
        return System.Int32.Parse(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Drunkard_Damage").GetValueAsString());
    }
    public w_Dupe_Drunkard() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Drunkard>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Drunkard(System.IntPtr ptr) : base(ptr)
    {
    }
}