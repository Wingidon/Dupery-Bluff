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
public class w_Dupe_Barkeep : w_DupeZ_RoleBase
{
    public CharacterData[] allDatas = Il2CppSystem.Array.Empty<CharacterData>();
    public override string Description
    {
        get
        {
            return "Puts the Drunkard into play.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Barkeep initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Start)
        {
            wx_SavedScripts sharedScripts = new wx_SavedScripts();
            sharedScripts.DebugMessage($"Barkeep at #{charRef.id} Start-acting");
            Il2CppSystem.Collections.Generic.List<Character> drunkardTargets = Characters.Instance.FilterRealCharacterType(Gameplay.CurrentCharacters, ECharacterType.Villager);
            drunkardTargets = Characters.Instance.FilterRealAlignmentCharacters(drunkardTargets, EAlignment.Good);

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
            Il2CppSystem.Collections.Generic.List<CharacterData> allMeddlers = new();
            CharacterData drunkard = new();
            for (int i = 0; i < allDatas.Length; i++)
            {
                if (allDatas[i].type == ECharacterType.Outcast)
                {
                    allMeddlers.Add(allDatas[i]);
                }
                if (allDatas[i].characterId == "WING_Dupery_Drunkard" || allDatas[i].characterName == "Drunkard") drunkard = allDatas[i];
            }

            bool drunkardInPlay = false;
            CharacterData chosenMeddler = allMeddlers[0];
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                allMeddlers.Remove(character.dataRef);
                if (character.dataRef.characterId == "WING_Dupery_Drunkard")
                {
                    drunkardInPlay = true;
                    sharedScripts.DebugMessage($"Drunkard already in-play at #{character.id}, marking them as affected by Evil");
                    character.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                }
            }
            if (!drunkardInPlay) chosenMeddler = drunkard;
            else chosenMeddler = allMeddlers[UnityEngine.Random.RandomRangeInt(0, allMeddlers.Count)];
            Gameplay.Instance.AddScriptCharacterIfAble(ECharacterType.Outcast, chosenMeddler);
            Character target = drunkardTargets[UnityEngine.Random.RandomRangeInt(0, drunkardTargets.Count)];
            sharedScripts.DebugMessage($"Barkeep at #{charRef.id} turning #{target.id} into {chosenMeddler.characterName}");
            target.Init(chosenMeddler);
            if (!drunkardInPlay) target.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        return GrabDisguise(charRef, false);
    }
    public w_Dupe_Barkeep() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Barkeep>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Barkeep(System.IntPtr ptr) : base(ptr)
    {
    }
}