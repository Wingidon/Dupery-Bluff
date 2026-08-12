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
public class w_Dupe_Wannabe : w_DupeZ_RoleBase
{
    public override string Description
    {
        get
        {
            return "Reveals an Evil. Pretends to be Evil.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Wannabe initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Start)
        {
            wx_SavedScripts sharedScripts = new();
            sharedScripts.DebugMessage($"Wannabe at #{charRef.id} acting");
            Il2CppSystem.Collections.Generic.List<string> blacklistIDs = new();
            blacklistIDs.Add("Professional_WING");
            Il2CppSystem.Collections.Generic.List<string> whitelistIDs = new();
            whitelistIDs.Add("Wretch_80988916");
            whitelistIDs.Add("WING_Dupery_Fall Guy");
            whitelistIDs.Add("Marionette_WING");
            Il2CppSystem.Collections.Generic.List<Character> minions = new();
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                if (character.dataRef.type == ECharacterType.Minion || whitelistIDs.Contains(character.dataRef.characterId)) if (!blacklistIDs.Contains(character.dataRef.characterId)) minions.Add(character);
            }
            if (minions.Count != 0)
            {
                Character target = minions[UnityEngine.Random.RandomRangeInt(0, minions.Count)];
                sharedScripts.DebugMessage($"Wannabe at #{charRef.id} forcing #{target.id} to be face-up.");
                target.GiveBluff(target.dataRef);
                target.statuses.AddStatus(ECharacterStatus.AppearHonest, charRef);
                target.statuses.AddStatus(ECharacterStatus.AppearTruthfull, charRef);
            }
            else
            {
                sharedScripts.DebugMessage($"Wannabe at #{charRef.id} couldn't find any Minions!");

                Il2CppSystem.Collections.Generic.List<string> whitelistOutcastIDs = new(); // Other roles to transform into
                whitelistOutcastIDs.Add("WING_Dupery_Copycat");
                whitelistOutcastIDs.Add("WING_Dupery_Drunkard");
                whitelistOutcastIDs.Add("WING_Dupery_Fall Guy");
                whitelistOutcastIDs.Add("WING_Dupery_Surgeon");
                whitelistOutcastIDs.Add("WING_Dupery_Youngster");
                whitelistOutcastIDs.Add("Doppleganger_52694042");
                whitelistOutcastIDs.Add("Drunk_15369527");
                whitelistOutcastIDs.Add("Rambler_13041651");
                whitelistOutcastIDs.Add("Rambler_57930131");
                foreach (Character character in Gameplay.CurrentCharacters)
                {
                    while (whitelistOutcastIDs.Contains(character.dataRef.characterId)) whitelistOutcastIDs.Remove(character.dataRef.characterId);
                }
                Il2CppSystem.Collections.Generic.List<CharacterData> possibleOutcasts = new();
                foreach (CharacterData character in Gameplay.Instance.GetAscensionAllStartingCharacters())
                {
                    if (character.type == ECharacterType.Outcast && whitelistOutcastIDs.Contains(character.characterId))
                    {
                        possibleOutcasts.Add(character);
                    }
                }
                if (possibleOutcasts.Count == 0)
                {
                    sharedScripts.DebugMessage($"Wannabe at #{charRef.id} found no Outcasts to transform into, what the hell-");
                }
                else
                {
                    CharacterData chosenOutcast = possibleOutcasts[UnityEngine.Random.RandomRangeInt(0, possibleOutcasts.Count)];
                    sharedScripts.DebugMessage($"Wannabe at #{charRef.id} transforming into {chosenOutcast.characterName}");
                    charRef.Init(chosenOutcast);
                }


            }
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Wannabe initialised at #{charRef.id}");
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> minions = new();
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.dataRef.type == ECharacterType.Minion) minions.Add(character);
        }
        if (minions.Count == 0)
        {
            new wx_SavedScripts().DebugMessage($"Wannabe at #{charRef.id} couldn't find any Minions to bluff!");
            return null;
        }
        charRef.statuses.AddStatus(ECharacterStatus.BrokenAbility,charRef);
        charRef.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
        charRef.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
        CharacterData bluff = minions[UnityEngine.Random.RandomRangeInt(0, minions.Count)].GetRegisterAs();
        Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);
        new wx_SavedScripts().DebugMessage($"Wannabe at #{charRef.id} bluffing as {bluff.characterName}");
        return bluff;
    }
    public override bool CheckIfCanRemoveStatus(ECharacterStatus status)
    {
        if (status == ECharacterStatus.Corrupted) return false;
        return true;
    }
    public w_Dupe_Wannabe() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Wannabe>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Wannabe(System.IntPtr ptr) : base(ptr)
    {
    }
}