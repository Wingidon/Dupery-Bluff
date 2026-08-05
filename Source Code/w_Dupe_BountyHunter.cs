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
public class w_Dupe_BountyHunter : Role
{
    Character myTarget = new();
    public override string Description
    {
        get
        {
            return "Plague Doctorn't";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Bounty Hunter initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Start)
        {
            wx_SavedScripts sharedScripts = new wx_SavedScripts();
            Il2CppSystem.Collections.Generic.List<Character> validTargets = Characters.Instance.FilterCharacterType(Gameplay.CurrentCharacters, ECharacterType.Villager);
            validTargets = Characters.Instance.FilterCharactersWithoutResistance(validTargets, ECharacterStatus.Corrupted);
            validTargets = Characters.Instance.FilterCharacterMissingStatus(validTargets, ECharacterStatus.Corrupted);
            validTargets.Remove(charRef);
            if (validTargets.Count != 0)
            {
                Character target = validTargets[UnityEngine.Random.RandomRangeInt(0, validTargets.Count)];
                sharedScripts.DebugMessage($"Bounty Hunter at #{charRef.id} poisoning #{target.id}");
                target.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
                myTarget = target;
            }
            else
            {
                sharedScripts.DebugMessage($"Bounty Hunter at #{charRef.id} found nobody to poison!");
            }
        }
        if (trigger == ETriggerPhase.Day)
        {
            wx_SavedScripts sharedScripts = new wx_SavedScripts();
            sharedScripts.DebugMessage($"Bounty Hunter at #{charRef.id} acting");
            if (myTarget != null)
            {
                ActedInfo myInfo = GetBHInfo(charRef, myTarget);
                OnActed(ETriggerPhase.Day, charRef, myInfo);
            }
            else
            {
                OnActed(ETriggerPhase.Day, charRef, new ActedInfo("I arrived late\n\nI have no targets"));
            }
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Lying Bounty Hunter initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Day)
        {
            wx_SavedScripts sharedScripts = new wx_SavedScripts();
            sharedScripts.DebugMessage($"Bounty Hunter at #{charRef.id} bluff-acting");
            ActedInfo myInfo = GetBHInfo(charRef, charRef);
            OnActed(ETriggerPhase.Day, charRef, myInfo);
        }
    }
    public ActedInfo GetBHInfo(Character charRef, Character target)
    {
        Il2CppSystem.Collections.Generic.List<Character> allOtherChars = new();
        Il2CppSystem.Collections.Generic.List<Character> selection = new();
        foreach (Character character in Gameplay.CurrentCharacters) allOtherChars.Add(character);
        allOtherChars.Remove(charRef);
        if (charRef == target)
        {
            selection.Add(allOtherChars[UnityEngine.Random.RandomRangeInt(0, allOtherChars.Count)]);
            allOtherChars.Remove(selection[0]);
        }
        else
        {
            selection.Add(target);
            allOtherChars.Remove(target);
        }
        selection.Add(allOtherChars[UnityEngine.Random.RandomRangeInt(0, allOtherChars.Count)]);
        selection = new wx_SavedScripts().SortList(selection);
        string info = $"My target is\n#{selection[0].id} or #{selection[1].id}";
        return new ActedInfo(info, selection);
    }
    public w_Dupe_BountyHunter() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_BountyHunter>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_BountyHunter(System.IntPtr ptr) : base(ptr)
    {
    }
}