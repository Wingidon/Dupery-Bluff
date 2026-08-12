using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace DuperyBluff;

[RegisterTypeInIl2Cpp]
public class w_DupeZ_RoleBase : Role
{
    int clockTimer = -1;
    public override string Description
    {
        get
        {
            return "A base for all roles to derive from.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"{charRef.dataRef.characterName} initialised at #{charRef.id}");
        }
    }
    public w_DupeZ_RoleBase() : base(ClassInjector.DerivedConstructorPointer<w_DupeZ_RoleBase>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_DupeZ_RoleBase(System.IntPtr ptr) : base(ptr)
    {
    }


    public CharacterData GrabDisguise(Character charRef, bool onlyUnique)
    {
        wx_SavedScripts sharedScripts = new();
        CharacterData bluff = new();
        if (onlyUnique) bluff = sharedScripts.GetOverrideNotInPlayBluff(charRef, true);
        else if (sharedScripts.PercentChance(50)) bluff = sharedScripts.GetOverrideNotInPlayBluff(charRef, true);
        else bluff = sharedScripts.GetOverrideDuplicateBluff(charRef);
        sharedScripts.DebugMessage($"{charRef.dataRef.characterName} at #{charRef.id} chose bluff of {bluff.characterName}");
        Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);
        return bluff;
    }


    public bool CheckRoleFormatting()
    {
        Il2CppSystem.Collections.Generic.List<string> trueResults = new();
        trueResults.Add("true");
        trueResults.Add("True");
        trueResults.Add("TRUE");
        return trueResults.Contains(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Role_TypeReference").GetValueAsString());
    }


    public void RemoveNightActors()
    {
        Il2CppSystem.Collections.Generic.List<string> whitelistOutcastIDs = new();
        whitelistOutcastIDs.Add("WING_Dupery_Copycat");
        whitelistOutcastIDs.Add("WING_Dupery_Drunkard");
        whitelistOutcastIDs.Add("WING_Dupery_Fall Guy");
        whitelistOutcastIDs.Add("WING_Dupery_Surgeon");
        whitelistOutcastIDs.Add("WING_Dupery_Wannabe");
        whitelistOutcastIDs.Add("WING_Dupery_Youngster");
        whitelistOutcastIDs.Add("Doppleganger_52694042");
        whitelistOutcastIDs.Add("Drunk_15369527");
        whitelistOutcastIDs.Add("Plague Doctor_49312486");
        whitelistOutcastIDs.Add("Rambler_13041651");
        whitelistOutcastIDs.Add("Rambler_57930131");

        Il2CppSystem.Collections.Generic.List<CharacterData> validOutcasts = new();
        foreach (CharacterData character in Gameplay.Instance.GetAscensionAllStartingCharacters())
        {
            if (character.type == ECharacterType.Outcast && whitelistOutcastIDs.Contains(character.characterId))
            {
                validOutcasts.Add(character);
            }
        }

        foreach (Character character in Gameplay.CurrentCharacters)
        {
            validOutcasts.Remove(character.dataRef);
        }

        Il2CppSystem.Collections.Generic.List<string> nightActors = new();
        nightActors.Add("Lycanthrope_16077432");


        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (nightActors.Contains(character.dataRef.characterId))
            {
                CharacterData replacement = validOutcasts[UnityEngine.Random.RandomRangeInt(0, validOutcasts.Count)];
                validOutcasts.Remove(replacement);
                character.Init(replacement);
            }
        }
    }

    public void MarkClocktower()
    {
        bool towerActive = false;
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.statuses.Contains(clocktowerMarker) || character.statuses.Contains(clocktowerGoneOff))
            {
                towerActive = true;
                return;
            }
        }
        if (!towerActive)
        {
            Character target = Gameplay.CurrentCharacters[UnityEngine.Random.RandomRangeInt(0, Gameplay.CurrentCharacters.Count)];
            new wx_SavedScripts().DebugMessage($"The Clocktower will ring at {target.id}h");
            target.statuses.AddStatus(clocktowerMarker, target);
        }
    }
    public void CheckClockTimer()
    {
        clockTimer++;
        Il2CppSystem.Collections.Generic.List<Character> curCharactersCorrectOrder = new();
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (curCharactersCorrectOrder.Count == 0) curCharactersCorrectOrder.Add(character);
            else curCharactersCorrectOrder.Insert(0, character);
        }
        if (Gameplay.CurrentCharacters.Count > clockTimer)
        {
            if (curCharactersCorrectOrder[clockTimer].statuses.Contains(clocktowerMarker))
            {
                curCharactersCorrectOrder[clockTimer].statuses.statuses.Remove(clocktowerMarker);
                curCharactersCorrectOrder[clockTimer].statuses.AddStatus(clocktowerGoneOff, Gameplay.CurrentCharacters[clockTimer]);
                RingClocktower();
            }
        }
    }
    public void RingClocktower()
    {
        if (Gameplay.GameplayState == EGameplayState.Summary) return;
        Gameplay.ChangeGameplayState(EGameplayState.Night);
        new NightModeRule(999).onNightStart?.Invoke();
        new wx_SavedScripts().DebugMessage("Clocktower ringing!");
    }
    public static ECharacterStatus clocktowerMarker = (ECharacterStatus)312153112;
    public static ECharacterStatus clocktowerGoneOff = (ECharacterStatus)312153113;
}