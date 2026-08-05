using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;

namespace DuperyBluff;

[RegisterTypeInIl2Cpp]
public class w_Dupe_Romantic : Role
{
    public override ActedInfo GetInfo(Character charRef)
    {
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.dataRef.characterId == "WING_Dupery_Casanova") return new ActedInfo(ConjureInfo(charRef, charRef, true));
        }
        int myRange = System.Int32.Parse(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Romantic_Range").GetValueAsString());
        wx_SavedScripts sharedScripts = new();
        Il2CppSystem.Collections.Generic.List<Character> charactersInRange = sharedScripts.GetCharactersWithinRange(charRef, myRange);
        Il2CppSystem.Collections.Generic.List<Character> possibleTargets = sharedScripts.GetCharactersWithinRange(charRef, myRange);
        possibleTargets = Characters.Instance.FilterCharacterType(possibleTargets, ECharacterType.Villager);
        Il2CppSystem.Collections.Generic.List<Character> selection = new();
        if (possibleTargets.Count != 0)
        {
            Character lover = possibleTargets[UnityEngine.Random.RandomRangeInt(0, possibleTargets.Count)];
            selection.Add(lover);
            return new ActedInfo(ConjureInfo(charRef, lover, false), selection);
        }
        else
        {
            return new ActedInfo(ConjureInfo(charRef, charRef, false), charactersInRange);
        }
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<ECharacterType> validTypes = new();
        validTypes.Add(ECharacterType.Outcast);
        validTypes.Add(ECharacterType.Minion);
        validTypes.Add(ECharacterType.Demon);
        int myRange = System.Int32.Parse(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Romantic_Range").GetValueAsString());
        wx_SavedScripts sharedScripts = new();
        Il2CppSystem.Collections.Generic.List<Character> charactersInRange = sharedScripts.GetCharactersWithinRange(charRef, myRange);
        Il2CppSystem.Collections.Generic.List<Character> possibleTargets = new();
        foreach (Character character in charactersInRange)
        {
            if (validTypes.Contains(character.GetRegisterAs().type)) possibleTargets.Add(character);
        }
        Il2CppSystem.Collections.Generic.List<Character> selection = new();
        if (possibleTargets.Count != 0)
        {
            Character lover = possibleTargets[UnityEngine.Random.RandomRangeInt(0, possibleTargets.Count)];
            selection.Add(lover);
            return new ActedInfo(ConjureInfo(charRef, lover, false), selection);
        }
        else
        {
            return new ActedInfo(ConjureInfo(charRef, charRef, false), charactersInRange);
        }
    }
    public override string Description
    {
        get
        {
            return "Learns a nearby Villager.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Romantic initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Romantic at #{charRef.id} acting.");
        OnActed(ETriggerPhase.Day, charRef, GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Romantic initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Romantic at #{charRef.id} bluff-acting.");
        OnActed(ETriggerPhase.Day, charRef, GetBluffInfo(charRef));
    }
    private string ConjureInfo(Character charRef, Character target, bool casanova)
    {
        if (casanova) return "My lover ran away with somebody else?!";
        if (charRef == target) return "I love myself!";
        return $"I love #{target.id}";
    }
    public w_Dupe_Romantic() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Romantic>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Romantic(System.IntPtr ptr) : base(ptr)
    {
    }
}