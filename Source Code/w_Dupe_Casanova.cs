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
public class w_Dupe_Casanova : w_DupeZ_RoleBase
{
    public override string Description
    {
        get
        {
            return "Charms the Romantic's lover.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Casanova initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Start)
        {
            wx_SavedScripts sharedScripts = new wx_SavedScripts();
            sharedScripts.DebugMessage($"Casanova at #{charRef.id} Start-acting");
            Il2CppSystem.Collections.Generic.List<Character> romanticTargets = Characters.Instance.FilterRealCharacterType(Gameplay.CurrentCharacters, ECharacterType.Villager);
            romanticTargets = Characters.Instance.FilterRealAlignmentCharacters(romanticTargets, EAlignment.Good);
            CharacterData romantic = new();
            foreach (CharacterData character in Gameplay.Instance.GetAscensionAllStartingCharacters())
            {
                if (character.characterId == "WING_Dupery_Romantic")
                {
                    romantic = character;
                    break;
                }
            }

            bool romanticInPlay = false;
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                if (character.dataRef.characterId == "WING_Dupery_Romantic")
                {
                    romanticInPlay = true;
                    sharedScripts.DebugMessage($"Romantic already in-play at #{character.id}, marking them as affected by Evil");
                    character.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                }
            }
            if (!romanticInPlay)
            {
                Character target = romanticTargets[UnityEngine.Random.RandomRangeInt(0, romanticTargets.Count)];
                sharedScripts.DebugMessage($"Found no in-play Romantic, turning #{target.id} into Romantic");
                if (romantic != null)
                {
                    target.Init(romantic);
                    target.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                    Gameplay.Instance.AddScriptCharacterIfAble(romantic.type, romantic);
                }
                else
                {
                    sharedScripts.DebugMessage($"Couldn't find the Romantic's file! What is happening?!");
                }
            }
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        return GrabDisguise(charRef, false);
    }
    public w_Dupe_Casanova() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Casanova>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Casanova(System.IntPtr ptr) : base(ptr)
    {
    }
}