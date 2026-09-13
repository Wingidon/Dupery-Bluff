using DuperyBluff;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Il2CppSystem.Runtime.Remoting.RemotingServices;
using static InfoViewPatch;
using static MelonLoader.MelonLogger;

namespace DuperyBluff
{
    [RegisterTypeInIl2Cpp]
    public class t_Dupe_Spectre : w_DupeZ_RoleBase // Code by That Town Of Salem Player
    {
        Character alteredChar;
        bool lagPrevention = false;
        bool foundTarget = false;
        public t_Dupe_Spectre() : base(ClassInjector.DerivedConstructorPointer<t_Dupe_Spectre>())
        {
            ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        }
        public t_Dupe_Spectre(System.IntPtr ptr) : base(ptr)
        {

        }
        public override void Act(ETriggerPhase trigger, Character charRef)
        {
            if (trigger == ETriggerPhase.Start) // Cleaned up some of this a bit.
            {
                Il2CppSystem.Collections.Generic.List<Character> possibleTargets = new();
                foreach (Character character in Gameplay.CurrentCharacters) if (character.GetRegisterAs().type != ECharacterType.Outcast) possibleTargets.Add(character);
                possibleTargets = Characters.Instance.FilterCharacterMissingStatus(possibleTargets, Obscured.Obscure);
                if (possibleTargets.Count == 0) return;
                foundTarget = true;
                Character target = possibleTargets[UnityEngine.Random.RandomRangeInt(0, possibleTargets.Count)];
                new wx_SavedScripts().DebugMessage($"Spectre at #{charRef.id} obscuring the {target.dataRef.characterName} at #{target.id}");
                target.statuses.AddStatus(Obscured.Obscure, charRef);
                alteredChar = target;
                
            }
            if (trigger == ETriggerPhase.AfterRoundStart)
            {
                if (!foundTarget) return;
                testMethod(); // Great code naming, Salem.
            }
        }
        public override CharacterData GetBluffIfAble(Character charRef)
        {
            return GrabDisguise(charRef, false);
        }
        public static string obscureWords(string desc)
        {
            char[] allChars = desc.ToCharArray();
            List<string> words = new List<string>();
            foreach (char c in allChars)
            {
                words.Add(c.ToString());
            }
            List<string> nums = numbers();
            for (int i = 0; i < words.Count; i++)
            {
                if (!nums.Contains(words[i]))
                {
                    words[i] = "-";
                }
            }
            string newString = "";
            foreach (string word in words)
            {
                newString += word;
            }
            return newString;
        }
        public static List<string> numbers()
        {
            List<string> nums = new List<string>();
            nums.Add("0");
            nums.Add("1");
            nums.Add("2");
            nums.Add("3");
            nums.Add("4");
            nums.Add("5");
            nums.Add("6");
            nums.Add("7");
            nums.Add("8");
            nums.Add("9");
            nums.Add(":");
            nums.Add(" ");
            nums.Add("'");
            nums.Add(",");
            nums.Add(".");
            nums.Add("!");
            nums.Add("?");
            // nums.Add("#");
            return nums;
        }
        public void testMethod()
        {

            CharacterData resetData;
            wx_SavedScripts shared = new wx_SavedScripts();
            if (alteredChar.alignment == EAlignment.Evil)
            {
                if (alteredChar.bluff != null)
                {
                    resetData = alteredChar.bluff;
                }
                else
                {
                    resetData = alteredChar.dataRef;
                }

            }
            else
            {
                resetData = alteredChar.dataRef;
            }
            CharacterData masterCd = shared.GrabCharacterDataByID("WING_Dupery_ObscuredRole");

            if (masterCd == null || masterCd.characterId != "WING_Dupery_ObscuredRole")
            {
                MelonLogger.Error("Failed to fetch WING_Dupery_ObscuredRole data reference!");
                return;
            }

            // CRITICAL FIX: Clone the object via Unity's Instantiate so you don't corrupt the global game data pool
            CharacterData cd = masterCd.MemberwiseClone().Cast<CharacterData>();
            // W: I don't think this works, if multiple characters are obscured they all use the same number of letters in their names.

            // Copy over the roles and states cleanly to the runtime clone
            cd.role = resetData.role;
            if (resetData.picking)
            {
                cd.picking = true;
            }

            // Mutate the clone's text properties securely
            cd.name = obscureWords(resetData.name);
            // cd.startingAlignment = resetData.startingAlignment; // W: Hmm yes I will Disguise as a role that says 'Evil' when hovered over.
            // cd.type = resetData.type; // W: Hmm yes I will Disguise as a role that says 'Minion' when hovered over.
            // Re-initialize the active entity with your safe runtime clone data
            /*
            if (alteredChar.alignment == EAlignment.Evil)
            {
                alteredChar.GiveBluff(cd);
                alteredChar.RevealBluff();
                alteredChar.RefreshCharacter();
            }
            else
            {
                alteredChar.Init(cd);
            }
            */
            // W: Mm, I don't like that we're outright overriding the Good role. Let me try something different.
            alteredChar.GiveBluff(cd);
            alteredChar.RevealBluff();
            alteredChar.RefreshCharacter();
            if (!alteredChar.dataRef.usuallyDisguised) alteredChar.statuses.AddStatus(ECharacterStatus.AppearHonest, charRef); // Avoids it registering as Disguised if it shouldn't be.


        }
        public static List<string> FilteredRoles()
        {
            List<string> roles = new List<string>();
            roles.Add("0");
            return roles;
        }
    }
    public static class Obscured
    {
        public static ECharacterStatus Obscure = (ECharacterStatus)1521932118;

    }
}

