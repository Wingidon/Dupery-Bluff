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
public class w_Dupe_Poisoner : Role
{
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
            new wx_SavedScripts().DebugMessage($"Poisoner initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Start)
        {
            wx_SavedScripts sharedScripts = new wx_SavedScripts();
            Il2CppSystem.Collections.Generic.List<Character> myNeighbours = sharedScripts.GetCharacterNeighbours(charRef);
            myNeighbours = Characters.Instance.FilterCharacterType(myNeighbours, ECharacterType.Villager);
            myNeighbours = Characters.Instance.FilterCharactersWithoutResistance(myNeighbours, ECharacterStatus.Corrupted);
            if (myNeighbours.Count != 0)
            {
                Character target = myNeighbours[UnityEngine.Random.RandomRangeInt(0, myNeighbours.Count)];
                sharedScripts.DebugMessage($"Poisoner at #{charRef.id} poisoning #{target.id}");
                target.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
                target.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
            }
            else
            {
                sharedScripts.DebugMessage($"Poisoner at #{charRef.id} found nobody to poison!");
            }
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        wx_SavedScripts sharedScripts = new();
        CharacterData bluff = sharedScripts.GetOverrideDuplicateBluff(charRef);
        if (new wx_SavedScripts().PercentChance(50)) bluff = sharedScripts.GetOverrideNotInPlayBluff(charRef, true);
        sharedScripts.DebugMessage($"Poisoner at #{charRef.id} chose {bluff.characterName} as bluff");
        Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);
        return bluff;
    }
    public w_Dupe_Poisoner() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Poisoner>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Poisoner(System.IntPtr ptr) : base(ptr)
    {
    }
}