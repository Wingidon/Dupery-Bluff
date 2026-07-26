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
public class w_Dupe_Therapist : Role
{
    public override ActedInfo GetInfo(Character charRef)
    {
        int evilClients = 0;
        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        foreach (Character character in sharedScripts.GetCharacterNeighbours(charRef))
        {
            if (character.GetRegisterAlignment() == EAlignment.Evil) evilClients++;
        }
        return new ActedInfo(ConjureInfo(evilClients), sharedScripts.GetCharacterNeighbours(charRef));
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        int evilClients = 0;
        int fakeEvilClients = 0;
        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        Il2CppSystem.Collections.Generic.List<Character> fakeEvilTeam = sharedScripts.GetFakeEvilTeam();
        foreach (Character character in sharedScripts.GetCharacterNeighbours(charRef))
        {
            if (character.GetRegisterAlignment() == EAlignment.Evil) evilClients++;
            if (fakeEvilTeam.Contains(character)) fakeEvilClients++;
        }
        sharedScripts.MakeNumberWrong(evilClients, fakeEvilClients, 0);
        return new ActedInfo(ConjureInfo(fakeEvilClients), sharedScripts.GetCharacterNeighbours(charRef));
    }
    public override string Description
    {
        get
        {
            return "Lovern't";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Therapist initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Therapist at #{charRef.id} acting.");
        OnActed(ETriggerPhase.Day, charRef, GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Therapist initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Therapist at #{charRef.id} bluff-acting.");
        OnActed(ETriggerPhase.Day, charRef, GetBluffInfo(charRef));
    }
    private string ConjureInfo(int number)
    {
        string clients = "clients";
        if (number == 1) clients = "client";
        return $"I have {number} Evil {clients}";
    }
    public w_Dupe_Therapist() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Therapist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Therapist(System.IntPtr ptr) : base(ptr)
    {
    }
}