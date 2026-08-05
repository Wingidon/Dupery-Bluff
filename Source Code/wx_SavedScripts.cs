using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppRewired.Glyphs;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using UnityEngine;
using static MelonLoader.MelonLaunchOptions;

namespace DuperyBluff
{
    // Token: 0x02000007 RID: 7
    [RegisterTypeInIl2Cpp]
    public class wx_SavedScripts : Role
    {
        // Token: 0x06000019 RID: 25 RVA: 0x0000359C File Offset: 0x0000179C
        public wx_SavedScripts() : base(ClassInjector.DerivedConstructorPointer<wx_SavedScripts>())
        {
            ClassInjector.DerivedConstructorBody(this);
        }

        // Token: 0x0600001A RID: 26 RVA: 0x000035B2 File Offset: 0x000017B2
        public wx_SavedScripts(IntPtr ptr) : base(ptr)
        {
        }

    




        // This is where I store my miscellaneous scripts and some various things that aren't tied to any particular character.

    public Il2CppSystem.Collections.Generic.List<Character> SortList(Il2CppSystem.Collections.Generic.List<Character> list)
        {
            Il2CppSystem.Collections.Generic.List<Character> newList = new Il2CppSystem.Collections.Generic.List<Character>();
            if (list.Count == 0) return newList;
            for (int i = 0; i < Gameplay.CurrentCharacters.Count + 3; i++)
            {
                foreach (Character character in list)
                {
                    if (character.id == i) newList.Add(character);
                }
            }
            return newList;
        }
        public Il2CppSystem.Collections.Generic.List<ECharacterType> SortList(Il2CppSystem.Collections.Generic.List<ECharacterType> list)
        {
            Il2CppSystem.Collections.Generic.List<ECharacterType> newList = new Il2CppSystem.Collections.Generic.List<ECharacterType>();
            if (list.Count == 0) return newList;
            foreach (ECharacterType characterType in list)
            {
                if (characterType == ECharacterType.Villager)
                {
                    newList.Add(characterType);
                }
            }
            foreach (ECharacterType characterType in list)
            {
                if (characterType == ECharacterType.Outcast)
                {
                    newList.Add(characterType);
                }
            }
            foreach (ECharacterType characterType in list)
            {
                if (characterType == ECharacterType.Minion)
                {
                    newList.Add(characterType);
                }
            }
            foreach (ECharacterType characterType in list)
            {
                if (characterType == ECharacterType.Demon)
                {
                    newList.Add(characterType);
                }
            }
            return newList;
        }
        public Il2CppSystem.Collections.Generic.List<Character> GetFakeEvilTeam()
        {
            Il2CppSystem.Collections.Generic.List<Character> possibleTargets = new Il2CppSystem.Collections.Generic.List<Character>();
            Il2CppSystem.Collections.Generic.List<Character> newList = new Il2CppSystem.Collections.Generic.List<Character>();
            Character target = new Character();
            int totalEvils = 0;
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                possibleTargets.Add(character);
                if (character.GetRegisterAlignment() == EAlignment.Evil)
                {
                    totalEvils++;
                }
            }
            for (int i = 0; i < totalEvils; i++)
            {
                target = possibleTargets[UnityEngine.Random.RandomRangeInt(0, possibleTargets.Count)];
                newList.Add(target);
                possibleTargets.Remove(target);
            }
            return newList;
        }
        public Il2CppSystem.Collections.Generic.List<Character> GetFakeGroup(Il2CppSystem.Collections.Generic.List<Character> targets)
        {
            // MelonLogger.Msg("Getting fake group");
            Il2CppSystem.Collections.Generic.List<Character> possibleTargets = new Il2CppSystem.Collections.Generic.List<Character>();
            Il2CppSystem.Collections.Generic.List<Character> newList = new Il2CppSystem.Collections.Generic.List<Character>();
            Character target = new Character();
            int totalTargets = targets.Count;
            // MelonLogger.Msg($"Fake group has {totalTargets} characters in it");
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                possibleTargets.Add(character);
            }
            for (int i = 0; i < totalTargets; i++)
            {
                target = possibleTargets[UnityEngine.Random.RandomRangeInt(0, possibleTargets.Count)];
                // MelonLogger.Msg($"Added #{target.id} to fake group");
                newList.Add(target);
                possibleTargets.Remove(target);
            }
            MelonLogger.Msg($"Fake group is {MentionEveryCharacterInList(newList, "and")}");
            return newList;
        }

        public int GetDistanceBetweenCharacters(Character char1, Character char2)
        {
            int totalCharCount = Gameplay.CurrentCharacters.Count;
            int tempDist = char1.id - char2.id;
            if (tempDist < 0)
            {
                tempDist = tempDist + totalCharCount;
            }
            int tempDist2 = char2.id - char1.id;
            if (tempDist2 < 0)
            {
                tempDist2 = tempDist2 + totalCharCount;
            }
            if (tempDist > tempDist2)
            {
                return tempDist2;
            }
            return tempDist;
        }

        public int GetClosestDistance(Il2CppSystem.Collections.Generic.List<Character> targets, Character anchor)
        {
            int dist = 10000;
            int calcDist = 0;
            foreach (Character character in targets)
            {
                calcDist = GetDistanceBetweenCharacters(character, anchor);
                if (calcDist != 0 && dist > calcDist)
                {
                    dist = calcDist;
                }
            }
            if (dist == 10000) dist = 1;
            return dist;
        }

        public int GetFurthestDistance(Il2CppSystem.Collections.Generic.List<Character> targets, Character anchor)
        {
            int dist = 0;
            int calcDist = 0;
            foreach (Character character in targets)
            {
                calcDist = GetDistanceBetweenCharacters(character, anchor);
                if (calcDist != 0 && dist < calcDist)
                {
                    dist = calcDist;
                }
            }
            return dist;
        }


        public float GetTrustworthiness(Character target)
        {
            float trust = 1f;
            if (target.GetRegisterAs().type == ECharacterType.Villager) trust *= 5;
            if (target.GetRegisterAs().type == ECharacterType.Outcast) trust *= 3;
            if (target.GetRegisterAs().type == ECharacterType.Minion) trust *= 3;
            if (target.GetRegisterAlignment() == EAlignment.Good) trust *= 3;
            if (!CharacterHelper.CheckLyingAppearance(target)) trust *= 3;
            if (!CharacterHelper.CheckIfDisguisedAppearance(target)) trust *= 2.5f;
            return trust;
        }

        public string MentionEveryCharacterInList(Il2CppSystem.Collections.Generic.List<Character> characters, string andOr)
        {
            string returnString = "Return";
            int characterCount = characters.Count;
            int counter = 0;
            Il2CppSystem.Collections.Generic.List<Character> sortedCharacters = SortList(characters);
            foreach (Character character in sortedCharacters)
            {
                if (returnString == "Return")
                {
                    counter++;
                    returnString = $"#{character.id}";
                }
                else
                {
                    counter++;
                    if (counter == characterCount)
                    {
                        if (andOr == "And" || andOr == "and")
                        {
                            returnString = $"{returnString} and #{character.id}";
                        }
                        else if (andOr == "Or" || andOr == "or")
                        {
                            returnString = $"{returnString} or #{character.id}";
                        }
                        else
                        {
                            returnString = $"{returnString}, #{character.id}";
                        }
                    }
                    else
                    {
                        returnString = $"{returnString}, #{character.id}";
                    }
                }
            }
            return returnString;
        }


        public string MentionEveryRoleInList(Il2CppSystem.Collections.Generic.List<CharacterData> characters, string andOr)
        {
            string returnString = "Return";
            int characterCount = characters.Count;
            int counter = 0;
            Il2CppSystem.Collections.Generic.List<CharacterData> sortedCharacters = characters;
            foreach (CharacterData character in sortedCharacters)
            {
                if (returnString == "Return")
                {
                    counter++;
                    returnString = $"{character.characterName}";
                }
                else
                {
                    counter++;
                    if (counter == characterCount)
                    {
                        if (andOr == "And" || andOr == "and")
                        {
                            returnString = $"{returnString} and {character.characterName}";
                        }
                        else if (andOr == "Or" || andOr == "or")
                        {
                            returnString = $"{returnString} or {character.characterName}";
                        }
                        else
                        {
                            returnString = $"{returnString}, {character.characterName}";
                        }
                    }
                    else
                    {
                        returnString = $"{returnString}, {character.characterName}";
                    }
                }
            }
            return returnString;
        }

        public string MentionEveryStringInList(Il2CppSystem.Collections.Generic.List<string> characters, string andOr)
        {
            string returnString = "Return";
            int characterCount = characters.Count;
            int counter = 0;
            Il2CppSystem.Collections.Generic.List<string> sortedCharacters = characters;
            foreach (string character in sortedCharacters)
            {
                if (returnString == "Return")
                {
                    counter++;
                    returnString = $"{character}";
                }
                else
                {
                    counter++;
                    if (counter == characterCount)
                    {
                        if (andOr == "And" || andOr == "and")
                        {
                            returnString = $"{returnString} and {character}";
                        }
                        else if (andOr == "Or" || andOr == "or")
                        {
                            returnString = $"{returnString} or {character}";
                        }
                        else
                        {
                            returnString = $"{returnString}, {character}";
                        }
                    }
                    else
                    {
                        returnString = $"{returnString}, {character}";
                    }
                }
            }
            return returnString;
        }

        public string MentionEveryCharacterInUnsortedList(Il2CppSystem.Collections.Generic.List<Character> characters, string andOr)
        {
            string returnString = "Return";
            int characterCount = characters.Count;
            int counter = 0;
            Il2CppSystem.Collections.Generic.List<Character> sortedCharacters = characters;
            foreach (Character character in sortedCharacters)
            {
                if (returnString == "Return")
                {
                    counter++;
                    returnString = $"#{character.id}";
                }
                else
                {
                    counter++;
                    if (counter == characterCount)
                    {
                        if (andOr == "And" || andOr == "and")
                        {
                            returnString = $"{returnString} and #{character.id}";
                        }
                        else if (andOr == "Or" || andOr == "or")
                        {
                            returnString = $"{returnString} or #{character.id}";
                        }
                        else if (andOr == "Then" || andOr == "then")
                        {
                            returnString = $"{returnString}, then #{character.id}";
                        }
                        else
                        {
                            returnString = $"{returnString}, #{character.id}";
                        }
                    }
                    else if (andOr == "Then" || andOr == "then")
                    {
                        returnString = $"{returnString}, then #{character.id}";
                    }
                }
            }
            return returnString;
        }

        public string MentionEveryTypeInList(Il2CppSystem.Collections.Generic.List<ECharacterType> types, string andOr)
        {
            string returnString = "Return";
            int characterCount = types.Count;
            int counter = 0;
            Il2CppSystem.Collections.Generic.List<ECharacterType> sortedTypes = SortList(types);
            foreach (ECharacterType type in sortedTypes)
            {
                if (returnString == "Return")
                {
                    counter++;
                    returnString = type.ToString();
                }
                else
                {
                    counter++;
                    if (counter == characterCount)
                    {
                        if (andOr == "And" || andOr == "and")
                        {
                            returnString = $"{returnString} and {type.ToString()}";
                        }
                        else if (andOr == "Or" || andOr == "or")
                        {
                            returnString = $"{returnString} or {type.ToString()}";
                        }
                        else
                        {
                            returnString = $"{returnString}, {type.ToString()}";
                        }
                    }
                    else
                    {
                        returnString = $"{returnString}, {type.ToString()}";
                    }
                }
            }
            return returnString;
        }

        public bool CheckIfNeighbour(Character character1, Character character2)
        {
            if (GetDistanceBetweenCharacters(character1, character2) == 1) return true;
            return false;
        }
        public bool PercentChance(float percentage)
        {
            if (UnityEngine.Random.RandomRange(0, 100) <= percentage) return true;
            return false;
        }

        public Il2CppSystem.Collections.Generic.List<Character> GetCharacterNeighbours(Character targetChar)
        {
            Il2CppSystem.Collections.Generic.List<Character> outputList = new Il2CppSystem.Collections.Generic.List<Character>();
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                if (CheckIfNeighbour(targetChar, character)) outputList.Add(character);
            }
            return outputList;
        }

        public Il2CppSystem.Collections.Generic.List<Character> GetCharactersWithinRange(Character charRef, int range)
        {
            Il2CppSystem.Collections.Generic.List<Character> outputList = new Il2CppSystem.Collections.Generic.List<Character>();
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                if (GetDistanceBetweenCharacters(charRef, character) <= range && character != charRef)
                {
                    outputList.Add(character);
                }
            }
            return outputList;
        }



        public Il2CppSystem.Collections.Generic.List<string> AddStringToList(string input, Il2CppSystem.Collections.Generic.List<string> list, int weight)
        {
            for (int i = 0; i < weight; i++)
            {
                list.Add(input);
            }
            return list;
        }


        public string CheckIfThe(string Name)
        {
            Il2CppSystem.Collections.Generic.List<string> pluralNamesV = new Il2CppSystem.Collections.Generic.List<string>(); // There may be many of this character, they start with a vowel. #X is an Y.
            Il2CppSystem.Collections.Generic.List<string> pluralNamesC = new Il2CppSystem.Collections.Generic.List<string>(); // There may be many of this character, they start with a consonant. #X is a Y.
            Il2CppSystem.Collections.Generic.List<string> demonNames = new Il2CppSystem.Collections.Generic.List<string>(); // This character has an actual name. #X is Y.

            // Villagers
            pluralNamesC.Add("Citizen");

            // Outcasts
            pluralNamesC.Add("Pariah");
            pluralNamesC.Add("Trickster"); // From Skill Cycler's Riddles mod.

            // Minions
            demonNames.Add("Swarm");
            pluralNamesV.Add("Acolyte");
            pluralNamesV.Add("Underling");
            pluralNamesC.Add("Fanatic");
            pluralNamesC.Add("Mastermind"); // From Skill Cycler's Riddles mod
            pluralNamesC.Add("Zealot");

            // Vanilla
            demonNames.Add("Baa");
            demonNames.Add("Lilis");
            demonNames.Add("Pooka");

            // Wingidon's Expansion Pack
            demonNames.Add("Agmeres");
            demonNames.Add("Caedoccidere");
            demonNames.Add("Carnicarius");
            demonNames.Add("Emenverax");
            demonNames.Add("Iris");
            demonNames.Add("Leviathan");
            demonNames.Add("Magnere");
            demonNames.Add("Mendaverte");
            demonNames.Add("Praesect");
            demonNames.Add("Sanguitaurus");
            demonNames.Add("Specularus");
            demonNames.Add("Tenecaligo");
            demonNames.Add("Venelum");
            demonNames.Add("Veniyon");
            demonNames.Add("Vidiyon");
            demonNames.Add("Viciyon");

            // Role Ideas Collection
            demonNames.Add("Death");

            // ExtraRandomized
            demonNames.Add("Better Baa");

            // CarlzVilliagePack
            pluralNamesC.Add("Hydra");
            demonNames.Add("Pestilence");

            // CSK's Expansion Pack
            demonNames.Add("Belias");

            // LRZH's Circus
            demonNames.Add("Dominion");
            demonNames.Add("Mahr");
            demonNames.Add("Po");

            // Power Play
            demonNames.Add("Snowed In");
            demonNames.Add("Death");
            demonNames.Add("Famine");
            demonNames.Add("Pestilence");
            demonNames.Add("War");

            if (pluralNamesV.Contains(Name))
            {
                return "an ";
            }
            if (pluralNamesC.Contains(Name))
            {
                return "a ";
            }
            if (demonNames.Contains(Name))
            {
                return "";
            }
            return "the ";
        }



        public bool CheckRoleValidBluff(string roleID)
        {
            if (roleID == "WING_Dupery_Doppelganger")
            {
                foreach (Character character in Gameplay.CurrentCharacters)
                {
                    if (GetFaceUpClaim(character).picking && GetFaceUpClaim(character).characterId != "WING_Dupery_Doppelganger") return true;
                }
                return false;
            }
            return true;
        }


        public CharacterData GetOverrideNotInPlayBluff(Character charRef, bool outcastsAllowed)
        {
            CharacterData bluff = Characters.Instance.GetRandomUniqueBluff();
            Il2CppSystem.Collections.Generic.List<string> blacklistBluffs = new Il2CppSystem.Collections.Generic.List<string>();
            blacklistBluffs.Add("Bounty Hunter_39284184");
            if (charRef.dataRef.characterId == "Iris_WING") blacklistBluffs.Add("Baker_22847064");
            Il2CppSystem.Collections.Generic.List<CharacterData> possibleBluffs = Characters.Instance.FilterAlignmentCharacters(Gameplay.Instance.GetAllAscensionCharacters(), EAlignment.Good);
            Il2CppSystem.Collections.Generic.List<CharacterData> removeBluffs = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            possibleBluffs = Characters.Instance.FilterBluffableCharacters(possibleBluffs);
            foreach (CharacterData character in possibleBluffs)
            {
                if (blacklistBluffs.Contains(character.characterId))
                {
                    removeBluffs.Add(character);
                }
                if (!outcastsAllowed && character.type == ECharacterType.Outcast)
                {
                    removeBluffs.Add(character);
                }
                if (!CheckRoleValidBluff(character.characterId)) removeBluffs.Add(character);
            }
            foreach (CharacterData character in removeBluffs)
            {
                possibleBluffs.Remove(character);
            }
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                while (possibleBluffs.Contains(character.dataRef))
                {
                    possibleBluffs.Remove(character.dataRef);
                }
            }
            if (possibleBluffs.Count != 0)
            {
                bluff = possibleBluffs[UnityEngine.Random.RandomRangeInt(0, possibleBluffs.Count)];
            }
            return bluff;
        }

        public static int RoundValToInt(decimal val)
        {
            return (int)Math.Round(val);
        }

        public CharacterData GetOverrideDuplicateBluff(Character charRef)
        {
            CharacterData bluff = Characters.Instance.GetRandomDuplicateBluff();
            Il2CppSystem.Collections.Generic.List<CharacterData> possibleBluffs = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                if (character.dataRef.bluffable == true && character.dataRef.startingAlignment == EAlignment.Good)
                {
                    possibleBluffs.Add(character.dataRef);
                }
            }
            if (possibleBluffs.Count != 0)
            {
                bluff = possibleBluffs[UnityEngine.Random.RandomRangeInt(0, possibleBluffs.Count)];
            }
            return bluff;
        }

        public void TurnEvilIfPossible(Character character)
        {

            if (CheckIfAlwaysGood(character)) character.ChangeAlignment(EAlignment.Evil);
        }

        public bool CheckIfAlwaysGood(Character character)
        {
            Il2CppSystem.Collections.Generic.List<string> alwaysGoodIDs = new Il2CppSystem.Collections.Generic.List<string>();
            alwaysGoodIDs.Add("Saint_61372493");
            alwaysGoodIDs.Add("Politician_WING");
            alwaysGoodIDs.Add("Saint_WING");

            if (alwaysGoodIDs.Contains(character.dataRef.characterId)) return true;
            return false;
        }


        public int GetPairsOfCharactersInList(Il2CppSystem.Collections.Generic.List<Character> list)
        {
            //MelonLogger.Msg("Getting pairs");
            Il2CppSystem.Collections.Generic.List<Character> allCharactersPlusOne = new Il2CppSystem.Collections.Generic.List<Character>();
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                allCharactersPlusOne.Add(character);
            }
            allCharactersPlusOne.Add(Gameplay.CurrentCharacters[0]);

            bool prevCount = false;
            int pairs = 0;
            foreach (Character character in allCharactersPlusOne)
            {
                //MelonLogger.Msg($"Checking #{character.id}");
                if (list.Contains(character) && prevCount == true)
                {
                    pairs++;
                    //MelonLogger.Msg($"Found a pair, there are now {pairs} pair(s)");
                }
                if (list.Contains(character))
                {
                    prevCount = true;
                    //MelonLogger.Msg($"#{character.id} is in the list, so they're ready to be part of the next pair");
                }
                else
                {
                    prevCount = false;
                    //MelonLogger.Msg($"#{character.id} is not in the list, so they will not be part of the next pair");
                }
                
            }
            return pairs;
        }


        public void DebugMessage(string message)
        {
            string debugVal = MelonPreferences.GetCategory("DuperyBluffSettings").GetEntry("DebugMode").GetValueAsString();
            if (debugVal == "True" || debugVal == "true" || debugVal == "t")
            {
                MelonLogger.Msg("DEBUG: " + message);
            }
        }



        public ActedInfo ReturnInfoWithSingleSelection(string info, Character selection)
        {
            Il2CppSystem.Collections.Generic.List<Character> selectionList = new Il2CppSystem.Collections.Generic.List<Character>();
            selectionList.Add(selection);
            return new ActedInfo(info, selectionList);
        }



        public Character GetRandomItemOfList(Il2CppSystem.Collections.Generic.List<Character> list)
        {
            if (list.Count == 0)
            {
                return null;
            }
            return list[UnityEngine.Random.RandomRangeInt(0, list.Count)];
        }
        public CharacterData GetRandomItemOfList(Il2CppSystem.Collections.Generic.List<CharacterData> list)
        {
            if (list.Count == 0)
            {
                return null;
            }
            return list[UnityEngine.Random.RandomRangeInt(0, list.Count)];
        }
        public string GetRandomItemOfList(Il2CppSystem.Collections.Generic.List<string> list)
        {
            if (list.Count == 0)
            {
                return null;
            }
            return list[UnityEngine.Random.RandomRangeInt(0, list.Count)];
        }
        public int GetRandomItemOfList(Il2CppSystem.Collections.Generic.List<int> list)
        {
            if (list.Count == 0)
            {
                return -1;
            }
            return list[UnityEngine.Random.RandomRangeInt(0, list.Count)];
        }

        public Il2CppSystem.Collections.Generic.List<CharacterData> GetUnderlingDatas(Character charRef)
        {
            if (allDatas.Length == 0)
            {
                var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
                if (loadedCharList != null)
                {
                    allDatas = new CharacterData[loadedCharList.Length];
                    for (int j = 0; j < loadedCharList.Length; j++)
                    {
                        allDatas[j] = loadedCharList[j]!.Cast<CharacterData>();
                    }
                }
            }

            Il2CppSystem.Collections.Generic.List<CharacterData> underlingDatas = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            underlingDatas.Add(charRef.dataRef);
            underlingDatas.Add(charRef.dataRef);
            underlingDatas.Add(charRef.dataRef);

            foreach (CharacterData character in allDatas)
            {
                if (character.characterId == "Underling_V_WING") underlingDatas[0] = character;
                if (character.characterId == "Underling_O_WING") underlingDatas[1] = character;
                if (character.characterId == "Underling_M_WING") underlingDatas[2] = character;
            }
            DebugMessage($"Underling roles found: {underlingDatas[0].characterName}, {underlingDatas[1].characterName}, {underlingDatas[2].characterName}");
            return underlingDatas;
        }



        public int MakeNumberWrong(int trueNumber, int falseNumber, int minimum)
        {
            int returnVal = falseNumber;
            if (returnVal < minimum)
            {
                while (returnVal < minimum) returnVal++;
            }
            if (trueNumber != falseNumber) return falseNumber;
            if (falseNumber == minimum) returnVal++;
            else returnVal--;
            return returnVal;
        }

        public int MakeNumberWrongByRange(int trueNumber, int falseNumber, int minimum, int maximum, int maxSubtract, int maxAdd)
        {
            int returnVal = falseNumber;
            if (returnVal < minimum)
            {
                while (returnVal < minimum) returnVal++;
            }
            if (returnVal > maximum)
            {
                while (returnVal > maximum) returnVal--;
            }
            if (trueNumber != falseNumber) return falseNumber;
            Il2CppSystem.Collections.Generic.List<int> possibleModifiers = new Il2CppSystem.Collections.Generic.List<int>();
            for (int i = (maxSubtract*-1); i < (maxAdd+1); i++)
            {
                if (i != 0 && (returnVal+i <= maximum) && (returnVal + i >= minimum))
                {
                    possibleModifiers.Add(i);
                }
            }
            if (possibleModifiers.Count == 0) return MakeNumberWrong(trueNumber, falseNumber, minimum);
            returnVal += possibleModifiers[UnityEngine.Random.RandomRangeInt(0, possibleModifiers.Count)];
            return returnVal;
        }

        public CharacterData[] allDatas = Il2CppSystem.Array.Empty<CharacterData>();
        public CharacterData GrabCharacterDataByID(string characterID)
        {
            if (allDatas.Length == 0)
            {
                var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
                if (loadedCharList != null)
                {
                    allDatas = new CharacterData[loadedCharList.Length];
                    for (int j = 0; j < loadedCharList.Length; j++)
                    {
                        allDatas[j] = loadedCharList[j]!.Cast<CharacterData>();
                    }
                }
            }

            for (int j = 0; j < allDatas.Length; j++)
            {
                if (characterID == allDatas[j].characterId)
                {
                    return allDatas[j];
                }
            }
            return null;
        }





        public Il2CppSystem.Collections.Generic.List<CharacterData> GetScriptRoles(EAlignment alignment)
        {
            Il2CppSystem.Collections.Generic.List<CharacterData> returnList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            foreach (CharacterData character in Gameplay.Instance.GetAllAscensionCharacters())
            {
                if (character.startingAlignment == alignment)
                {
                    returnList.Add(character);
                }
            }
            return returnList;
        }

        public Il2CppSystem.Collections.Generic.List<CharacterData> GetScriptRoles(ECharacterType type)
        {
            Il2CppSystem.Collections.Generic.List<CharacterData> returnList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            foreach (CharacterData character in Gameplay.Instance.GetAllAscensionCharacters())
            {
                if (character.type == type)
                {
                    returnList.Add(character);
                }
            }
            if (type == ECharacterType.Demon)
            {
                foreach (CharacterData character in GetAllDemons())
                {
                    returnList.Add(character);
                }
            }
            return returnList;
        }

        public Il2CppSystem.Collections.Generic.List<CharacterData> GetScriptRoles(EAlignment alignment, ECharacterType type)
        {
            Il2CppSystem.Collections.Generic.List<CharacterData> returnList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            foreach (CharacterData character in Gameplay.Instance.GetAllAscensionCharacters())
            {
                if (character.startingAlignment == alignment && character.type == type)
                {
                    returnList.Add(character);
                }
            }
            if (type == ECharacterType.Demon)
            {
                foreach (CharacterData character in GetAllDemons())
                {
                    if (character.startingAlignment == alignment) returnList.Add(character);
                }
            }
            return returnList;
        }

        public Il2CppSystem.Collections.Generic.List<CharacterData> GetScriptRoles()
        {
            Il2CppSystem.Collections.Generic.List<CharacterData> returnList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            foreach (CharacterData character in GetScriptRoles(EAlignment.Good)) returnList.Add(character);
            foreach (CharacterData character in GetScriptRoles(EAlignment.Evil)) returnList.Add(character);
            return returnList;
        }

        public Il2CppSystem.Collections.Generic.List<CharacterData> GetAllDemons()
        {
            if (allDatas.Length == 0)
            {
                var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
                if (loadedCharList != null)
                {
                    allDatas = new CharacterData[loadedCharList.Length];
                    for (int j = 0; j < loadedCharList.Length; j++)
                    {
                        allDatas[j] = loadedCharList[j]!.Cast<CharacterData>();
                    }
                }
            }

            Il2CppSystem.Collections.Generic.List<CharacterData> returnList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            for (int j = 0; j < allDatas.Length; j++)
            {
                if (allDatas[j].type == ECharacterType.Demon) returnList.Add(allDatas[j]);
            }
            return returnList;
        }

        public Il2CppSystem.Collections.Generic.List<CharacterData> GetPossibleHiddenRoles()
        {
            Il2CppSystem.Collections.Generic.List<CharacterData> returnList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            foreach (CharacterData character in Gameplay.Instance.GetScriptCharacters())
            {
                if (character.characterId == "Cryptid_WING")
                {
                    foreach (CharacterData character2 in GetScriptRoles(EAlignment.Evil, ECharacterType.Minion))
                    {
                        if (GetPossibleCharacterIDsOfRole("Cryptid_WING").Contains(character2.characterId))
                        {
                            returnList.Add(character2);
                        }
                    }
                }
                if (character.characterId == "Clown_LRZH") // This role hides the Demon
                {
                    foreach (CharacterData character2 in GetAllDemons())
                    {
                        returnList.Add(character2);
                    }
                }
            }
            return returnList;
        }


        public Il2CppSystem.Collections.Generic.List<string> GetLastStandIDs()
        {
            Il2CppSystem.Collections.Generic.List<string> returnList = new Il2CppSystem.Collections.Generic.List<string>();
            returnList.Add("Undying_WING");
            returnList.Add("Vizier_LRZH");
            returnList.Add("Apprentice_POW");
            returnList.Add("Squire_scm");
            return returnList;
        }


        public Il2CppSystem.Collections.Generic.List<string> GetLastStandPunisherIDs(bool includeNormalLastStand)
        {
            Il2CppSystem.Collections.Generic.List<string> returnList = new();
            if (includeNormalLastStand) returnList = GetLastStandIDs();
            returnList.Add("Praesect_WING");
            returnList.Add("Grenadier_POW");
            return returnList;
        }


        public CharacterData GetFaceUpClaim(Character character)
        {
            if (character.bluff) return character.bluff;
            return character.dataRef;
        }


        public Il2CppSystem.Collections.Generic.List<string> GetPossibleCharacterIDsOfRole(string roleID)
        {
            Il2CppSystem.Collections.Generic.List<string> returnChars = new Il2CppSystem.Collections.Generic.List<string>();

            if (roleID == "Cryptid_WING")
            {
                // Vanilla
                returnChars.Add("Baron_04539999"); // Chancellor
                returnChars.Add("Poisoner_64796285"); // Poisoner
                returnChars.Add("Mezepheles_09511163"); // Puppeteer
                returnChars.Add("Shaman_26945607"); // Shaman
                returnChars.Add("Witch_25286521"); // Witch

                // Wingidon's Expansion Pack
                returnChars.Add("Heretic_WING"); // Heretic
                returnChars.Add("Professional_WING"); // Professional
                returnChars.Add("Ritualist_WING"); // Ritualist
                returnChars.Add("Saboteur_WING"); // Saboteur
                returnChars.Add("Snake Charmer_WING"); // Snake Charmer
                returnChars.Add("Turncoat_WING"); // Turncoat

                // Skill Cycler's Riddles
                returnChars.Add("Accuser_scm"); // Accuser
                returnChars.Add("Baffler_scm"); // Baffler
                returnChars.Add("Hypnotist_scm"); // Hypnotist

                // LRZH's Circus
                returnChars.Add("Clown_LRZH"); // Clown
                returnChars.Add("Wraith_LRZH"); // Wraith
            }
            if (roleID == "Occultist_WING")
            {
                /*
                Acts after Chancellor:
                - Accuser
                - Baffler
                - Heretic
                - Poisoner
                - Puppeteer
                - Saboteur
                - Shaman
                - Snake Charmer
                - Slanderer
                - Swarm
                - Witch

                Doesn't act on start:
                - Ritualist
                */

                // Vanilla
                returnChars.Add("Poisoner_64796285"); // Poisoner
                returnChars.Add("Mezepheles_09511163"); // Puppeteer
                returnChars.Add("Shaman_26945607"); // Shaman
                returnChars.Add("Witch_25286521"); // Witch

                // Wingidon's Expansion Pack
                returnChars.Add("Heretic_WING"); // Heretic
                returnChars.Add("Ritualist_WING"); // Ritualist
                returnChars.Add("Saboteur_WING"); // Saboteur
                returnChars.Add("Snake Charmer_WING"); // Snake Charmer 
                returnChars.Add("Swarm_Good_WING"); // Good Swarm 

                // Skill Cycler's Riddles
                returnChars.Add("Accuser_scm"); // Accuser
                returnChars.Add("Baffler_scm"); // Baffler
                returnChars.Add("Slanderer_scm"); // Slanderer
            }
            if (roleID == "Mutant_WING")
            {
                // Vanilla
                returnChars.Add("Minion_71804875"); // Minion
                returnChars.Add("Poisoner_64796285"); // Poisoner
                returnChars.Add("Mezepheles_09511163"); // Puppeteer
                returnChars.Add("Shaman_26945607"); // Shaman
                returnChars.Add("Twin Minion_15695218"); // Twin Minion
                returnChars.Add("Witch_25286521"); // Witch

                // Wingidon's Expansion Pack
                returnChars.Add("Acolyte_WING"); // Acolyte
                returnChars.Add("Heretic_WING"); // Heretic
                returnChars.Add("Professional_WING"); // Professional
                returnChars.Add("Ritualist_WING"); // Ritualist
                returnChars.Add("Saboteur_WING"); // Saboteur
                returnChars.Add("Snake Charmer_WING"); // Snake Charmer
                returnChars.Add("Swarm_Good_WING"); // Swarm
                returnChars.Add("Turncoat_WING"); // Turncoat
                returnChars.Add("Undying_WING"); // Undying
                returnChars.Add("Zealot_WING"); // Zealot

                // Skill Cycler's Riddles
                returnChars.Add("Accuser_scm"); // Accuser
                returnChars.Add("Baffler_scm"); // Baffler
                returnChars.Add("Guardian_scm"); // Guardian
                returnChars.Add("Hypnotist_scm"); // Hypnotist

                // LZRH's Circus
                returnChars.Add("Clown_LRZH"); // Clown

                // Tavern Mod
                returnChars.Add("Brewer_TAVERN"); // Brewer
                returnChars.Add("Florist_TAVERN"); // Florist
                returnChars.Add("Gangster_TAVERN"); // Gangster
                returnChars.Add("Strategist_TAVERN"); // Strategist
                returnChars.Add("Summoner_TAVERN"); // Summoner
                returnChars.Add("Trickster_TAVERN"); // Trickster

                // Mass Hysteria
                returnChars.Add("Siren_MaHy"); // Siren

                // ExtraRandomised
                returnChars.Add("Purifier_ER"); // Purifier

                // CarlzVillagePack
                returnChars.Add("Husher_VP"); // Blackmailer
                returnChars.Add("Lycaon_VP"); // Lycaon

                // RevealDilemma
                returnChars.Add("Ambush_rdm"); // Ambusher
                returnChars.Add("Martyr_rdm"); // Martyr

                // CSK's Expansion Pack
                returnChars.Add("Cavalier_EP"); // Cavalier
            }
            if (roleID == "Pandemonium_WING")
            {
                // Vanilla
                returnChars.Add("Imp_58992273"); // Baa
                returnChars.Add("Lillith_90453844"); // Lilis
                returnChars.Add("Pooka_13445289"); // Pooka

                // Wingidon's Expansion Pack
                returnChars.Add("Caedoccidere_WING"); // Caedoccidere
                returnChars.Add("Carnicarius_WING"); // Carnicarius
                returnChars.Add("Iris_WING"); // Iris
                returnChars.Add("Leviathan_WING"); // Leviathan
                //returnChars.Add("Mendaverte_WING"); // Mendaverte
                returnChars.Add("Praesect_WING"); // Praesect
                returnChars.Add("Mezepheles_WING"); // Venelum

                // Skill Cycler's Riddles
                returnChars.Add("Escapist_scm"); // Escapist
                returnChars.Add("Follower_scm"); // Follower
                returnChars.Add("Infestation_scm"); // Infestation
                returnChars.Add("Kingmaker_scm"); // Kingmaker
                returnChars.Add("Mystifier_scm"); // Mystifier
                returnChars.Add("Veil_scm"); // Veil

                // LRZH's Circus
                returnChars.Add("Lleech_LRZH"); // Lleech
                returnChars.Add("Po_LRZH"); // Po

                // Reveal Dilemma
                returnChars.Add("shroud_rdm"); // Shroud

                // Mass Hysteria
                returnChars.Add("Cackler_MaHy"); // Cakler

                // ExtraRandomized
                returnChars.Add("Hypnotist_ER"); // Hypnotist

                // CarlzVillagePack
                returnChars.Add("Hydra_VP"); // Hydra
                returnChars.Add("Phantom_VP"); // Phantom

                // CSK's Expansion Pack
                returnChars.Add("Belias_EP"); // Belias
            }
            return returnChars;
        }


        bool CheckModInstalled(string modName)
        {
            return MelonBase.RegisteredMelons
                .Any(melon => melon.Info.Name == modName);
        }

        public Il2CppSystem.Collections.Generic.List<string> GetInstalledMods()
        {
            // My attempt at detecting other in-play mods.
            Il2CppSystem.Collections.Generic.List<string> installedMods = new Il2CppSystem.Collections.Generic.List<string>();
            installedMods.Add("Wingidon's Expansion Pack");
            if (CheckModInstalled("Skill Cycler's Riddles")) installedMods.Add("Riddles"); // Skill Cycler's Riddles
            if (CheckModInstalled("Demon Bluff Mods")) installedMods.Add("Power Play"); // Power Play
            if (CheckModInstalled("Circus")) installedMods.Add("Circus"); // LRZH's Circus
            if (CheckModInstalled("Windways_TheSalemTrials")) installedMods.Add("The Salem Trials"); // LRZH's Circus
            return installedMods;
        }


        /* Will finish this later
        public string GetRoleIDByName(string name)
        {
            switch (name)
            {
                // Vanilla
                case "Alchemist": return "Alchemist_94446803"; // Alchemist
                case "Architect": return ""; // Architect
                case "Baker": return ""; // Baker
                case "Bard": return ""; // Bard
                case "Bishop": return ""; // Bishop
                case "Confessor": return ""; // Confessor
                case "Dreamer": return ""; // Dreamer
                case "Druid": return ""; // Druid
                case "Empress": return ""; // Empress
                case "Enlightened": return ""; // Enlightened
                case "Fortune Teller": return ""; // Fortune Teller
                case "Gemcrafter": return ""; // Gemcrafter
                case "Hunter": return ""; // Hunter
                case "Investigator": return ""; // Investigator
                case "Jester": return ""; // Jester
                case "Judge": return ""; // Judge
                case "Knight": return ""; // Knight
                case "Knitter": return ""; // Knitter
                case "Lover": return ""; // Lover
                case "Medium": return ""; // Medium
                case "Oracle": return ""; // Oracle
                case "Poet": return ""; // Poet
                case "Scout": return ""; // Scout
                case "Slayer": return ""; // Slayer
                case "Witness": return ""; // Witness

                case "Bombardier": return ""; // Bombardier
                case "Doppelganger": return ""; // Doppelganger
                case "Drunk": return ""; // Drunk
                case "Lycanthrope": return ""; // Lycanthrope
                case "Plague Doctor": return ""; // Plague Doctor
                case "Rambler": return ""; // Rambler
                case "Wretch": return ""; // Wretch

                case "Chancellor": return ""; // Chancellor
                case "Minion": return ""; // Minion
                case "Poisoner": return ""; // Poisoner
                case "Puppet": return ""; // Puppet
                case "Puppeteer": return ""; // Puppeteer
                case "Shaman": return ""; // Shaman
                case "Twin Minion": return ""; // Twin Minion
                case "": return ""; // Werewolf
                case "": return ""; // Witch

                case "": return ""; // Baa
                case "": return ""; // Lilis
                case "": return ""; // Pooka


                
                // Wingidon's Expansion Pack
                case "": return ""; // Arbiter
                case "": return ""; // Arithmetician
                case "": return ""; // Bloodseer
                case "": return ""; // Cardshark
                case "": return ""; // Chiromancer
                case "": return ""; // Clairvoyant
                case "": return ""; // Copycat
                case "": return ""; // Detective
                case "": return ""; // Devout
                case "": return ""; // Forager
                case "": return ""; // Gossip
                case "": return ""; // Graveakeeper
                case "": return ""; // Introvert
                case "": return ""; // Jewelsmith
                case "": return ""; // Knave
                case "": return ""; // Lamb
                case "": return ""; // Performer
                case "": return ""; // Prince
                case "": return ""; // Ranger
                case "": return ""; // Scavenger
                case "": return ""; // Sentinel
                case "": return ""; // Sheriff
                case "": return ""; // Spy
                case "": return ""; // Warden

                case "": return ""; // Chatterbox
                case "": return ""; // Lunatic
                case "": return ""; // Marionette
                case "": return ""; // Mutant
                case "": return ""; // Revolutionary
                case "": return ""; // Renegade
                case "": return ""; // Tergiversator

                case "": return ""; // Acolyte
                case "": return ""; // Fanatic
                case "": return ""; // Zealot
                case "": return ""; // Heretic
                case "": return ""; // Professional
                case "": return ""; // Ritualist
                case "": return ""; // Snake Charmer
                case "": return ""; // Swarm (Good)
                case "": return ""; // Swarm (Evil)
                case "": return ""; // Turncoat
                case "": return ""; // Undying

                case "": return ""; // Agmeres
                case "": return ""; // Caedoccidere
                case "": return ""; // Carnicarius
                case "": return ""; // Iris
                case "": return ""; // Leviathan
                case "": return ""; // Mendaverte
                case "": return ""; // Praesect
                case "": return ""; // Sanguitaurus
                case "": return ""; // Tenecaligo
                case "": return ""; // Venelum
                case "": return ""; // Veniyon
                case "": return ""; // Vidiyon
                case "": return ""; // Viciyon

                // Skill Cycler's Riddles
                case "": return ""; // Coach
                case "": return ""; // Comedian
                case "": return ""; // Commander
                case "": return ""; // Cowboy
                case "": return ""; // Director
                case "": return ""; // Engineer
                case "": return ""; // Governor
                case "": return ""; // Innkeeper
                case "": return ""; // Lawyer
                case "": return ""; // Mathematician
                case "": return ""; // Necromancer
                case "": return ""; // Nurse
                case "": return ""; // Obsessor
                case "": return ""; // Officer
                case "": return ""; // Pioneer
                case "": return ""; // Psychic
                case "": return ""; // Recruiter
                case "": return ""; // Riddler
                case "": return ""; // Scanner
                case "": return ""; // Stylist
                case "": return ""; // Surveyor
                case "": return ""; // Swapper
                case "": return ""; // Tracker
                case "": return ""; // Trickster
                case "": return ""; // Weaver

                case "": return ""; // Captivator
                case "": return ""; // Confectioner
                case "": return ""; // Gambler
                case "": return ""; // Ghost
                case "": return ""; // Hitman
                case "": return ""; // Mad Scientist
                case "": return ""; // Muddler
                case "": return ""; // Reflector

                case "": return ""; // Accuser
                case "": return ""; // Baffler
                case "": return ""; // Channeler
                case "": return ""; // Guardian
                case "": return ""; // Hypnotist
                case "": return ""; // Mastermind
                case "": return ""; // Sleeper
                case "": return ""; // Wizard

                case "": return ""; // Escapist
                case "": return ""; // Follower
                case "": return ""; // Infestation
                case "": return ""; // Kingmaker
                case "": return ""; // Mystifier
                case "": return ""; // Summoner
                case "": return ""; // Veil
            }
            return "";
        }
        */







        public string ConjureStatusName(string statusID)
        {
            switch (statusID)
            {
                // This mod
                case "315167151": return "Befriended (Good Cop)";
                case "31516214": return "Befriended (Bad Cop)";
                case "1853152120": return "Recruited";
                case "1853146320": return "Evil (Kingpin)";
            }
            return statusID;
        }



        public Il2CppSystem.Collections.Generic.List<Character> GetCurrentCharacters()
        {
            Il2CppSystem.Collections.Generic.List<Character> allChars = new Il2CppSystem.Collections.Generic.List<Character>();
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                allChars.Add(character);
            }
            return allChars;
        }



        [HarmonyPatch(typeof(Gameplay), "OnCharacterReveal")]
        public static class w_AnyRevealPatch
        {
            public static ECharacterStatus JustRevealed = (ECharacterStatus)1021192018;
            public static ETriggerPhase AnyReveal = (ETriggerPhase)1121228522;
            public static ETriggerPhase SelfReveal = (ETriggerPhase)1951361852; // Used for Pick characters
            [HarmonyPrefix]
            public static bool CharacterRevealPrefix(Character obj)
            {
                // MelonLogger.Msg("Revealing character...");
                obj.statuses.AddStatus(JustRevealed, obj);
                obj.Act(SelfReveal);
                foreach (Character character in Gameplay.CurrentCharacters)
                {
                    //MelonLogger.Msg("Calling on the Ritualist");
                    character.Act(AnyReveal);
                }
                obj.statuses.statuses.Remove(JustRevealed);
                return true;
            }
        }
        public static class w_StatusLog
        {
            [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
            public static class pvt
            {
                public static void Postfix(Character __instance)
                {
                    if (__instance.statuses.statuses.Count != 0)
                    {
                        wx_SavedScripts sharedScripts = new wx_SavedScripts();
                        foreach (ECharacterStatus status in __instance.statuses.statuses)
                        {
                            sharedScripts.DebugMessage($"Found status on #{__instance.id}: {sharedScripts.ConjureStatusName(status.ToString())}");
                        }
                    }
                }
            }
        }


        public static class KillText
        {
            [HarmonyPatch(typeof(Character), nameof(Character.ShowDescription))]
            public static class ChangeKillByDemonText
            {
                public static void Postfix(Character __instance)
                {
                    if ((__instance.killedByDemon || __instance.statuses.Contains(ECharacterStatus.KilledByEvil)) && Gameplay.GameplayState != EGameplayState.Summary)
                    {
                        HintInfo info = new HintInfo();
                        info.text = "This character is dead.\nThey cannot use abilities and their True Role is not revealed.";
                        UIEvents.OnShowHint.Invoke(info, __instance.hintPivot);
                    }
                }
            }
        }




        /* Doesn't fucking work :(
        [HarmonyPatch(typeof(Gossip), nameof(Gossip.infoRoles))]
        private static class PoetInfo
        {
            private static void Postfix(ref Gossip __instance, Character charRef, ref Il2CppSystem.Collections.Generic.List<Role> __result)
            {
                __result.Add(new w_Arithmetician());
                __result.Add(new w_Chiromancer());
                __result.Add(new w_Clairvoyant());
                __result.Add(new w_Detective());
                __result.Add(new w_Devout());
                __result.Add(new w_Introvert());
                __result.Add(new w_Jewelsmith());
                __result.Add(new w_Lamb());
                __result.Add(new w_Prince());
                __result.Add(new w_Ranger());
                __result.Add(new w_Sentinel());
                __result.Add(new w_Sheriff());
                __result.Add(new w_Spy());
            }
        }
        */
    }

}
