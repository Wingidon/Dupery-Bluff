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
public class w_Dupe_Doppelganger : w_DupeZ_RoleBase
{
    Character chRef;
    private Il2CppSystem.Action action1;
    private Il2CppSystem.Action action2;
    private Il2CppSystem.Action action3;
    public override string Description
    {
        get
        {
            return "Picks 1 and Disguises as them if they're Truthful and have a Pick ability.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Doppelganger initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Day)
        {
            new wx_SavedScripts().DebugMessage($"Doppelganger at #{charRef.id} acting");
            chRef = charRef;
            CharacterPicker.Instance.StartPickCharacters(1, charRef);
            CharacterPicker.OnCharactersPicked += action1;
            CharacterPicker.OnStopPick += action2;
        }
        if (trigger == ETriggerPhase.Start)
        {
            bool foundPick = false;
            wx_SavedScripts sharedScripts = new();
            sharedScripts.DebugMessage($"Doppelganger at #{charRef.id} start-acting");
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                if (sharedScripts.GetFaceUpClaim(character).picking && character != charRef) foundPick = true;
            }
            if (!foundPick)
            {
                sharedScripts.DebugMessage($"Doppelganger at #{charRef.id} found no Pick characters! Need to create one!");
                Il2CppSystem.Collections.Generic.List<Character> possibleTargets = Characters.Instance.FilterRealCharacterType(Gameplay.CurrentCharacters, ECharacterType.Villager);
                possibleTargets = Characters.Instance.FilterRealAlignmentCharacters(possibleTargets, EAlignment.Good);
                possibleTargets = Characters.Instance.FilterCharacterMissingStatus(possibleTargets, ECharacterStatus.Corrupted);
                foreach (Character character in Gameplay.CurrentCharacters) if (Characters.Instance.startGameActOrder.Contains(character.dataRef)) possibleTargets.Remove(character);
                possibleTargets.Remove(charRef);
                if (possibleTargets.Count == 0)
                {
                    sharedScripts.DebugMessage($"Doppelganger at #{charRef.id} found no possible Villagers to replace, what the hell?");
                    return;
                }
                Character chosenTarget = possibleTargets[UnityEngine.Random.RandomRangeInt(0, possibleTargets.Count)];
                Il2CppSystem.Collections.Generic.List<CharacterData> pickCharacters = new();
                Il2CppSystem.Collections.Generic.List<string> pickIDs = new();
                pickIDs.Add("Dreamer_32014895");
                pickIDs.Add("Druid_89845092");
                pickIDs.Add("Fortune Teller_74565681");
                pickIDs.Add("Investigator_34015277");
                pickIDs.Add("Jester_41367606");
                pickIDs.Add("Judge_87202475");
                pickIDs.Add("Gambler_42592744");

                pickIDs.Add("WING_Dupery_Empath");
                pickIDs.Add("WING_Dupery_Researcher");
                pickIDs.Add("WING_Dupery_Tailor");
                pickIDs.Add("WING_Dupery_Vigilante");
                foreach (CharacterData character in Gameplay.Instance.GetAllAscensionCharacters())
                {
                    if (character.type == ECharacterType.Villager && pickIDs.Contains(character.characterId)) pickCharacters.Add(character);
                }
                foreach (Character character in Gameplay.CurrentCharacters) pickCharacters.Remove(character.dataRef);
                if (pickCharacters.Count == 0)
                {
                    sharedScripts.DebugMessage($"Doppelganger at #{charRef.id} found no possible Pick roles, what the hell?");
                    return;
                }
                CharacterData chosenPickCharacter = pickCharacters[UnityEngine.Random.RandomRangeInt(0, pickCharacters.Count)];
                chosenTarget.Init(chosenPickCharacter);
            }
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Lying Doppelganger initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.Day)
        {
            new wx_SavedScripts().DebugMessage($"Lying Doppelganger at #{charRef.id} acting");
            chRef = charRef;
            CharacterPicker.Instance.StartPickCharacters(1, charRef);
            CharacterPicker.OnCharactersPicked += action3;
            CharacterPicker.OnStopPick += action2;
        }
    }
    private void CharacterPicked()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        chars.Add(CharacterPicker.PickedCharacters[0]);
        if (!Characters.Instance.FilterRevealedCharacters(Gameplay.CurrentCharacters).Contains(chars[0])) return;
        wx_SavedScripts sharedScripts = new();
        CharacterData targetRole = sharedScripts.GetFaceUpClaim(chars[0]);
        if (!targetRole.picking) return;
        if (CharacterHelper.CheckLyingAppearance(chars[0]))
        {
            OnActed(ETriggerPhase.Day, chRef, new ActedInfo("Something does not make sense", chars));
            return;
        }
        else
        {
            chRef.GiveBluff(targetRole);
            chRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, chRef);
            if (chRef.state != ECharacterState.Dead) chRef.RevealBluff();
            chRef.pickable.SetActive(true);
            chRef.pickableUses = 1;
        }
    }
    private void CharacterPickedLiar()
    {
        CharacterPicker.OnCharactersPicked -= action3;
        CharacterPicker.OnStopPick -= action2;
        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        chars.Add(CharacterPicker.PickedCharacters[0]);
        if (!Characters.Instance.FilterRevealedCharacters(Gameplay.CurrentCharacters).Contains(chars[0])) return;
        CharacterData targetRole = new wx_SavedScripts().GetFaceUpClaim(chars[0]);
        if (!targetRole.picking) return;
        OnActed(ETriggerPhase.Day, chRef, new ActedInfo("Something does not make sense", chars));
    }
    private void StopPick()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        CharacterPicker.OnCharactersPicked -= action3;
    }
    public w_Dupe_Doppelganger() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Doppelganger>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
    public w_Dupe_Doppelganger(System.IntPtr ptr) : base(ptr)
    {
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
    public override bool CheckIfCanBeKilled(Character charRef)
    {
        if (charRef.bluff)
        {
            if (charRef.bluff != charRef.dataRef) return charRef.bluff.role.CheckIfCanBeKilled(charRef);
        }
        return true;
    }
}