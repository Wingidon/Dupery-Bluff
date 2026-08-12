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
public class w_Dupe_Belfry : w_DupeZ_RoleBase
{
    public override string Description
    {
        get
        {
            return "Rings the Clock Tower";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Belfry initialised at #{charRef.id}");
        if (trigger == ETriggerPhase.Day)
        {
            RingClocktower();
            OnActed(ETriggerPhase.Day, charRef, new ActedInfo("Bing Bong\nBing Bong!"));
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Belfry initialised at #{charRef.id}");
        if (trigger == ETriggerPhase.Day)
        {
            OnActed(ETriggerPhase.Day, charRef, new ActedInfo("Bing Bong\nBing Bong!"));
        }
    }
    public w_Dupe_Belfry() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Belfry>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Belfry(System.IntPtr ptr) : base(ptr)
    {
    }
}