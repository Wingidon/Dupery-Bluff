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
public class w_Dupe_Priest : w_DupeZ_RoleBase
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
            return "Confessorn't";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Priest initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Priest at #{charRef.id} acting.");
        Il2CppSystem.Collections.Generic.List<Character> self = new();
        self.Add(charRef);
        OnActed(ETriggerPhase.Day, charRef, new ActedInfo(ConjureInfo(charRef, false), self));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Priest initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Priest at #{charRef.id} bluff-acting.");
        Il2CppSystem.Collections.Generic.List<Character> self = new();
        self.Add(charRef);
        OnActed(ETriggerPhase.Day, charRef, new ActedInfo(ConjureInfo(charRef, true), self));
    }
    private string ConjureInfo(Character charRef, bool lying)
    {
        if (!lying) return "I am the Priest";
        else
        {
            Il2CppSystem.Collections.Generic.List<string> trueResults = new();
            trueResults.Add("true");
            trueResults.Add("True");
            trueResults.Add("TRUE");
            bool expandedStatements = trueResults.Contains(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Priest_ExpandedStatements").GetValueAsString());
            Il2CppSystem.Collections.Generic.List<string> returnList = new();
            returnList.Add("I am the prest");
            returnList.Add("I am the monk");
            returnList.Add("I don't feel so good...");
            returnList.Add("Am I a good priest?");
            returnList.Add("Am I a holy priest?");
            returnList.Add("Have you come to pray?");
            returnList.Add("I was the priest");
            returnList.Add("I'll be the priest");

            returnList.Add("I am a goldfish"); // Nod to NoLucksGiven
            if (expandedStatements)
            {
                returnList.Add("I was a Priest"); // Baker
                returnList.Add("I am the original Priest"); // Baker
                returnList.Add("I was a Baker"); // Baker
                returnList.Add($"#{charRef.id} is a real Priest"); // Medium
                returnList.Add($"#{charRef.id} is actually a Priest"); // Medium
                returnList.Add("I am Good"); // Confessor
                returnList.Add("I am dizzy"); // Confessor
                returnList.Add("I am the Confessor"); // Confessor
                returnList.Add("Get it twisted!"); // Rambler (aka NL)
                returnList.Add("I am a rounding error"); // Get it twisted!
                returnList.Add("I am the Drunkard"); // Drunkard
                returnList.Add("I am drunk"); // Drunk
                returnList.Add("I could be the priest");
                returnList.Add("Am I the priest?");
                returnList.Add("I am not the priest");
                returnList.Add("I am the press");
                returnList.Add("I am the yeast");
                returnList.Add("I am an egg");
                returnList.Add("I am the Bishop");
                returnList.Add("I am the Saint");
                returnList.Add("I am the Heretic");
                returnList.Add("I am the Preacher");
                returnList.Add("Am I a real priest?");
                returnList.Add("Uh, line?");
                returnList.Add("I am about to cross a road!"); // Tom Scott, anyone?
                returnList.Add("I am imprest"); // Nod to @gangstakitten7 in the Gilded Rune Games Discord server.
            }
            string returnString = returnList[UnityEngine.Random.RandomRangeInt(0, returnList.Count)];
            return returnString;
        }
    }
    public w_Dupe_Priest() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Priest>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Priest(System.IntPtr ptr) : base(ptr)
    {
    }
}