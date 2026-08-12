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
public class w_Dupe_Scoundrel : w_DupeZ_RoleBase
{
    public override string Description
    {
        get
        {
            return "Undyingn't";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Scoundrel initialised at #{charRef.id}");
        }
    }
    public override bool CheckIfCanBeKilled(Character charRef)
    {
        if (charRef.statuses.Contains(ECharacterStatus.Corrupted)) return true;
        int penalty = System.Int32.Parse(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Scoundrel_FailPenalty").GetValueAsString());
        bool evilLives = false;
        Il2CppSystem.Collections.Generic.List<Character> aliveChars = Characters.Instance.FilterAliveCharacters(Gameplay.CurrentCharacters);
        aliveChars.Remove(charRef);
        Il2CppSystem.Collections.Generic.List<string> lastStandIDs = new();
        lastStandIDs.Add("Vizier_LRZH");
        lastStandIDs.Add("Undying_WING");
        lastStandIDs.Add("Squire_scm");
        lastStandIDs.Add("WING_Dupery_Scoundrel");
        foreach (Character character in aliveChars)
        {
            if (character.alignment == EAlignment.Evil && !lastStandIDs.Contains(character.dataRef.characterId)) evilLives = true;
        }
        if (evilLives) PlayerController.PlayerInfo.health.Damage(penalty);
        return !evilLives;
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        charRef.statuses.AddStatus(ECharacterStatus.AppearTruthfull, charRef);
        return null;
    }
    public w_Dupe_Scoundrel() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Scoundrel>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Scoundrel(System.IntPtr ptr) : base(ptr)
    {
    }
}