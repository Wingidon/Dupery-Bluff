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
            if (validTargets.Count != 0)
            {
                Character target = validTargets[UnityEngine.Random.RandomRangeInt(0, validTargets.Count)];
                sharedScripts.DebugMessage($"Bounty Hunter at #{charRef.id} poisoning #{target.id}");
                target.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
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
                Il2CppSystem.Collections.Generic.List<Character> nonTargets = new();
                Il2CppSystem.Collections.Generic.List<Character> myTargets = new();
                myTargets.Add(myTarget);
                foreach (Character character in Gameplay.CurrentCharacters)
                {
                    if (character != myTarget) nonTargets.Add(character);
                }
                myTargets.Add(nonTargets[UnityEngine.Random.RandomRangeInt(0, nonTargets.Count)]);
                myTargets = sharedScripts.SortList(myTargets);
                OnActed(ETriggerPhase.Day, charRef, new ActedInfo($"My target is\n#{myTargets[0].id} or #{myTargets[1].id}", myTargets));
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
            sharedScripts.DebugMessage($"Lying Bounty Hunter at #{charRef.id} acting");
            Il2CppSystem.Collections.Generic.List<Character> nonTargets = new();
            Il2CppSystem.Collections.Generic.List<Character> myTargets = new();
            if (myTarget != null)
            {
                myTargets.Add(myTarget);
                foreach (Character character in Gameplay.CurrentCharacters)
                {
                    if (character != myTarget) nonTargets.Add(character);
                }
                myTargets.Add(nonTargets[UnityEngine.Random.RandomRangeInt(0, nonTargets.Count)]);
            }
            else
            {
                myTargets.Add(nonTargets[UnityEngine.Random.RandomRangeInt(0, nonTargets.Count)]);
                nonTargets.Remove(myTargets[0]);
                myTargets.Add(nonTargets[UnityEngine.Random.RandomRangeInt(0, nonTargets.Count)]);
            }
            myTargets = sharedScripts.SortList(myTargets);
            OnActed(ETriggerPhase.Day, charRef, new ActedInfo($"My target is\n#{myTargets[0].id} or #{myTargets[1].id}", myTargets));
        }
    }
    public w_Dupe_BountyHunter() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_BountyHunter>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_BountyHunter(System.IntPtr ptr) : base(ptr)
    {
    }
}