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
public class w_Dupe_Mobster : w_DupeZ_RoleBase
{
    public override string Description
    {
        get
        {
            return "Minion't";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Mobster initialised at #{charRef.id}");
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        return new wx_SavedScripts().GetOverrideDuplicateBluff(charRef);
    }
    public w_Dupe_Mobster() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Mobster>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Mobster(System.IntPtr ptr) : base(ptr)
    {
    }
}