using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;

namespace DuperyBluff;

[RegisterTypeInIl2Cpp]
public class w_Dupe_Youngster : w_DupeZ_RoleBase
{
    public CharacterData[] allDatas = Il2CppSystem.Array.Empty<CharacterData>();
    public override string Description
    {
        get
        {
            return "Deals major damage if Executed";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Youngster initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.OnExecuted)
        {
            int damage = System.Int32.Parse(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Youngster_Damage").GetValueAsString());
            if (charRef.alignment == EAlignment.Evil) return;
            Health health = PlayerController.PlayerInfo.health;
            health.Damage(damage);
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Youngster initialised at #{charRef.id}");
    }
    public w_Dupe_Youngster() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Youngster>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Youngster(System.IntPtr ptr) : base(ptr)
    {
    }
}