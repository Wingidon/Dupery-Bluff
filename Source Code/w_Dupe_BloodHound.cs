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
public class w_Dupe_BloodHound : Role
{
    public override ActedInfo GetInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> charsTwice = new();

        int cwDist = 10000;
        int ccwDist = 10000;

        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.GetRegisterAlignment() == EAlignment.Evil && character != charRef)
            {
                int cwCheckDist = 100000;
                int ccwCheckDist = 100000;
                Character char1 = charRef;
                Character char2 = character;
                int totalCharCount = Gameplay.CurrentCharacters.Count;
                int tempDist = char1.id - char2.id; // #5 to #3: 2 steps CCW
                if (tempDist < 0)
                {
                    tempDist = tempDist + totalCharCount;
                }
                int tempDist2 = char2.id - char1.id; // #3 to #5: 2 steps CW
                if (tempDist2 < 0)
                {
                    tempDist2 = tempDist2 + totalCharCount;
                }
                cwCheckDist = tempDist2;
                ccwCheckDist = tempDist;

                if (cwDist > cwCheckDist) cwDist = cwCheckDist;
                if (ccwDist > ccwCheckDist) ccwDist = ccwCheckDist;
            }
        }
        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        sharedScripts.DebugMessage($"Blood Hound at #{charRef.id} found clockwise distance of {cwDist} and counter-clockwise distance of {ccwDist}");

        int infoID = 4;
        if (cwDist == 10000 && ccwDist == 10000) infoID = 3;
        else if (cwDist == ccwDist) infoID = 2;
        else if (cwDist > ccwDist) infoID = 1;
        else infoID = 0;
        return new ActedInfo(ConjureInfo(infoID));
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        ActedInfo trueInfo = GetInfo(charRef);
        string correctInfo = trueInfo.desc;
        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        sharedScripts.DebugMessage("^That was just a Lying Bloodhound checking the correct result");
        Il2CppSystem.Collections.Generic.List<string> results = new();
        if (correctInfo != "*Points Clockwise*")
        {
            results.Add("*Points Clockwise*");
            results.Add("*Points Clockwise*");
            results.Add("*Points Clockwise*");
        }
        if (correctInfo != "*Points Counter-Clockwise*")
        {
            results.Add("*Points Counter-Clockwise*");
            results.Add("*Points Counter-Clockwise*");
            results.Add("*Points Counter-Clockwise*");
        }
        if (correctInfo != "*BARK BARK BARK!*")
        {
            results.Add("*BARK BARK BARK!*");
        }
        string falseResult = results[UnityEngine.Random.RandomRangeInt(0, results.Count)];
        sharedScripts.DebugMessage($"Lying Blood Hound chose: {falseResult}");
        return new ActedInfo(falseResult);
    }
    public override string Description
    {
        get
        {
            return "Unenlightened";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Blood Hound initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Blood Hound at #{charRef.id} acting.");
        OnActed(ETriggerPhase.Day, charRef, GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Blood Hound initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Blood Hound at #{charRef.id} bluff-acting.");
        OnActed(ETriggerPhase.Day, charRef, GetBluffInfo(charRef));
    }
    private string ConjureInfo(int resultID)
    {
        switch (resultID)
        {
            case 0: return "*Points Clockwise*";
            case 1: return "*Points Counter-Clockwise*";
            case 2: return "*BARK BARK BARK!*";
            case 3: return "*sniffs*";
        }
        return "*sniffs";
    }
    public w_Dupe_BloodHound() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_BloodHound>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_BloodHound(System.IntPtr ptr) : base(ptr)
    {
    }
}