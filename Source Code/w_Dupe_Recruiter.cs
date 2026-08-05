using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;
using HarmonyLib;

namespace DuperyBluff;

[RegisterTypeInIl2Cpp]
public class w_Dupe_Recruiter : Role
{
    public CharacterData[] allDatas = Il2CppSystem.Array.Empty<CharacterData>();
    public override string Description
    {
        get
        {
            return "Poisonern't";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Recruiter initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Start)
        {
            if (allDatas.Length == 0)
            {
                var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
                if (loadedCharList != null)
                {
                    allDatas = new CharacterData[loadedCharList.Length];
                    for (int j = 0; j < loadedCharList.Length; j++)
                    {
                        allDatas[j] = loadedCharList[j]!.Cast<CharacterData>();
                    }
                }
            }
            wx_SavedScripts sharedScripts = new wx_SavedScripts();
            Il2CppSystem.Collections.Generic.List<string> whitelistOutcastIDs = new(); // A list of IDs of Outcasts that are unproblematic if turned Evil.
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

            Il2CppSystem.Collections.Generic.List<string> blacklistOutcastIDs = new(); // A list of IDs of Outcasts that ARE problematic if turned Evil.
            blacklistOutcastIDs.Add("Bombardier_79093372");
            blacklistOutcastIDs.Add("Lycanthrope_16077432");


            Il2CppSystem.Collections.Generic.List<string> excludeIDs = new(); // Outcasts that would register falsely to the Recruiter or would otherwise be problematic.
            excludeIDs.Add("WING_Dupery_Fall Guy");
            excludeIDs.Add("Wretch_80988916");
            excludeIDs.Add("Bombardier_79093372");
            excludeIDs.Add("Lycanthrope_16077432");

            Il2CppSystem.Collections.Generic.List<string> liars = new(); // Outcasts that should be Lying, but might not be at this point.
            liars.Add("Drunk_15369527");
            liars.Add("WING_Dupery_Drunkard");


            Il2CppSystem.Collections.Generic.List<CharacterData> validOutcasts = new();
            for (int i = 0; i < allDatas.Length; i++)
            {
                if (allDatas[i].type == ECharacterType.Outcast && whitelistOutcastIDs.Contains(allDatas[i].characterId))
                {
                    validOutcasts.Add(allDatas[i]);
                }
            }

            foreach (Character character in Gameplay.CurrentCharacters)
            {
                validOutcasts.Remove(character.dataRef);
            }
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                if (blacklistOutcastIDs.Contains(character.dataRef.characterId))
                {
                    sharedScripts.DebugMessage($"Recruiter found blacklisted Outcast {character.dataRef.characterName} at #{character.id}, replacing with other Outcast");
                    if (validOutcasts.Count != 0)
                    {
                        CharacterData chosenOutcast = validOutcasts[UnityEngine.Random.RandomRangeInt(0, validOutcasts.Count)];
                        sharedScripts.DebugMessage($"Chose {chosenOutcast.characterName} to replace target.");
                        character.Init(chosenOutcast);
                        Gameplay.Instance.AddScriptCharacterIfAble(ECharacterType.Outcast, chosenOutcast);
                        validOutcasts.Remove(chosenOutcast);

                    }
                    else
                    {
                        sharedScripts.DebugMessage("Recruiter couldn't find any Outcast roles to replace them with! What is happening?!");
                    }
                }
            }
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                if (character.dataRef.type == ECharacterType.Outcast && !excludeIDs.Contains(character.dataRef.characterId))
                {
                    bool lying = CharacterHelper.CheckLying(character);
                    character.ChangeAlignment(EAlignment.Evil);
                    if (!lying && !liars.Contains(character.dataRef.characterId)) character.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
                    sharedScripts.DebugMessage($"Recruiter at #{charRef.id} turned {character.dataRef.characterName} at #{character.id} Evil.");
                    character.statuses.AddStatus(RecruiterStatus.w_dupe_recOuts, charRef);
                    character.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                }
            }
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        wx_SavedScripts sharedScripts = new();
        CharacterData bluff = sharedScripts.GetOverrideNotInPlayBluff(charRef, true);
        sharedScripts.DebugMessage($"Recruiter at #{charRef.id} chose {bluff.characterName} as bluff");
        Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);
        return bluff;
    }
    public w_Dupe_Recruiter() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Recruiter>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Recruiter(System.IntPtr ptr) : base(ptr)
    {
    }
    public static class RecruiterStatus
    {
        public static ECharacterStatus w_dupe_recOuts = (ECharacterStatus)1853152120;

        [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
        public static class pvt
        {
            public static void Postfix(Character __instance)
            {
                if (__instance.statuses.Contains(w_dupe_recOuts))
                {
                    __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#9B4BD0><size=18>\n<Recruited></color></size>";
                }
            }
        }
    }
}