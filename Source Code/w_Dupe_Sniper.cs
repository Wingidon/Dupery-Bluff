using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace DuperyBluff;

[RegisterTypeInIl2Cpp]
public class w_Dupe_Sniper : w_DupeZ_RoleBase
{
    int remainingAmmo = 3;
    public override string Description
    {
        get
        {
            return "Stabs people every few hours.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Sniper initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            int maxAmmo = System.Int32.Parse(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Sniper_Shots").GetValueAsString());
            remainingAmmo = maxAmmo;
            MarkClocktower();
        }
        if (trigger == wx_SavedScripts.w_AnyRevealPatch.AnyReveal) CheckClockTimer();
        if (trigger == ETriggerPhase.Night)
        {
            wx_SavedScripts sharedScripts = new();
            sharedScripts.DebugMessage($"Sniper at #{charRef.id} preparing to fire...");
            if (remainingAmmo <= 0)
            {
                sharedScripts.DebugMessage($"Sniper at #{charRef.id} out of ammo!");
                return;
            }
            remainingAmmo--;
            int killDamage = System.Int32.Parse(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Sniper_Damage").GetValueAsString());
            Il2CppSystem.Collections.Generic.List<string> trueResults = new();
            trueResults.Add("true");
            trueResults.Add("True");
            trueResults.Add("TRUE");
            bool evilAllowed = trueResults.Contains(MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("Sniper_EvilAllowed").GetValueAsString());
            Il2CppSystem.Collections.Generic.List<Character> killTargets = Characters.Instance.FilterAliveCharacters(Gameplay.CurrentCharacters);
            if (!evilAllowed)
            {
                killTargets = Characters.Instance.FilterAlignmentCharacters(killTargets, EAlignment.Good);
                killTargets = Characters.Instance.FilterRealAlignmentCharacters(killTargets, EAlignment.Good);
            }
            int savedDistance = 0;
            Il2CppSystem.Collections.Generic.List<Character> scopedIn = new();
            foreach (Character character in killTargets)
            {
                if (sharedScripts.GetDistanceBetweenCharacters(charRef, character) == savedDistance) scopedIn.Add(character);
                if (sharedScripts.GetDistanceBetweenCharacters(charRef, character) > savedDistance)
                {
                    scopedIn.Clear();
                    scopedIn.Add(character);
                    savedDistance = sharedScripts.GetDistanceBetweenCharacters(charRef, character);
                }
            }
            sharedScripts.DebugMessage($"Scoped in on {sharedScripts.MentionEveryCharacterInList(scopedIn, "and")}");
            Character killshot = scopedIn[UnityEngine.Random.RandomRangeInt(0, scopedIn.Count)];
            killshot.KillByDemon(charRef);
            if (MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry<bool>("DisableRedText").Value) killshot.statuses.AddStatus(MainMod.HiddenRoleStatus.hiddenRole, charRef);
            Health health = PlayerController.PlayerInfo.health;
            health.Damage(killDamage);
            sharedScripts.DebugMessage($"Shot #{killshot.id}!");
        }
    }
    /*
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Serial Killer Bluff-Act initialised at #{charRef.id}");
        }
    }
    */
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        return GrabDisguise(charRef, false);
    }
    public w_Dupe_Sniper() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_Sniper>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_Sniper(System.IntPtr ptr) : base(ptr)
    {
    }
}