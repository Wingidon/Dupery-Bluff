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
public class w_Dupe_Artist : w_DupeZ_RoleBase
{
    public override ActedInfo GetInfo(Character charRef)
    {
        wx_SavedScripts sharedScripts = new();
        Il2CppSystem.Collections.Generic.List<string> possibleInfo = new();
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.GetRegisterAlignment() == EAlignment.Evil)
            {
                if (CharacterHelper.CheckIfDisguisedAppearance(character))
                {
                    possibleInfo.Add(sharedScripts.GetFaceUpClaim(character).characterName);
                }
            }
        }
        string info = "blank canvas";
        if (possibleInfo.Count != 0)
        {
            info = possibleInfo[UnityEngine.Random.RandomRangeInt(0, possibleInfo.Count)];
        }
        return new ActedInfo(ConjureInfo(info));
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        wx_SavedScripts sharedScripts = new();
        Il2CppSystem.Collections.Generic.List<string> trueInfo = new();
        Il2CppSystem.Collections.Generic.List<string> falseInfo = new();
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.GetRegisterAlignment() == EAlignment.Evil)
            {
                if (CharacterHelper.CheckIfDisguisedAppearance(character))
                {
                    trueInfo.Add(sharedScripts.GetFaceUpClaim(character).characterName);
                }
            }
        }

        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (sharedScripts.GetFaceUpClaim(character).bluffable && !trueInfo.Contains(sharedScripts.GetFaceUpClaim(character).characterName))
            {
                falseInfo.Add(sharedScripts.GetFaceUpClaim(character).characterName);
            }
        }

        if (falseInfo.Count == 0)
        {
            foreach (CharacterData character in Gameplay.Instance.GetScriptCharacters())
            {
                if (character.bluffable && !trueInfo.Contains(character.characterName))
                {
                    falseInfo.Add(character.characterName);
                }
            }
        }

        if (falseInfo.Count == 0)
        {
            foreach (CharacterData character in Gameplay.Instance.GetAscensionAllStartingCharacters())
            {
                if (character.bluffable && !trueInfo.Contains(character.characterName))
                {
                    falseInfo.Add(character.characterName);
                }
            }
        }
        string info = falseInfo[UnityEngine.Random.RandomRangeInt(0, falseInfo.Count)];
        return new ActedInfo(ConjureInfo(info));
    }
    public override string Description
    {
        get
        {
            return "Learns a role Evil is pretending to be";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Artist initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Artist at #{charRef.id} acting.");
        OnActed(ETriggerPhase.Day, charRef, GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Artist initialised at #{charRef.id}");
        if (trigger != ETriggerPhase.Day) return;
        new wx_SavedScripts().DebugMessage($"Artist at #{charRef.id} bluff-acting.");
        OnActed(ETriggerPhase.Day, charRef, GetBluffInfo(charRef));
    }
    private string ConjureInfo(string role)
    {
        Il2CppSystem.Collections.Generic.List<string> vowels = new();
        vowels.Add("A");
        vowels.Add("E");
        vowels.Add("I");
        vowels.Add("O");
        vowels.Add("U");
        if (vowels.Contains(role[0].ToString())) return $"I have painted an {role}";
        return $"I have painted a {role}";
    }
    public w_Dupe_Artist() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Artist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Artist(System.IntPtr ptr) : base(ptr)
    {
    }
}