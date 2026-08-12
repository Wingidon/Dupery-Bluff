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
public class w_Dupe_Weatherman : w_DupeZ_RoleBase
{
    public override ActedInfo GetInfo(Character charRef)
    {
        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        Il2CppSystem.Collections.Generic.List<Character> villagerChars = new();
        Il2CppSystem.Collections.Generic.List<Character> outcastChars = new();
        Il2CppSystem.Collections.Generic.List<Character> evilChars = new();
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.GetRegisterAs().type == ECharacterType.Villager) villagerChars.Add(character);
            if (character.GetRegisterAs().type == ECharacterType.Outcast) outcastChars.Add(character);
            if (character.GetRegisterAs().type == ECharacterType.Minion) evilChars.Add(character);
            if (character.GetRegisterAs().type == ECharacterType.Demon) evilChars.Add(character);
        }

        villagerChars.Remove(charRef);
        outcastChars.Remove(charRef);
        evilChars.Remove(charRef);

        bool villagerFound = false;
        bool outcastFound = false;
        bool minionFound = false;

        Il2CppSystem.Collections.Generic.List<Character> selection = new();
        if (villagerChars.Count != 0)
        {
            selection.Add(villagerChars[UnityEngine.Random.RandomRangeInt(0, villagerChars.Count)]);
            villagerFound = true;
        }
        if (outcastChars.Count != 0)
        {
            selection.Add(outcastChars[UnityEngine.Random.RandomRangeInt(0, outcastChars.Count)]);
            outcastFound = true;
        }
        if (evilChars.Count != 0)
        {
            selection.Add(evilChars[UnityEngine.Random.RandomRangeInt(0, evilChars.Count)]);
            minionFound = true;
        }
        return new ActedInfo(ConjureInfo(charRef, selection, villagerFound, outcastFound, minionFound), selection);
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        Il2CppSystem.Collections.Generic.List<Character> goodChars = new();
        Il2CppSystem.Collections.Generic.List<Character> outcastChars = new();
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.GetRegisterAs().type == ECharacterType.Villager) goodChars.Add(character);
            if (character.GetRegisterAs().type == ECharacterType.Outcast)
            {
                goodChars.Add(character);
                outcastChars.Add(character);
            }
        }

        goodChars.Remove(charRef);

        if (goodChars.Count == 0)
        {
            return new ActedInfo("Something does not make sense");
        }

        Il2CppSystem.Collections.Generic.List<Character> selection = new();
        bool bluffNoOuts = false;
        bool bluffNoVill = false;
        int totalCount = 3;
        if (outcastChars.Count == 0)
        {
            bluffNoOuts = true;
            totalCount = 2;
        }
        if (goodChars.Count < 2)
        {
            bluffNoVill = true;
        }

        for (int i = 0; i < totalCount; i++)
        {
            if (goodChars.Count != 0) selection.Add(goodChars[UnityEngine.Random.RandomRangeInt(0, goodChars.Count)]);
            goodChars.Remove(selection[selection.Count - 1]);
        }
        return new ActedInfo(ConjureInfo(charRef, selection, !bluffNoVill, !bluffNoOuts, true), selection);
    }
    public override string Description
    {
        get
        {
            return "Bishopn't";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Weatherman initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Weatherman at #{charRef.id} acting.");
        OnActed(ETriggerPhase.Day, charRef, GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Weatherman initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Lying Weatherman at #{charRef.id} acting.");
        OnActed(ETriggerPhase.Day, charRef, GetBluffInfo(charRef));
    }
    private string ConjureInfo(Character charRef, Il2CppSystem.Collections.Generic.List<Character> selection, bool villagerFound, bool outcastFound, bool evilFound)
    {
        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        string refCharacters = sharedScripts.MentionEveryCharacterInList(selection, "");
        string info = "The forecast today!";
        if (!villagerFound) info += $"\nNo {FormatTypeName("Villager")}!";
        if (!outcastFound) info += $"\nNo {FormatTypeName("Outcast")}!";
        if (!evilFound) info += "\nNo Evil!";
        info += $"\n{refCharacters}";
        return info;
    }
    private string FormatTypeName(string type)
    {
        if (type == "Villager")
        {
            if (CheckRoleFormatting()) return "Innocent";
            else return "Villager";
        }
        if (type == "Outcast")
        {
            if (CheckRoleFormatting()) return "Meddler";
            else return "Outcast";
        }
        if (type == "Minion")
        {
            if (CheckRoleFormatting()) return "Underling";
            else return "Minion";
        }
        if (type == "Demon")
        {
            if (CheckRoleFormatting()) return "Traitor";
            else return "Demon";
        }
        return "Innocent";
    }
    public w_Dupe_Weatherman() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Weatherman>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Weatherman(System.IntPtr ptr) : base(ptr)
    {
    }
}