using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Il2CppSystem.Numerics;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;

namespace DuperyBluff;

[RegisterTypeInIl2Cpp]
public class w_Dupe_BadCop : w_DupeZ_RoleBase
{
    public CharacterData[] allDatas = Il2CppSystem.Array.Empty<CharacterData>();
    public override string Description
    {
        get
        {
            return "Disguises as and puts the Good Cop in-play.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Bad Cop initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Start)
        {
            Il2CppSystem.Collections.Generic.List<Character> goodCopTargets = Characters.Instance.FilterRealCharacterType(Gameplay.CurrentCharacters, ECharacterType.Villager);
            goodCopTargets = Characters.Instance.FilterRealAlignmentCharacters(goodCopTargets, EAlignment.Good);
            goodCopTargets = Characters.Instance.FilterCharacterMissingStatus(goodCopTargets, ECharacterStatus.MessedUpByEvil); // Avoid overwriting Shaman clones
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                if (character.dataRef.characterName == "Trickster") goodCopTargets.Remove(character);
            }

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
            if (goodCopTargets.Count != 0)
            {
                CharacterData goodCop = new CharacterData();
                for (int i = 0; i < allDatas.Length; i++)
                {
                    if (allDatas[i].characterId == "WING_Dupery_Good Cop")
                    {
                        goodCop = allDatas[i];
                        break;
                    }
                }
                Character target = goodCopTargets[UnityEngine.Random.RandomRangeInt(0, goodCopTargets.Count)];
                new wx_SavedScripts().DebugMessage($"Bad Cop initialised Good Cop at #{target.id}");
                Gameplay.Instance.AddScriptCharacterIfAble(ECharacterType.Villager, goodCop);
                target.Init(goodCop);
            }
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        CharacterData goodCop = new CharacterData();

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
        for (int i = 0; i < allDatas.Length; i++)
        {
            if (allDatas[i].characterId == "WING_Dupery_Good Cop")
            {
                goodCop = allDatas[i];
                break;
            }
        }
        charRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
        return goodCop;
    }
    public w_Dupe_BadCop() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_BadCop>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_BadCop(System.IntPtr ptr) : base(ptr)
    {
    }
}