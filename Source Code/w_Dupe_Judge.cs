using Il2Cpp;
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
public class w_Dupe_Judge : w_DupeZ_RoleBase
{
    int currentReveal = 0;
    int curGavelPercent = 30;
    public override string Description
    {
        get
        {
            return "Forces an execution every now and then.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Judge initialised at #{charRef.id}");
        }
        if (trigger == wx_SavedScripts.w_AnyRevealPatch.AnyReveal)
        {
            if (Gameplay.GameplayState == EGameplayState.Summary) return;
            if (!Characters.Instance.FilterAliveCharacters(Gameplay.CurrentCharacters).Contains(charRef)) return;
            if (charRef.statuses.Contains(JudgeStatus.w_dupe_judgeWaiting))
            {
                charRef.statuses.statuses.Remove(JudgeStatus.w_dupe_judgeWaiting);
                Health health = PlayerController.PlayerInfo.health;
                health.Damage(99999);
                /*
                foreach (Character character in Gameplay.CurrentCharacters)
                {
                    character.Reveal();
                    if (character.alignment == EAlignment.Good) character.KillByDemon(charRef);
                }
                */
                foreach (Character character in Gameplay.CurrentCharacters) if (character.alignment == EAlignment.Good) character.ExecuteAndReveal();
                //OnActed(ETriggerPhase.Day, charRef, new ActedInfo(GetJudgeNonsense()));
                charRef.ShowActed(new ActedInfo(GetJudgeNonsense()), ETriggerPhase.Day);
                return;
            }

            currentReveal++;
            if (currentReveal < CheckConfigOption_Int("Judge_EarliestDemandTime")) return;
            wx_SavedScripts sharedScripts = new();
            if (sharedScripts.PercentChance(curGavelPercent))
            {
                sharedScripts.DebugMessage("Judge demanding an Execution!");
                curGavelPercent = 30;
                charRef.statuses.AddStatus(JudgeStatus.w_dupe_judgeWaiting, charRef);
                Il2CppSystem.Collections.Generic.List<Character> curChars = new();
                foreach (Character character in Gameplay.CurrentCharacters) if (!sharedScripts.GetFaceUpClaim(character).picking) curChars.Add(character);
                Il2CppSystem.Collections.Generic.List<Character> announcers = new();
                announcers.Add(curChars[UnityEngine.Random.RandomRangeInt(0, curChars.Count)]);
                curChars.Remove(announcers[0]);
                announcers.Add(curChars[UnityEngine.Random.RandomRangeInt(0, curChars.Count)]);
                curChars.Remove(announcers[1]);
                announcers.Add(curChars[UnityEngine.Random.RandomRangeInt(0, curChars.Count)]);
                sharedScripts.DebugMessage($"Chose announcers: {sharedScripts.MentionEveryCharacterInList(announcers, "")}");
                Il2CppSystem.Collections.Generic.List<string> announcements = CourtNonsense();
                foreach (Character character in announcers)
                {
                    string announcement = announcements[UnityEngine.Random.RandomRangeInt(0, announcements.Count)];
                    character.ShowActed(new ActedInfo(announcement), ETriggerPhase.Day);
                    announcements.Remove(announcement);
                }
            }
            else
            {
                curGavelPercent += 20;
                sharedScripts.DebugMessage($"Judge still handling previous case. New chance: {curGavelPercent}%.");
            }
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        wx_SavedScripts sharedScripts = new();
        CharacterData bluff = sharedScripts.GetOverrideNotInPlayBluff(charRef, true);
        sharedScripts.DebugMessage($"Judge at #{charRef.id} chose {bluff.characterName} as bluff");
        Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);
        return bluff;
    }

    private string GetJudgeNonsense()
    {
        Il2CppSystem.Collections.Generic.List<string> possibleOutcomes = new();
        possibleOutcomes.Add("\"Judge convicts famous Executioner!\"");
        possibleOutcomes.Add("Do you have any idea how much paperwork I have to do because of your executions?");
        possibleOutcomes.Add("I find you guilty of several counts of murder");
        possibleOutcomes.Add("I find you guilty of genocide");
        possibleOutcomes.Add("You're no better than us");
        possibleOutcomes.Add("I've about had enough of you");
        possibleOutcomes.Add("Right, that's enough");
        possibleOutcomes.Add("My legal advice is to take yourself back to Ravenswood Bluff and stop meddling in affairs that don't involve you");
        return possibleOutcomes[UnityEngine.Random.RandomRangeInt(0, possibleOutcomes.Count)];
    }

    private Il2CppSystem.Collections.Generic.List<string> CourtNonsense()
    {
        Il2CppSystem.Collections.Generic.List<string> possibleOutcomes = new();
        possibleOutcomes.Add("The Judge has called court!");
        possibleOutcomes.Add("The Judge has called court!");
        possibleOutcomes.Add("All rise!");
        possibleOutcomes.Add("All rise!");
        return possibleOutcomes;
    }
    public w_Dupe_Judge() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Judge>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Judge(System.IntPtr ptr) : base(ptr)
    {
    }
    public static class JudgeStatus
    {
        public static ECharacterStatus w_dupe_judgeWaiting = (ECharacterStatus)1021475451;
        [HarmonyPatch(typeof(Character), nameof(Character.Act))]
        public static class JudgeCheckExecution
        {
            public static void Postfix(Character __instance, ETriggerPhase trigger)
            {
                if (trigger == ETriggerPhase.OnExecuted)
                {
                    foreach (Character character in Gameplay.CurrentCharacters)
                    {
                        if (character.statuses.Contains(w_dupe_judgeWaiting)) character.statuses.statuses.Remove(w_dupe_judgeWaiting);
                    }
                }
            }
        }
    }

}