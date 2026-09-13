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
public class w_Dupe_Partner : w_DupeZ_RoleBase
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
        int lieTier = CheckConfigOption_Int("Partner_LyingTier");
        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        if (lieTier == 3) // Backs up a Disguised character's claim or accuses someone of being a Disguised character.
        {
            Il2CppSystem.Collections.Generic.List<Character> disguisedChars = new();
            Il2CppSystem.Collections.Generic.List<Character> validTargets = new();
            foreach (Character character in Gameplay.CurrentCharacters)
                if (CharacterHelper.CheckIfDisguisedAppearance(character))
                    if (character.bluff)
                        if (character.bluff.characterName != character.GetRegisterAs().characterName)
                            disguisedChars.Add(character);
            foreach (Character character in Gameplay.CurrentCharacters) validTargets.Add(character);
            disguisedChars.Remove(charRef);
            validTargets.Remove(charRef);

            foreach (Character character in disguisedChars) validTargets.Add(character); // Twice as likely to back up a character's claim.

            Il2CppSystem.Collections.Generic.List<string> possibleRoles = new();
            foreach (CharacterData character in Gameplay.Instance.GetScriptCharacters())
            {
                if (character.usuallyDisguised) possibleRoles.Add(character.characterName);
            }

            Character chosenChar = validTargets[UnityEngine.Random.RandomRangeInt(0, validTargets.Count)];
            while (possibleRoles.Contains(chosenChar.GetRegisterAs().characterName)) possibleRoles.Remove(chosenChar.GetRegisterAs().characterName);
            if (possibleRoles.Count == 0) // If there's no Disguised roles in the Deck, downgrade to the next tier. This is hilariously rare.
            {
                lieTier = 2;
            }
            else
            {
                string chosenCharRole = "- Pardon me, I've made a mistake";
                if (disguisedChars.Contains(chosenChar)) chosenCharRole = chosenChar.bluff.characterName;
                else
                {
                    chosenCharRole = possibleRoles[UnityEngine.Random.RandomRangeInt(0, possibleRoles.Count)];
                }
                return sharedScripts.ReturnInfoWithSingleSelection(ConjureInfo(chosenChar, chosenCharRole), chosenChar);
            }

        }
        if (lieTier == 2) // Learns a Disguised character and tries to back up their Disguise
        {
            Il2CppSystem.Collections.Generic.List<Character> disguisedChars = new();
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                if (CharacterHelper.CheckIfDisguisedAppearance(character))
                {
                    if (character.bluff)
                    {
                        if (character.bluff.characterName != character.GetRegisterAs().characterName)
                        {
                            disguisedChars.Add(character);
                        }
                    }
                }
            }
            disguisedChars.Remove(charRef);
            if (disguisedChars.Count == 0) lieTier = 1; // If there are no Disguised characters (somehow), downgrade to the next tier.
            else
            {
                Character chosenChar = disguisedChars[UnityEngine.Random.RandomRangeInt(0, disguisedChars.Count)];
                string chosenCharRole = chosenChar.bluff.characterName;
                return sharedScripts.ReturnInfoWithSingleSelection(ConjureInfo(chosenChar, chosenCharRole), chosenChar);
            }
        }
        if (lieTier == 1) // Learns a random character and a random role that they aren't.
        {
            Il2CppSystem.Collections.Generic.List<Character> charactersNotMe = new();
            foreach (Character character in Gameplay.CurrentCharacters) charactersNotMe.Add(character);
            charactersNotMe.Remove(charRef);
            Character chosenChar = charactersNotMe[UnityEngine.Random.RandomRangeInt(0, charactersNotMe.Count)];


            Il2CppSystem.Collections.Generic.List<string> deckRoles = new();
            foreach (CharacterData character in Gameplay.Instance.GetScriptCharacters())
            {
                if (character.characterName != chosenChar.GetRegisterAs().characterName) deckRoles.Add(character.characterName);
            }
            if (deckRoles.Count == 0) lieTier = 0; // If there's no false roles in the Deck (somehow), downgrade to the next tier.
            else
            {
                string chosenCharRole = deckRoles[UnityEngine.Random.RandomRangeInt(0, deckRoles.Count)];
                return sharedScripts.ReturnInfoWithSingleSelection(ConjureInfo(chosenChar, chosenCharRole), chosenChar);
            }
        }
        if (true) // Learn a random character and a random not-in-play role. Would usually trigger if lieTier = 0, but I need a fallback
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
            return sharedScripts.ReturnInfoWithSingleSelection(ConjureInfo(chosenChar, chosenCharRole), chosenChar);
        }
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