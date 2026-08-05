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
public class w_Dupe_Kingpin : Role
{
    public override string Description
    {
        get
        {
            return "Creates an Evil Villager and a Corrupted Villager nearby.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Kingpin initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Start)
        {
            new wx_SavedScripts().DebugMessage($"Kingpin at #{charRef.id} acting.");
            wx_SavedScripts sharedScripts = new wx_SavedScripts();
            Il2CppSystem.Collections.Generic.List<Character> closestVillagers = new();
            bool foundVillager = false;
            bool panic = false;
            int distance = 1;
            while (!foundVillager && !panic)
            {
                foreach (Character character in Characters.Instance.GetCharactersAtRange(distance, charRef))
                {
                    if (character.dataRef.type == ECharacterType.Villager) foundVillager = true;
                }
                if (!foundVillager) distance++;
                if (distance > 5) panic = true;
            }
            closestVillagers = Characters.Instance.FilterRealCharacterType(Characters.Instance.GetCharactersAtRange(distance, charRef), ECharacterType.Villager);
            if (closestVillagers.Count == 0)
            {
                sharedScripts.DebugMessage("Kingpin couldn't find any Villagers! What is going on!");
            }
            else
            {
                Il2CppSystem.Collections.Generic.List<Character> possibleCorruptionTargets = Characters.Instance.FilterCharactersWithoutResistance(closestVillagers, ECharacterStatus.Corrupted);
                possibleCorruptionTargets = Characters.Instance.FilterCharacterMissingStatus(possibleCorruptionTargets, ECharacterStatus.Corrupted);
                Il2CppSystem.Collections.Generic.List<Character> possibleEvilTargets = Characters.Instance.FilterRealAlignmentCharacters(closestVillagers, EAlignment.Good);
                if (possibleCorruptionTargets.Count == 0) sharedScripts.DebugMessage("Kingpin's closest Villager(s) were already Corrupted or resistant; couldn't Corrupt anyone!");
                else
                {
                    Character poisonTarget = possibleCorruptionTargets[UnityEngine.Random.RandomRangeInt(0, possibleCorruptionTargets.Count)];
                    sharedScripts.DebugMessage($"Kingpin Corrupting #{poisonTarget.id}");
                    poisonTarget.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
                    poisonTarget.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                    poisonTarget.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
                }
                if (possibleEvilTargets.Count == 0) sharedScripts.DebugMessage("Kingpin's closest Villager(s) were already Evil; couldn't turn anyone Evil!");
                else
                {
                    Character evilTarget = possibleEvilTargets[UnityEngine.Random.RandomRangeInt(0, possibleEvilTargets.Count)];
                    sharedScripts.DebugMessage($"Kingpin turning #{evilTarget.id} Evil");
                    bool lying = CharacterHelper.CheckLying(evilTarget);
                    evilTarget.ChangeAlignment(EAlignment.Evil);
                    evilTarget.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                    evilTarget.statuses.AddStatus(KingpinStatus.w_dupe_kingpinEvil, charRef);
                    if (!lying) evilTarget.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
                    if (evilTarget.dataRef.characterId == "Knight_47970624") // Evil Healthybluffing Knight is still immortal.
                    {
                        evilTarget.statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
                        if (evilTarget.statuses.Contains(ECharacterStatus.HealthyBluff))
                        {
                            evilTarget.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
                            evilTarget.statuses.AddStatus(ECharacterStatus.AppearTruthfull, charRef);
                        }
                    }
                }
            }
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        wx_SavedScripts sharedScripts = new();
        CharacterData bluff = sharedScripts.GetOverrideNotInPlayBluff(charRef, true);
        sharedScripts.DebugMessage($"Kingpin at #{charRef.id} chose {bluff.characterName} as bluff");
        Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);
        return bluff;
    }
    public w_Dupe_Kingpin() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Kingpin>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Kingpin(System.IntPtr ptr) : base(ptr)
    {
    }
    public static class KingpinStatus
    {
        public static ECharacterStatus w_dupe_kingpinEvil = (ECharacterStatus)1853146320;

        [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
        public static class pvt
        {
            public static void Postfix(Character __instance)
            {
                if (__instance.statuses.Contains(w_dupe_kingpinEvil))
                {
                    if (__instance.statuses.Contains(ECharacterStatus.Corrupted))
                    {
                        __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#9B4BD0><size=18>\n<Intoxicated></color></size>";
                    }
                    else
                    {
                        __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#9B4BD0><size=18>\n<Evil></color></size>";
                    }
                }
            }
        }
    }
}