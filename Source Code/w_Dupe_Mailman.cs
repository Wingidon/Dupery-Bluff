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
public class w_Dupe_Mailman : Role
{
    public override ActedInfo GetInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<string> inPlayRoles = new();
        Il2CppSystem.Collections.Generic.List<string> outOfPlayRoles = new();
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            inPlayRoles.Add(character.GetRegisterAs().characterName);
        }
        foreach (CharacterData character in Gameplay.Instance.GetScriptCharacters())
        {
            if (!inPlayRoles.Contains(character.characterName)) outOfPlayRoles.Add(character.characterName);
        }
        while (inPlayRoles.Contains("Mailman")) inPlayRoles.Remove("Mailman");
        while (outOfPlayRoles.Contains("Mailman")) outOfPlayRoles.Remove("Mailman");
        if (outOfPlayRoles.Count == 0)
        {
            foreach (CharacterData character in Gameplay.Instance.GetAllAscensionCharacters())
            {
                if (!inPlayRoles.Contains(character.characterName)) outOfPlayRoles.Add(character.characterName);
            }
        }
        while (outOfPlayRoles.Contains("Mailman")) outOfPlayRoles.Remove("Mailman");
        string inPlay = inPlayRoles[UnityEngine.Random.RandomRangeInt(0, inPlayRoles.Count)];
        string outOfPlay = outOfPlayRoles[UnityEngine.Random.RandomRangeInt(0, outOfPlayRoles.Count)];
        return new ActedInfo(ConjureInfo(inPlay, outOfPlay));
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<string> inPlayRoles = new();
        Il2CppSystem.Collections.Generic.List<string> outOfPlayRoles = new();
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            inPlayRoles.Add(character.GetRegisterAs().characterName);
        }
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.bluff)
            {
                if (!inPlayRoles.Contains(character.bluff.characterName)) outOfPlayRoles.Add(character.bluff.characterName);
            }
        }
        while (inPlayRoles.Contains("Mailman")) inPlayRoles.Remove("Mailman");
        while (outOfPlayRoles.Contains("Mailman")) outOfPlayRoles.Remove("Mailman");
        if (outOfPlayRoles.Count == 0)
        {
            foreach (CharacterData character in Gameplay.Instance.GetScriptCharacters())
            {
                if (!inPlayRoles.Contains(character.characterName)) outOfPlayRoles.Add(character.characterName);
            }
        }
        while (outOfPlayRoles.Contains("Mailman")) outOfPlayRoles.Remove("Mailman");
        if (outOfPlayRoles.Count == 0)
        {
            foreach (CharacterData character in Gameplay.Instance.GetAllAscensionCharacters())
            {
                if (!inPlayRoles.Contains(character.characterName)) outOfPlayRoles.Add(character.characterName);
            }
        }
        while (outOfPlayRoles.Contains("Mailman")) outOfPlayRoles.Remove("Mailman");
        string inPlay = inPlayRoles[UnityEngine.Random.RandomRangeInt(0, inPlayRoles.Count)];
        string outOfPlay = outOfPlayRoles[UnityEngine.Random.RandomRangeInt(0, outOfPlayRoles.Count)];
        return new ActedInfo(ConjureInfo(outOfPlay, inPlay));
    }
    public override string Description
    {
        get
        {
            return "Learns an in-play role and a not-in-play role.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Mailman initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Mailman at #{charRef.id} acting.");
        OnActed(ETriggerPhase.Day, charRef, GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Mailman initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Mailman at #{charRef.id} bluff-acting.");
        OnActed(ETriggerPhase.Day, charRef, GetBluffInfo(charRef));
    }
    private string ConjureInfo(string inPlay, string outOfPlay)
    {
        return $"The {inPlay} lives in town, I've never heard of the {outOfPlay}";
    }
    public w_Dupe_Mailman() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Mailman>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Mailman(System.IntPtr ptr) : base(ptr)
    {
    }
}