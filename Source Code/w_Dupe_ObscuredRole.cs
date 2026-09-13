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
public class w_Dupe_ObscuredRole : w_DupeZ_RoleBase
{
    public override ActedInfo GetInfo(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override string Description
    {
        get
        {
            return "OooOOOooOOOooO sPOOOoooOKYYYyyyYY";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Obscured Role initialised at #{charRef.id}. Why wasn't this character's .role replaced?");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Obscured Role at #{charRef.id} acting. Why wasn't this character's .role replaced?");
        Il2CppSystem.Collections.Generic.List<Character> self = new();
        self.Add(charRef);
        OnActed(ETriggerPhase.Day, charRef, new ActedInfo(ConjureInfo(charRef, false), self));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Obscured Role initialised at #{charRef.id}. Why wasn't this character's .role replaced?");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Obscured Role at #{charRef.id} bluff-acting. Why wasn't this character's .role replaced?");
        Il2CppSystem.Collections.Generic.List<Character> self = new();
        self.Add(charRef);
        OnActed(ETriggerPhase.Day, charRef, new ActedInfo(ConjureInfo(charRef, true), self));
    }
    private string ConjureInfo(Character charRef, bool lying)
    {
        if (!lying) return "I am the Priest";
        else
        {
            Il2CppSystem.Collections.Generic.List<string> returnList = new();
            returnList.Add("I am the prest");
            returnList.Add("I am the monk");
            returnList.Add("I don't feel so good...");
            returnList.Add("Am I a good priest?");
            returnList.Add("Am I a holy priest?");
            returnList.Add("Have you come to pray?");
            returnList.Add("I was the priest");
            returnList.Add("I'll be the priest");
            string returnString = returnList[UnityEngine.Random.RandomRangeInt(0, returnList.Count)];
            return returnString;
        }
    }
    public w_Dupe_ObscuredRole() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_ObscuredRole>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_ObscuredRole(System.IntPtr ptr) : base(ptr)
    {
    }
}