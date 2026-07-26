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
public class w_Dupe_Reporter : Role
{
    public override ActedInfo GetInfo(Character charRef)
    {
        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        Il2CppSystem.Collections.Generic.List<Character> evilTeam = Characters.Instance.FilterAlignmentCharacters(Gameplay.CurrentCharacters, EAlignment.Evil);
        if (evilTeam.Count == 0)
        {
            return new ActedInfo("I did not find any Evil", Gameplay.CurrentCharacters);
        }
        int dist = sharedScripts.GetClosestDistance(evilTeam, charRef);
        return new ActedInfo(ConjureInfo(charRef, dist), Characters.Instance.GetCharactersAtRange(dist, charRef));
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        Il2CppSystem.Collections.Generic.List<Character> evilTeam = Characters.Instance.FilterAlignmentCharacters(Gameplay.CurrentCharacters, EAlignment.Evil);
        Il2CppSystem.Collections.Generic.List<Character> fakeEvilTeam = sharedScripts.GetFakeEvilTeam();
        int trueDist = sharedScripts.GetClosestDistance(evilTeam, charRef);
        sharedScripts.DebugMessage($"True distance is {trueDist}");
        int falseDist = sharedScripts.GetClosestDistance(fakeEvilTeam, charRef);
        sharedScripts.DebugMessage($"False distance is {falseDist}");
        if (falseDist > Gameplay.CurrentCharacters.Count) falseDist = 1;
        if (trueDist == falseDist)
        {
            if (falseDist == 1) falseDist += 1;
            else falseDist -= 1;
        }
        sharedScripts.DebugMessage($"After making sure it's false, chosen distance is {falseDist}");
        return new ActedInfo(ConjureInfo(charRef, falseDist), Characters.Instance.GetCharactersAtRange(falseDist, charRef));
    }
    public override string Description
    {
        get
        {
            return "Huntern't";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Reporter initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Reporter at #{charRef.id} acting.");
        OnActed(ETriggerPhase.Day, charRef, GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Reporter initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Lying Reporter at #{charRef.id} acting.");
        OnActed(ETriggerPhase.Day, charRef, GetBluffInfo(charRef));
    }
    private string ConjureInfo(Character charRef, int steps)
    {
        if (steps == 1) return "I'm 1 step away from closest Evil";
        else return $"I'm {steps} steps away from closest Evil";
    }
    public w_Dupe_Reporter() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Reporter>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Reporter(System.IntPtr ptr) : base(ptr)
    {
    }
}