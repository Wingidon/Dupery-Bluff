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
public class w_Dupe_Partner : Role
{
    bool haveActed = false;
    public override ActedInfo GetInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> charactersNotMe = new();
        foreach (Character character in Gameplay.CurrentCharacters) charactersNotMe.Add(character);
        charactersNotMe.Remove(charRef);
        Character chosenChar = charactersNotMe[UnityEngine.Random.RandomRangeInt(0, charactersNotMe.Count)];
        string chosenCharRole = chosenChar.GetRegisterAs().characterName;
        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        return sharedScripts.ReturnInfoWithSingleSelection(ConjureInfo(chosenChar, chosenCharRole), chosenChar);
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> charactersNotMe = new();
        foreach (Character character in Gameplay.CurrentCharacters) charactersNotMe.Add(character);
        charactersNotMe.Remove(charRef);
        Character chosenChar = charactersNotMe[UnityEngine.Random.RandomRangeInt(0, charactersNotMe.Count)];


        Il2CppSystem.Collections.Generic.List<string> inPlayRoles = new();
        Il2CppSystem.Collections.Generic.List<string> outOfPlayRoles = new();

        foreach (Character character in Gameplay.CurrentCharacters)
            inPlayRoles.Add(character.GetRegisterAs().characterName);

        foreach (CharacterData character in Gameplay.Instance.GetScriptCharacters())
            if (!inPlayRoles.Contains(character.characterName))
                outOfPlayRoles.Add(character.characterName);

        if (outOfPlayRoles.Count == 0)
            foreach (CharacterData character in Gameplay.Instance.GetAllAscensionCharacters())
                if (!inPlayRoles.Contains(character.characterName))
                    outOfPlayRoles.Add(character.characterName);


        string chosenCharRole = outOfPlayRoles[UnityEngine.Random.RandomRangeInt(0, outOfPlayRoles.Count)];
        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        return sharedScripts.ReturnInfoWithSingleSelection(ConjureInfo(chosenChar, chosenCharRole), chosenChar);
    }
    public override string Description
    {
        get
        {
            return "Starts revealed. Learns Medium info.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Partner initialised at #{charRef.id}");
        }
        if (!haveActed)
        {
            if (trigger == ETriggerPhase.AfterRoundStart)
            {
                haveActed = true;
                new wx_SavedScripts().DebugMessage($"Partner at #{charRef.id} acting AfterRoundStart, revealing");
                charRef.Reveal();
                charRef.onReveal.Invoke();
                charRef.ChangeState(ECharacterState.Alive);
                charRef.Act(ETriggerPhase.Day);
            }
        }
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Partner at #{charRef.id} acting during Day.");
        OnActed(ETriggerPhase.Day, charRef, GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Lying Partner initialised at #{charRef.id}");
        }
        if (!haveActed)
        {
            if (trigger == ETriggerPhase.AfterRoundStart)
            {
                haveActed = true;
                new wx_SavedScripts().DebugMessage($"Lying Partner at #{charRef.id} acting AfterRoundStart, revealing");
                charRef.Reveal();
                charRef.onReveal.Invoke();
                charRef.ChangeState(ECharacterState.Alive);
                charRef.Act(ETriggerPhase.Day);
            }
        }
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Lying Partner at #{charRef.id} acting during Day.");
        OnActed(ETriggerPhase.Day, charRef, GetBluffInfo(charRef));
    }
    private string ConjureInfo(Character character, string role)
    {
        return $"#{character.id} is the {role}";
    }
    public w_Dupe_Partner() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Partner>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Partner(System.IntPtr ptr) : base(ptr)
    {
    }
}