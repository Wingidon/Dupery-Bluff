using DuperyBluff;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using Il2CppSystem.Runtime.Remoting.Messaging;
using MelonLoader;
using MelonLoader.Utils;
using System;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using UnityEngine;
using UnityEngine.Playables;
using static Il2Cpp.GameplayEvents;
using static Il2CppSystem.Array;
using static MelonLoader.Modules.MelonModule;
using Il2CppSystem.Reflection;

[assembly: MelonInfo(typeof(MainMod), "Dupery Bluff", "1.0.0", "Wingidon")]
[assembly: MelonGame("UmiArt", "Demon Bluff")]

namespace DuperyBluff;
public class MainMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        // Villagers
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_GoodCop>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Mathematician>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Priest>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_PrivateEye>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Reporter>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Skeptic>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Therapist>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Vigilante>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Weatherman>();

        // Outcasts
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Copycat>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Drunkard>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_FallGuy>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Surgeon>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Youngster>();

        // Minions
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_BadCop>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Barkeep>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Mobster>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Poisoner>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_SerialKiller>();
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_TravelAgent>();

        // Demons
        ClassInjector.RegisterTypeInIl2Cpp<w_Dupe_Idol>();
    }


    public MelonPreferences_Category duperyModConfigCategory = null!;
    public override void OnLateInitializeMelon()
    {
        GameObject content = GameObject.Find("Game/Gameplay/Content");
        NightPhase nightPhase = content.GetComponent<NightPhase>();
        Statics.GetStartingRoles();



        duperyModConfigCategory = MelonPreferences.CreateCategory("DuperyBluffSettings");

        // Debug
        duperyModConfigCategory.CreateEntry("DebugMode", false, "Debug Mode", "DEBUG\nWhether or not debug mode is enabled. Debug Mode outputs logs to the console about some roles and what they're doing.");

        // Village Generation
        duperyModConfigCategory.CreateEntry("Traitor_Weight", 9, description: "\nVILLAGE GENERATION\nHow likely the Critic, Idol or Recruiter are to be in-play.\nSetting this to \'9\' will give them equal odds to the vanilla Demons.");
        duperyModConfigCategory.CreateEntry("EnableLargeVillages", false, "EnableLargeVillages", "\nWhen this setting is enabled, every Demon from this mod can show up in villages up to 16 cards big.");

        // Villagers
        duperyModConfigCategory.CreateEntry("PrivateEye_InfoHour", 5, description: "\n\n\nVILLAGERS\nHow many characters must be Revealed before the Private Eye Learns anything?\nDefault: 5");
        duperyModConfigCategory.CreateEntry("Skeptic_FaithThreshold", 6, description: "\nAt what point does the Skeptic lose faith in you?\nDefault: 6");

        // Outcasts
        duperyModConfigCategory.CreateEntry("Drunkard_Damage", 5, description: "\n\n\nOUTCASTS\nThe penalty for executing the Drunkard.\nDefault: 5\nRecommended: 3");
        duperyModConfigCategory.CreateEntry("Surgeon_Damage", 0, description: "\nThe damage dealt when the Surgeon botches a surgery.\nDefault: 0\nRecommended: 2");
        duperyModConfigCategory.CreateEntry("Surgeon_KillChance", 50, description: "\nThe percent chance for a Surgeon to botch a surgery.\nDefault: 50");
        duperyModConfigCategory.CreateEntry("Surgeon_KillHour", 6, description: "\nHow many characters must be Revealed before the Surgeon botches a surgery?\nDefault: 6");
        duperyModConfigCategory.CreateEntry("Youngster_Damage", 5, description: "\nThe additional penalty for executing the Youngster.\nDefault: 5");

        // Minions
        duperyModConfigCategory.CreateEntry("Poisoner_Range", 1, description: "\n\nMINIONS\nThe Poisoner's range.\nDefault: 1");
        duperyModConfigCategory.CreateEntry("Scoundrel_FailPenalty", 0, description: "\nThe penalty for attempting (but failing) to execute a Scoundrel.\nDefault: 0");
        duperyModConfigCategory.CreateEntry("SerialKiller_Range", 1, description: "\nThe Serial Killer's range.\nDefault: 1\nRecommended: 2");
        duperyModConfigCategory.CreateEntry("SerialKiller_Damage", 0, description: "\nThe Serial Killer's damage per kill.\nDefault: 0\nRecommended: 1");

        duperyModConfigCategory.SetFilePath(Path.Combine(MelonEnvironment.UserDataDirectory, "DuperyBluffSettings.cfg"));
        duperyModConfigCategory.SaveToFile();
        



        wx_SavedScripts sharedScripts = new wx_SavedScripts();

        /*
        CharacterData w_prince = newCharacter("Prince", EAlignment.Good, ECharacterType.Villager, true, false, "\"Secretly wishes that his mother was more trusting.\"", "Bishop_58855542");
        w_prince.role = new w_Prince();
        w_prince.description = "Learn that exactly 1 of 2 characters is Disguised.";
        w_prince.ifLies = $"Both characters in my info are {formattedKeyText("Honest")}.";
        w_prince.gender = EGender.Male;

        CharacterData w_forager = newCharacter("Forager", EAlignment.Good, ECharacterType.Villager, true, false, "\"Her instructions are clear, it's just that her assistants don't follow them.\"", "Gossip_85354100");
        w_forager.role = new w_Forager();
        w_forager.description = "<b>Pick 1 character:</b>\nLearn if they are a Villager.";
        w_forager.hints = customHint("Ability Refresh Hint", "Once Per Game") + $"\n\nArt by {formattedKeyText("WeekendWolf")} ({formattedKeyText("@weekendwolf")}) on {formattedKeyText("Discord")}";
        w_forager.gender = EGender.Female;
        w_forager.picking = true;
        w_forager.abilityUsage = EAbilityUsage.Once;
        */


        CharacterData w_dupe_priest = newCharacter("Priest", EAlignment.Good, ECharacterType.Villager, true, false, "\"The holiest of priests.\nAlso really bad at lying.\"", "Bishop_58855542");
        w_dupe_priest.role = new w_Dupe_Priest();
        w_dupe_priest.description = "Learn that \"I am the Priest\"";
        w_dupe_priest.ifLies = $"Learn something else";


        CharacterData w_dupe_weatherman = newCharacter("Weatherman", EAlignment.Good, ECharacterType.Villager, true, false, "\"It'll be windy today, lots of Villagers with a high chance of Outcasts.\nThose are meteorological terms?\"", "Scout_88081716");
        w_dupe_weatherman.role = new w_Dupe_Weatherman();
        w_dupe_weatherman.description = "Learn 3 characters.\nOne is a Villager, one is an Outcast, and one is a Minion or Demon.";
        w_dupe_weatherman.ifLies = $"All characters in my info are Villagers or Outcasts.";
        w_dupe_weatherman.hints = "If I cannot find a character of a particular type for my info, Learn this.\n\nI cannot mention myself.";


        CharacterData w_dupe_reporter = newCharacter("Reporter", EAlignment.Good, ECharacterType.Villager, true, false, "\"You gotta get in there if you want the juiciest stories!\"", "Hunter_93427887");
        w_dupe_reporter.role = new w_Dupe_Reporter();
        w_dupe_reporter.description = "Learn how many steps I'd need to take to reach the closest Evil character.";


        CharacterData w_dupe_skeptic = newCharacter("Skeptic", EAlignment.Good, ECharacterType.Villager, true, false, "\"Won't cooperate if they don't have a reason to.\nAlso won't cooperate if they have a reason not to.\"", "Witness_25155076");
        w_dupe_skeptic.role = new w_Dupe_Skeptic();
        w_dupe_skeptic.description = $"Learn a character and their {formattedKeyText("Alignment")}.\n\nIf your {formattedKeyText("Health")} is {duperyModConfigCategory.GetEntry<int>("Skeptic_FaithThreshold").Value} or lower, I lose faith in you.";
        w_dupe_skeptic.ifLies = $"I call a character the wrong {formattedKeyText("Alignment")}. I never lose faith in you.";


        CharacterData w_dupe_mathematician = newCharacter("Mathematician", EAlignment.Good, ECharacterType.Villager, true, false, "\"Carries the one, then carries the village.\"", "Gossip_85354100");
        w_dupe_mathematician.role = new w_Dupe_Mathematician();
        w_dupe_mathematician.description = $"Learn the sum of all Evil.";
        w_dupe_mathematician.ifLies = $"Learn a number that is slightly higher or slightly lower than the true sum.";


        CharacterData w_dupe_therapist = newCharacter("Therapist", EAlignment.Good, ECharacterType.Villager, true, false, "\"Surprisingly good at getting Evil to admit to their crimes\"", "Lover_91302708");
        w_dupe_therapist.role = new w_Dupe_Therapist();
        w_dupe_therapist.description = $"Learn how many Evil characters are adjacent to me.";


        CharacterData w_dupe_privateeye = newCharacter("Private Eye", EAlignment.Good, ECharacterType.Villager, true, false, "\"He's already on the case!\"", "Investigator_34015277");
        w_dupe_privateeye.role = new w_Dupe_PrivateEye();
        w_dupe_privateeye.description = $"After {duperyModConfigCategory.GetEntry<int>("PrivateEye_InfoHour").Value} {formattedKeyText("Reveals")}, Learn an Unrevealed Minion character if possible.";
        w_dupe_privateeye.hints = "If there are no Unrevealed Minions, \"I got nothing\"";


        CharacterData w_dupe_vigilante = newCharacter("Vigilante", EAlignment.Good, ECharacterType.Villager, true, false, "\"Bringer of justice!\nAlso a criminal, but nevermind that.\"", "Gambler_42592744");
        w_dupe_vigilante.role = new w_Dupe_Vigilante();
        w_dupe_vigilante.description = $"<b>Pick 1 character:</b>\nIf Evil picked, I {formattedKeyText("Kill")} them.";
        w_dupe_vigilante.ifLies = "I always miss my target.";
        w_dupe_vigilante.picking = true;
        w_dupe_vigilante.abilityUsage = EAbilityUsage.Once;


        CharacterData w_dupe_goodcop = newCharacter("Good Cop", EAlignment.Good, ECharacterType.Villager, true, false, "\"Bad Cop's (reluctant) partner.\"", "Knight_47970624");
        w_dupe_goodcop.role = new w_Dupe_GoodCop();
        w_dupe_goodcop.description = $"<b>Pick 1 character:</b>\nI convert their {formattedKeyText("Alignment")} to my own.\nIf the last Evil becomes Good this way, they {formattedKeyText("Die")}.\n\nThe <color=#BA4848>Bad Cop</color> is in-play.";
        w_dupe_goodcop.ifLies = "My ability does not work.";
        w_dupe_goodcop.picking = true;
        w_dupe_goodcop.abilityUsage = EAbilityUsage.Once;


        CharacterData w_dupe_tailor = newCharacter("Tailor", EAlignment.Good, ECharacterType.Villager, true, false, "\"Knows when people are cut from the same cloth.\"", "Knitter_32352172");
        w_dupe_tailor.role = new w_Dupe_Tailor();
        w_dupe_tailor.description = $"<b>Pick 2 characters:</b>\nLearn if they are the same {formattedKeyText("Alignment")}.";
        w_dupe_tailor.picking = true;
        w_dupe_tailor.abilityUsage = EAbilityUsage.Once;


        CharacterData w_dupe_researcher = newCharacter("Researcher", EAlignment.Good, ECharacterType.Villager, true, false, "\"Knows just a little too much about just about everyone.\"", "Fortune Teller_74565681");
        w_dupe_researcher.role = new w_Dupe_Researcher();
        w_dupe_researcher.description = $"<b>Pick 1 character:</b>\nLearn if they are a Villager.";
        w_dupe_researcher.picking = true;
        w_dupe_researcher.abilityUsage = EAbilityUsage.Once;



        CharacterData w_dupe_fallguy = newCharacter("Fall Guy", EAlignment.Good, ECharacterType.Outcast, false, false, "\"Whenever something goes wrong, everyone wants someone to blame.\"", "Wretch_80988916");
        w_dupe_fallguy.role = new w_Dupe_FallGuy();
        w_dupe_fallguy.description = $"I Register as Evil and as a Minion.";
        w_dupe_fallguy.hints = $"I cannot be Disguised as.\nCharacters who check my Role will be correct about it - only my Type and {formattedKeyText("Alignment")} Register falsely.";

        CharacterData w_dupe_fallguy_minion = newCharacter("Fall Guy", EAlignment.Evil, ECharacterType.Minion, false, false, "\"Whenever something goes wrong, everyone wants someone to blame.\"", "Wretch_80988916");
        w_dupe_fallguy_minion.role = new w_Dupe_FallGuy();
        w_dupe_fallguy_minion.description = $"The Fall Guy Registers as me.";
        w_dupe_fallguy_minion.hints = $"You shouldn't be seeing this.";
        w_dupe_fallguy_minion.characterId = "WING_Dupery_Fall Guy MinionRegister";


        CharacterData w_dupe_surgeon = newCharacter("Surgeon", EAlignment.Good, ECharacterType.Outcast, true, false, "\"Look, it's easier than you think to botch a surgery under stress, okay?\"", "Lookout_41018246");
        w_dupe_surgeon.role = new w_Dupe_Surgeon();
        w_dupe_surgeon.description = $"After 6 {formattedKeyText("Reveals")}, I might {formattedKeyText("Kill")} a Good Villager.";
        w_dupe_surgeon.ifLies = $"I never {formattedKeyText("Kill")} anyone.";
        if (duperyModConfigCategory.GetEntry<int>("Surgeon_Damage").Value != 0) w_dupe_surgeon.description += $"\nIf I do, deal {duperyModConfigCategory.GetEntry<int>("Surgeon_Damage").Value} {formattedKeyText("Damage")} to you.";

        CharacterData w_dupe_copycat = newCharacter("Copycat", EAlignment.Good, ECharacterType.Outcast, false, true, "\"Curiosity couldn't keep this cat down.\nSatisfaction always brought it back.\"", "Doppleganger_52694042");
        w_dupe_copycat.role = new w_Dupe_Copycat();
        w_dupe_copycat.description = $"I Disguise as an in-play Good character.";

        CharacterData w_dupe_drunkard = newCharacter("Drunkard", EAlignment.Good, ECharacterType.Outcast, false, true, "\"Every day, the bar opens at 9AM sharp.\nThe Drunkard then stumbles in about 2 seconds later.\"", "Drunk_15369527");
        w_dupe_drunkard.role = new w_Dupe_Drunkard();
        w_dupe_drunkard.description = $"I Lie and Disguise as a not-in-play Good character.";
        if (duperyModConfigCategory.GetEntry<int>("Drunkard_Damage").Value != 5) w_dupe_drunkard.hints = $"Executing me deals {duperyModConfigCategory.GetEntry<int>("Drunkard_Damage").Value} {formattedKeyText("Damage")} to you instead of 5.";

        CharacterData w_dupe_youngster = newCharacter("Youngster", EAlignment.Good, ECharacterType.Outcast, true, false, "\"Are you really gonna execute the poor kid?\"", "Scout_88081716");
        w_dupe_youngster.role = new w_Dupe_Youngster();
        w_dupe_youngster.description = $"If you Execute me, take {duperyModConfigCategory.GetEntry<int>("Youngster_Damage").Value} additional {formattedKeyText("Damage")}.";
        w_dupe_youngster.hints = "My ability does not work if I am Evil.";

        CharacterData w_dupe_wannabe = newCharacter("Wannabe", EAlignment.Good, ECharacterType.Outcast, false, true, "\"Pretends and wants to be Evil, but just\ndoesn't have it in their heart.\"", "Witch_25286521");
        w_dupe_wannabe.role = new w_Dupe_Wannabe();
        w_dupe_wannabe.description = $"I Disguise as a Corrupted in-play Minion.\nOne Minion doesn't Disguise.";
        w_dupe_wannabe.hints = "My ability can activate on already face-up Minions.\n\nIf there are no Minions, I do not Disguise.";

        /* Keeps breaking by initialising at weird times for no reason whatsoever
        CharacterData w_dupe_bountyhunter = newCharacter("Bounty Hunter", EAlignment.Good, ECharacterType.Outcast, true, false, "\"Searching for an unrelated crook.\nTracked them down to this village.\"", "Hunter_93427887");
        w_dupe_bountyhunter.role = new w_Dupe_BountyHunter();
        w_dupe_bountyhunter.description = $"<b>Game Start:</b>\n1 Good Villager is Corrupted.\n\nLearn that I Corrupted 1 of 2 characters.";
        */


        CharacterData w_dupe_mobster = newCharacter("Mobster", EAlignment.Evil, ECharacterType.Minion, false, true, "\"His favourite phrases are \'You're the boss, boss\', \'Consider it done, boss\' and \'You gotta problem with the boss?!\'\"", "Baron_04539999");
        w_dupe_mobster.role = new w_Dupe_Mobster();
        w_dupe_mobster.description = "I Lie and Disguise as an in-play Good character.";


        CharacterData w_dupe_poisoner = newCharacter("Poisoner", EAlignment.Evil, ECharacterType.Minion, false, true, "\"If you cross the syndicate, you'd better check your drinks.\"", "Poisoner_64796285");
        w_dupe_poisoner.role = new w_Dupe_Poisoner();
        w_dupe_poisoner.description = "<b>Game Start:</b>\n1 Villager adjacent to me is Corrupted, if possible.\n\nI Lie and Disguise.";
        if (duperyModConfigCategory.GetEntry<int>("Poisoner_Range").Value != 1) w_dupe_poisoner.description = $"<b>Game Start:</b>\n1 Villager within {duperyModConfigCategory.GetEntry<int>("Poisoner_Range").Value} steps of me is Corrupted, if possible.\n\nI Lie and Disguise.";


        CharacterData w_dupe_travelagent = newCharacter("Travel Agent", EAlignment.Evil, ECharacterType.Minion, false, true, "\"Are you looking for the Reporter?\nSorry, they're out travelling currently.\"", "Architect_39883285");
        w_dupe_travelagent.role = new w_Dupe_TravelAgent();
        w_dupe_travelagent.description = $"While I am {formattedKeyText("Alive")}, the last character cannot be {formattedKeyText("Revealed")}\n\nI Lie and Disguise.";


        CharacterData w_dupe_serialkiller = newCharacter("Serial Killer", EAlignment.Evil, ECharacterType.Minion, false, true, "\"He can never get enough blood!\"", "Slayer_WING");
        w_dupe_serialkiller.role = new w_Dupe_SerialKiller();
        w_dupe_serialkiller.description = $"<b>{formattedKeyText("Cycle 4")}:</b>\nI {formattedKeyText("Kill")} a random Good character adjacent to me, if possible.";
        if (duperyModConfigCategory.GetEntry<int>("SerialKiller_Range").Value != 1) w_dupe_serialkiller.description = $"<b>{formattedKeyText("Cycle 4")}:</b>\nI {formattedKeyText("Kill")} a random Good character within 2 steps of me, if possible.";
        if (duperyModConfigCategory.GetEntry<int>("SerialKiller_Damage").Value != 0) w_dupe_serialkiller.description += $"\nDeal {duperyModConfigCategory.GetEntry<int>("SerialKiller_Damage").Value} {formattedKeyText("Damage")} to you.";
        w_dupe_serialkiller.description += "\n\nI Lie and Disguise.";


        CharacterData w_dupe_badcop = newCharacter("Bad Cop", EAlignment.Evil, ECharacterType.Minion, false, true, "\"Good Cop's (way-too-eager) partner.\"", "Knight_47970624");
        w_dupe_badcop.role = new w_Dupe_BadCop();
        w_dupe_badcop.description = $"I Disguise as the <color=#8BC6E4>Good Cop</color>.\nThe <color=#8BC6E4>Good Cop</color> is in-play.";


        CharacterData w_dupe_barkeep = newCharacter("Barkeep", EAlignment.Evil, ECharacterType.Minion, false, true, "\"Opens the bar at 9AM sharp, and serves the Drunkard about two seconds later.\"", "Alchemist_94446803");
        w_dupe_barkeep.role = new w_Dupe_Barkeep();
        w_dupe_barkeep.description = $"<b>Game Start:</b>\n1 Good Villager becomes the <color=#F6D88D>Drunkard</color>.\n\nI Lie and Disguise.";
        w_dupe_barkeep.additionalPossibleCharacters.count.Add(NewPossibleCharacterCount(ECharacterType.Outcast, 1));


        CharacterData w_dupe_scoundrel = newCharacter("Scoundrel", EAlignment.Evil, ECharacterType.Minion, false, true, "\"They're just daring you to approach!\nStick it to 'em, Detective.\"", "Bombardier_79093372");
        w_dupe_scoundrel.role = new w_Dupe_Scoundrel();
        w_dupe_scoundrel.description = $"I can't {formattedKeyText("Die")} unless I am the last {formattedKeyText("Alive")} Evil character.";
        w_dupe_scoundrel.hints = $"My ability checks everyone's {formattedKeyText("True Role")}. Characters who Register falsely do not protect me.";
        if (duperyModConfigCategory.GetEntry<int>("Scoundrel_FailPenalty").Value != 0) w_dupe_scoundrel.hints += $"\nTrying and failing to Execute me deals {duperyModConfigCategory.GetEntry<int>("Scoundrel_FailPenalty").Value} {formattedKeyText("Damage")} to you.";



        CharacterData w_dupe_idol = newCharacter("Idol", EAlignment.Evil, ECharacterType.Demon, false, true, "\"Never meet your heroes.\"", "Lover_91302708");
        w_dupe_idol.role = new w_Dupe_Idol();
        w_dupe_idol.description = $"<b>Game Start:</b>\nAll Villagers adjacent to me are Corrupted.\n\nI Lie and Disguise.";


        CharacterData w_dupe_critic = newCharacter("Critic", EAlignment.Evil, ECharacterType.Demon, false, true, "\"Everything is wrong.\nNothing is right.\"", "Architect_39883285");
        w_dupe_critic.role = new w_Dupe_Critic();
        w_dupe_critic.description = $"<b>Game Start:</b>\nReduce your {formattedKeyText("Max Health")} by 5.\n\n<b>On Death:</b>\nRegain 5 {formattedKeyText("Max Health")}\n\nI Lie and Disguise.";


        CharacterData w_dupe_recruiter = newCharacter("Recruiter", EAlignment.Evil, ECharacterType.Demon, false, true, "\"One contract and your life comes crumbling down.\"", "Plague Doctor_49312486");
        w_dupe_recruiter.role = new w_Dupe_Recruiter();
        w_dupe_recruiter.description = $"<b>Game Start:</b>\nAll Outcasts become Evil.\n\nI Lie and Disguise.";











        MelonLogger.Msg($"Doing act order");


        // Characters.Instance.startGameActOrder = InsertAfterAct("Poisoner", w_dupe_bountyhunter);
        Characters.Instance.startGameActOrder = InsertAfterAct("Pooka", w_dupe_idol);
        Characters.Instance.startGameActOrder = InsertAfterAct("Poisoner", w_dupe_poisoner);
        Characters.Instance.startGameActOrder = InsertAfterAct("Shaman", w_dupe_badcop);
        Characters.Instance.startGameActOrder = InsertAfterAct("Chancellor", w_dupe_barkeep);
        Characters.Instance.startGameActOrder = InsertAfterAct("Barkeep", w_dupe_recruiter);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(w_dupe_wannabe);

        /*
        // Vanilla order: Baa, Chancellor, Pooka, Poisoner, Witch, Puppeteer, Plague Doctor, Shaman, Alchemist, Puppet, Lilis

        Characters.Instance.startGameActOrder = InsertAtStartOfActOrder(w_legion);
        Characters.Instance.startGameActOrder = InsertAtStartOfActOrder(w_invertDemon);
        Characters.Instance.startGameActOrder = InsertAtStartOfActOrder(w_cryptid);
        Characters.Instance.startGameActOrder = InsertAtStartOfActOrder(w_fogDemon);
        Characters.Instance.startGameActOrder = InsertAtStartOfActOrder(w_leviathan);
        Characters.Instance.startGameActOrder = InsertAtStartOfActOrder(w_pandemonium);


        Characters.Instance.startGameActOrder = InsertAfterAct("Baa", w_minos);


        Characters.Instance.startGameActOrder = InsertAfterAct("Chancellor", w_swarm_good);
        Characters.Instance.startGameActOrder = InsertAfterAct("Chancellor", w_undying);
        Characters.Instance.startGameActOrder = InsertAfterAct("Chancellor", w_praesect);
        // Characters.Instance.startGameActOrder = insertAfterAct("Chancellor", w_twindemontriplet); // No longer needs to act on start after rework
        Characters.Instance.startGameActOrder = InsertAfterAct("Chancellor", w_mutant);
        Characters.Instance.startGameActOrder = InsertAfterAct("Chancellor", w_marionette);

        //Characters.Instance.startGameActOrder = insertBeforeAct("Pooka", w_politician);
        //Characters.Instance.startGameActOrder = insertBeforeAct("Pooka", w_twindemon);
        //Characters.Instance.startGameActOrder = insertBeforeAct("Pooka", w_saboteur);
        Characters.Instance.startGameActOrder = InsertAfterAct("Pooka", w_snakeCharmer);
        Characters.Instance.startGameActOrder = InsertAfterAct("Pooka", w_saboteur);
        Characters.Instance.startGameActOrder = InsertAfterAct("Pooka", w_twindemontriplet);
        Characters.Instance.startGameActOrder = InsertAfterAct("Pooka", w_twindemon);

        Characters.Instance.startGameActOrder = InsertAfterAct("Shaman", w_acolyte);
        Characters.Instance.startGameActOrder = InsertAfterAct("Shaman", w_fanatic);
        Characters.Instance.startGameActOrder = InsertAfterAct("Shaman", w_zealot);
        Characters.Instance.startGameActOrder = InsertAfterAct("Shaman", w_politician);
        Characters.Instance.startGameActOrder = InsertAfterAct("Shaman", w_iris);

        Characters.Instance.startGameActOrder = InsertAfterAct("Alchemist", w_heretic);
        Characters.Instance.startGameActOrder = InsertAfterAct("Alchemist", w_mezepheles); // This makes it uncurable by Alchemist but it might still have issues with other roles later on.
        Characters.Instance.startGameActOrder = InsertAfterAct("Alchemist", w_twindemontwin);

        //Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(w_illusionist);
        //Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(w_shard);
        // Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(w_pilgrim);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(w_tergiversator);
        //Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(w_wannabe);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(w_copycat);
        //Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(w_devout);
        */

        MelonLogger.Msg($"Act order done.");

        /*
        foreach (CharacterData character in Characters.Instance.startGameActOrder)
        {
            MelonLogger.Msg($"Act Order: {character.characterName}");
        }
        */



        // New act order: Pandemonium, Tenecaligo, Baa, Sanguitaurus, Chancellor, Mutant, Praesect, Undying, Good Swarm, Marionette, Politician, Veniyon, Saboteur, Pooka, Poisoner, Witch, Puppeteer, Plague Doctor, Shaman, Agmeres, Zealot, Fanatic, Acolyte, Vidiyon, Venelum, Copycat, Emenverax, Specularus, Pilgrim, Mendaverte, Devout.







        /*
        foreach (CharacterData character in Characters.Instance.startGameActOrder)
        {
            MelonLogger.Msg($"Game Start order: {character.name.ToString()}");
        }
        */














        bool largerVillages = duperyModConfigCategory.GetEntry<bool>("EnableLargeVillages").Value;



        MelonLogger.Msg($"Preparing scripts");


        Il2CppSystem.Collections.Generic.List<CharacterData> emptyCharacterDataList = new Il2CppSystem.Collections.Generic.List<CharacterData>();


        
        CustomScriptData duperyScriptData = new CustomScriptData();
        duperyScriptData.name = "Dupery_1";
        ScriptInfo duperyScript = new ScriptInfo();
        Il2CppSystem.Collections.Generic.List<CharacterData> duperyList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        duperyList.Add(w_dupe_idol);
        duperyList.Add(w_dupe_critic);
        duperyList.Add(w_dupe_recruiter);
        duperyScript.startingDemons = duperyList;
        duperyScript.startingTownsfolks = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingTownsfolks;
        duperyScript.startingOutsiders = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingOutsiders;
        duperyScript.startingMinions = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingMinions;
        Il2CppSystem.Collections.Generic.List<CharactersCount> duperyCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();


        // 8 characters (13 total)
        duperyCounterList = addCharacterCount(setCharacterCount(3, 3, 1, 1), duperyCounterList, 1);
        duperyCounterList = addCharacterCount(setCharacterCount(4, 2, 1, 1), duperyCounterList, 5);
        duperyCounterList = addCharacterCount(setCharacterCount(4, 1, 2, 1), duperyCounterList, 5);
        duperyCounterList = addCharacterCount(setCharacterCount(4, 0, 3, 1), duperyCounterList, 2);

        // 9 characters (16 total)
        duperyCounterList = addCharacterCount(setCharacterCount(4, 1, 3, 1), duperyCounterList, 4);
        duperyCounterList = addCharacterCount(setCharacterCount(4, 2, 2, 1), duperyCounterList, 8);
        duperyCounterList = addCharacterCount(setCharacterCount(4, 3, 1, 1), duperyCounterList, 4);

        // 10 characters (23 total)
        duperyCounterList = addCharacterCount(setCharacterCount(5, 0, 4, 1), duperyCounterList, 3);
        duperyCounterList = addCharacterCount(setCharacterCount(5, 1, 3, 1), duperyCounterList, 7);
        duperyCounterList = addCharacterCount(setCharacterCount(5, 2, 2, 1), duperyCounterList, 10);
        duperyCounterList = addCharacterCount(setCharacterCount(5, 3, 1, 1), duperyCounterList, 3);

        if (largerVillages)
        {
            // 11 characters (20 total)
            duperyCounterList = addCharacterCount(setCharacterCount(5, 4, 1, 1), duperyCounterList, 1);
            duperyCounterList = addCharacterCount(setCharacterCount(5, 3, 2, 1), duperyCounterList, 8);
            duperyCounterList = addCharacterCount(setCharacterCount(5, 2, 3, 1), duperyCounterList, 8);
            duperyCounterList = addCharacterCount(setCharacterCount(5, 1, 4, 1), duperyCounterList, 3);

            // 12 characters (17 total)
            duperyCounterList = addCharacterCount(setCharacterCount(5, 5, 1, 1), duperyCounterList, 1);
            duperyCounterList = addCharacterCount(setCharacterCount(6, 3, 2, 1), duperyCounterList, 8);
            duperyCounterList = addCharacterCount(setCharacterCount(6, 2, 3, 1), duperyCounterList, 8);
            duperyCounterList = addCharacterCount(setCharacterCount(6, 1, 4, 1), duperyCounterList, 3);

            // 13 characters (14 total)
            duperyCounterList = addCharacterCount(setCharacterCount(6, 4, 2, 1), duperyCounterList, 4);
            duperyCounterList = addCharacterCount(setCharacterCount(6, 3, 3, 1), duperyCounterList, 8);
            duperyCounterList = addCharacterCount(setCharacterCount(6, 2, 4, 1), duperyCounterList, 8);

            // 14 characters (11 total)
            duperyCounterList = addCharacterCount(setCharacterCount(7, 4, 2, 1), duperyCounterList, 6);
            duperyCounterList = addCharacterCount(setCharacterCount(7, 3, 3, 1), duperyCounterList, 8);
            duperyCounterList = addCharacterCount(setCharacterCount(7, 2, 4, 1), duperyCounterList, 6);

            // 15 characters (8 total)
            duperyCounterList = addCharacterCount(setCharacterCount(7, 4, 4, 1), duperyCounterList, 2);
            duperyCounterList = addCharacterCount(setCharacterCount(8, 2, 5, 1), duperyCounterList, 1);
            duperyCounterList = addCharacterCount(setCharacterCount(8, 3, 4, 1), duperyCounterList, 2);
            duperyCounterList = addCharacterCount(setCharacterCount(8, 4, 3, 1), duperyCounterList, 2);
            duperyCounterList = addCharacterCount(setCharacterCount(8, 5, 2, 1), duperyCounterList, 1);

            // 16 characters (5 total)
            duperyCounterList = addCharacterCount(setCharacterCount(8, 3, 4, 1), duperyCounterList, 3);
            duperyCounterList = addCharacterCount(setCharacterCount(8, 4, 3, 1), duperyCounterList, 2);
        }


        duperyScript.characterCounts = duperyCounterList;
        duperyScriptData.scriptInfo = duperyScript;
        

        MelonLogger.Msg($"Adding scripts");
        AscensionsData advancedAscension = ProjectContext.Instance.gameData.advancedAscension;
        w_addDemonRole(advancedAscension, w_dupe_idol, "Baa_Difficult", "Dupery_1", duperyScriptData, emptyCharacterDataList, duperyModConfigCategory.GetEntry<int>("Traitor_Weight").Value);



        for (int i = 0; i < 100; i++)
        {
            //w_addDemonRole(advancedAscension, w_pandemonium, "Baa_Difficult", "Pandemonium_1", pandemoniumScriptData, emptyCharacterDataList);
        }


       


        MelonLogger.Msg($"Adding roles to scripts");
        Il2CppSystem.Collections.Generic.List<string> displayedScripts = new Il2CppSystem.Collections.Generic.List<string>();
        foreach (CustomScriptData scriptData in advancedAscension.possibleScriptsData)
        {
            ScriptInfo script = scriptData.scriptInfo;
            if (!displayedScripts.Contains(scriptData.name))
            {
                MelonLogger.Msg($"Found a script! Name: {scriptData.name}. Compositions:");
                displayedScripts.Add(scriptData.name);
                Il2CppSystem.Collections.Generic.List<string> characterCounts = new Il2CppSystem.Collections.Generic.List<string>();
                foreach (CharactersCount characterCount in script.characterCounts)
                {
                    string charCount = $"{characterCount.town}/{characterCount.outs}/{characterCount.minion}/{characterCount.demon}";
                    if (!characterCounts.Contains(charCount)) MelonLogger.Msg(charCount);
                    characterCounts.Add(charCount);
                }
            }
            addRole(script.startingTownsfolks, w_dupe_mathematician);
            addRole(script.startingTownsfolks, w_dupe_priest);
            addRole(script.startingTownsfolks, w_dupe_privateeye);
            addRole(script.startingTownsfolks, w_dupe_reporter);
            addRole(script.startingTownsfolks, w_dupe_researcher);
            addRole(script.startingTownsfolks, w_dupe_skeptic);
            addRole(script.startingTownsfolks, w_dupe_tailor);
            addRole(script.startingTownsfolks, w_dupe_therapist);
            addRole(script.startingTownsfolks, w_dupe_weatherman);
            addRole(script.startingTownsfolks, w_dupe_vigilante);

            //addRole(script.startingOutsiders, w_dupe_bountyhunter);
            addRole(script.startingOutsiders, w_dupe_copycat);
            addRole(script.startingOutsiders, w_dupe_drunkard);
            addRole(script.startingOutsiders, w_dupe_fallguy);
            addRole(script.startingOutsiders, w_dupe_surgeon);
            addRole(script.startingOutsiders, w_dupe_wannabe);
            addRole(script.startingOutsiders, w_dupe_youngster);

            addRole(script.startingMinions, w_dupe_mobster);
            addRole(script.startingMinions, w_dupe_travelagent);
            addRole(script.startingMinions, w_dupe_serialkiller);
            addRole(script.startingMinions, w_dupe_poisoner);
            addRole(script.startingMinions, w_dupe_badcop);
            addRole(script.startingMinions, w_dupe_barkeep);
            addRole(script.startingMinions, w_dupe_scoundrel);



            for (int i = 0; i < 100; i++)
            {
                //addRoleEvenIfDupe(script.startingTownsfolks, w_dupe_tailor);
                //addRoleEvenIfDupe(script.startingOutsiders, w_dupe_wannabe);
                //addRoleEvenIfDupe(script.startingMinions, w_dupe_scoundrel);
            }
            for (int i = 0; i < allDatas.Length; i++)
            {
                //if (allDatas[i].characterId == "Gambler_42592744")
                //{
                //    script.startingTownsfolks.Remove(allDatas[i]);
                //}
            }
        }


        for (int j = 0; j < advancedAscension.possibleScriptsData.Length; j++)
        {
            Debug.LogWarning(advancedAscension.possibleScriptsData[j].name);
            MelonLogger.Msg($"Script: {advancedAscension.possibleScriptsData[j].name.ToString()}");
        }



        /*
        sharedScripts.DebugMessage("Trying to do jinx list");
        Il2CppSystem.Collections.Generic.List<string> jinxedScripts_TooFewVillagers = new Il2CppSystem.Collections.Generic.List<string>(); // Scripts that don't tend to have enough Villagers for some roles to work.
        Il2CppSystem.Collections.Generic.List<string> jinxedRoles_TooFewVillagers = new Il2CppSystem.Collections.Generic.List<string>(); // Roles that're jinxed with the above scripts, by ID.
        Il2CppSystem.Collections.Generic.List<string> jinxedScripts_TooMuchPoison = new Il2CppSystem.Collections.Generic.List<string>(); // Scripts that tend to have an obnoxious amount of Corruption, so a lot of Corruption characters are redundant.
        Il2CppSystem.Collections.Generic.List<string> jinxedRoles_TooMuchPoison = new Il2CppSystem.Collections.Generic.List<string>(); // Roles that're jinxed with the above scripts, by ID.
        jinxedScripts_TooFewVillagers.Add("Dominion_Small");
        jinxedScripts_TooFewVillagers.Add("Dominion_Large");
        jinxedScripts_TooFewVillagers.Add("Legion_1");
        jinxedRoles_TooFewVillagers.Add("Bishop_58855542"); // Will frequently just stay silent, especially when Lying
        jinxedRoles_TooFewVillagers.Add("Empress_13782227"); // Will frequently just stay silent, especially when Lying
        jinxedRoles_TooFewVillagers.Add("Chatterbox_WING"); // Your info is already bad enough, it doesn't need to be worse.
        jinxedRoles_TooFewVillagers.Add("Marionette_WING"); // Marionette's not really fun in these kinds of villages.
        jinxedRoles_TooFewVillagers.Add("Mutant_WING"); // Same goes for Mutant.
        jinxedRoles_TooFewVillagers.Add("Switchblade_WING"); // There is every possibility that the Switchblade kills the last remaining Good character, triggering a loss.
        jinxedRoles_TooFewVillagers.Add("Wretch_80988916"); // Wretch kind of defeats Agmeres' win condition on its own.
        jinxedRoles_TooFewVillagers.Add("Baron_04539999"); // Already few enough Villagers, we don't need less.
        jinxedRoles_TooFewVillagers.Add("Mezepheles_09511163"); // Already few enough Villagers, we don't need a Puppet.
        // jinxedRoles_TooFewVillagers.Add("Poisoner_64796285"); // Poisoner is a special case, he only poisons his neighbours. I'm *okay* with his presence.
        jinxedRoles_TooFewVillagers.Add("Cryptid_WING"); // Will probably bring in jinxed Minions.
        jinxedRoles_TooFewVillagers.Add("Ritualist_WING"); // You just do not have enough health to reasonably tank a Ritualist.
        jinxedRoles_TooFewVillagers.Add("Saboteur_WING"); // Your info is already bad enough, it doesn't need to be worse.
        jinxedRoles_TooFewVillagers.Add("Snake Charmer_WING"); // Already brutal enough without SC.
        jinxedRoles_TooFewVillagers.Add("Swarm_Good_WING"); // "Hi I'd like all my Villagers replaced with Swarm please"
        // I'm gonna stop now, we only barely have enough Minions for a full-sized Agmeres village (Witch, Minion, Twinion, Poisoner, Shaman, Cryptid, Heretic, Professional)

        jinxedScripts_TooMuchPoison.Add("Mendaverte_1"); // All the Villagers are already Corrupted, further Corruption is pointless. I'm also hoping I can disable Alch this way.
        jinxedRoles_TooMuchPoison.Add("Alchemist_94446803"); // If I'm lucky, this will stop characters from Disguising as the Alchemist and curing everything. Hopefully.
        jinxedRoles_TooMuchPoison.Add("Chatterbox_WING"); // Pointless
        jinxedRoles_TooMuchPoison.Add("Plague Doctor_49312486"); // Special case on this one, since he can actually *help*.
        jinxedRoles_TooMuchPoison.Add("Poisoner_64796285"); // Pointless
        jinxedRoles_TooMuchPoison.Add("Saboteur_WING"); // Pointless



        Il2CppSystem.Collections.Generic.List<string> specificJinxes_Mendaverte = new Il2CppSystem.Collections.Generic.List<string>(); // Roles that're jinxed with Mendaverte specifically.
        specificJinxes_Mendaverte.Add("Lycanthrope_16077432"); // I don't know how to fix this and I don't care to fix it if I can help it.
        specificJinxes_Mendaverte.Add("Mezepheles_09511163"); // Puppet's kinda pointless ngl
        specificJinxes_Mendaverte.Add("Turncoat_WING"); // Pointless
        for (int j = 0; j < advancedAscension.possibleScriptsData.Length; j++)
        {
            if (jinxedScripts_TooFewVillagers.Contains(advancedAscension.possibleScriptsData[j].name))
            {
                sharedScripts.DebugMessage($"Found jinxed script: {advancedAscension.possibleScriptsData[j].name}. Reason: Too few Villagers");
                for (int k = 0; k < advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks.Count; k++)
                {
                    if (jinxedRoles_TooFewVillagers.Contains(advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks[k].characterId))
                    {
                        sharedScripts.DebugMessage($"Removing found jinxed Villager: {advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks[k].characterName}");
                        advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks.RemoveAt(k);
                        k--;
                    }
                }
                sharedScripts.DebugMessage($"Finished jinxing Villagers. New list: {sharedScripts.MentionEveryRoleInList(advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks, "")}");
                for (int k = 0; k < advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders.Count; k++)
                {
                    if (jinxedRoles_TooFewVillagers.Contains(advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders[k].characterId))
                    {
                        sharedScripts.DebugMessage($"Removing found jinxed Outcast: {advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders[k].characterName}");
                        advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders.RemoveAt(k);
                        k--;
                    }
                }
                sharedScripts.DebugMessage($"Finished jinxing Outcasts. New list: {sharedScripts.MentionEveryRoleInList(advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders, "")}");
                for (int k = 0; k < advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions.Count; k++)
                {
                    if (jinxedRoles_TooFewVillagers.Contains(advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions[k].characterId))
                    {
                        sharedScripts.DebugMessage($"Removing found jinxed Minion: {advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions[k].characterName}");
                        advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions.RemoveAt(k);
                        k--;
                    }
                }
                sharedScripts.DebugMessage($"Finished jinxing Minions. New list: {sharedScripts.MentionEveryRoleInList(advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions, "")}");
            }
            if (jinxedScripts_TooMuchPoison.Contains(advancedAscension.possibleScriptsData[j].name))
            {
                sharedScripts.DebugMessage($"Found jinxed script: {advancedAscension.possibleScriptsData[j].name}. Reason: Too much Corruption");
                for (int k = 0; k < advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks.Count; k++)
                {
                    if (jinxedRoles_TooMuchPoison.Contains(advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks[k].characterId))
                    {
                        sharedScripts.DebugMessage($"Removing found jinxed Villager: {advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks[k].characterName}");
                        advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks.RemoveAt(k);
                        k--;
                    }
                }
                sharedScripts.DebugMessage($"Finished jinxing Villagers. New list: {sharedScripts.MentionEveryRoleInList(advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks, "")}");
                for (int k = 0; k < advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders.Count; k++)
                {
                    if (jinxedRoles_TooMuchPoison.Contains(advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders[k].characterId))
                    {
                        sharedScripts.DebugMessage($"Removing found jinxed Outcast: {advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders[k].characterName}");
                        advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders.RemoveAt(k);
                        k--;
                    }
                }
                sharedScripts.DebugMessage($"Finished jinxing Outcasts. New list: {sharedScripts.MentionEveryRoleInList(advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders, "")}");
                for (int k = 0; k < advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions.Count; k++)
                {
                    if (jinxedRoles_TooMuchPoison.Contains(advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions[k].characterId))
                    {
                        sharedScripts.DebugMessage($"Removing found jinxed Minion: {advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions[k].characterName}");
                        advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions.RemoveAt(k);
                        k--;
                    }
                }
                sharedScripts.DebugMessage($"Finished jinxing Minions. New list: {sharedScripts.MentionEveryRoleInList(advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions, "")}");
            }
            if (advancedAscension.possibleScriptsData[j].name == "Mendaverte_1")
            {
                sharedScripts.DebugMessage($"Found jinxed script: {advancedAscension.possibleScriptsData[j].name}. Reason: Mendaverte");
                for (int k = 0; k < advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks.Count; k++)
                {
                    if (specificJinxes_Mendaverte.Contains(advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks[k].characterId))
                    {
                        sharedScripts.DebugMessage($"Removing found jinxed Villager: {advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks[k].characterName}");
                        advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks.RemoveAt(k);
                        k--;
                    }
                }
                sharedScripts.DebugMessage($"Finished jinxing Villagers. New list: {sharedScripts.MentionEveryRoleInList(advancedAscension.possibleScriptsData[j].scriptInfo.startingTownsfolks, "")}");
                for (int k = 0; k < advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders.Count; k++)
                {
                    if (specificJinxes_Mendaverte.Contains(advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders[k].characterId))
                    {
                        sharedScripts.DebugMessage($"Removing found jinxed Outcast: {advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders[k].characterName}");
                        advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders.RemoveAt(k);
                        k--;
                    }
                }
                sharedScripts.DebugMessage($"Finished jinxing Outcasts. New list: {sharedScripts.MentionEveryRoleInList(advancedAscension.possibleScriptsData[j].scriptInfo.startingOutsiders, "")}");
                for (int k = 0; k < advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions.Count; k++)
                {
                    if (specificJinxes_Mendaverte.Contains(advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions[k].characterId))
                    {
                        sharedScripts.DebugMessage($"Removing found jinxed Minion: {advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions[k].characterName}");
                        advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions.RemoveAt(k);
                        k--;
                    }
                }
                sharedScripts.DebugMessage($"Finished jinxing Minions. New list: {sharedScripts.MentionEveryRoleInList(advancedAscension.possibleScriptsData[j].scriptInfo.startingMinions, "")}");
            }
        }
        sharedScripts.DebugMessage("Finished jinxing scripts");
        */
        // Thought I was doing something, but this seems to be removing the relevant roles from *every* script.
    }
    // By the vanilla rule of one demon per village.


    public CharactersCount setCharacterCount(int Villagers, int Outcasts, int Minions, int Demons)
    {
        CharactersCount myCharacterCount = new CharactersCount(Villagers + Outcasts + Minions + Demons, Villagers, Demons, Outcasts, Minions);
        myCharacterCount.dOuts = Outcasts + 1;
        return myCharacterCount;
    }


    public CharactersCount setCharacterCountByVillageSize(int size, int Outcasts, int Minions, int Demons)
    {
        int villagers = (size - Outcasts - Minions - Demons);
        CharactersCount myCharacterCount = new CharactersCount(size, villagers, Demons, Outcasts, Minions);
        myCharacterCount.dOuts = Outcasts + 1;
        return myCharacterCount;
    }
    public Il2CppSystem.Collections.Generic.List<CharactersCount> addCharacterCount(CharactersCount characterCount, Il2CppSystem.Collections.Generic.List<CharactersCount> addList, int weight)
    {
        Il2CppSystem.Collections.Generic.List<CharactersCount> returnList = addList;
        for (int i = 0; i < weight; i++)
        {
            returnList.Add(characterCount);
        }
        return returnList;
    }

    public void w_addDemonRole(AscensionsData advancedAscension, CharacterData? data, string oldScriptName, string newScriptName, CustomScriptData w_NewScript, Il2CppSystem.Collections.Generic.List<CharacterData> jinxList, int configAmount)
    {
        if (data == null)
        {
            return;
        }
        if (configAmount == 0)
        {
            return;
        }
        foreach (CustomScriptData scriptData in advancedAscension.possibleScriptsData)
        {
            if (scriptData.name == oldScriptName)
            {
                CustomScriptData newScriptData = GameObject.Instantiate(scriptData);
                newScriptData.name = newScriptName;
                ScriptInfo newScript = new ScriptInfo();
                ScriptInfo script = w_NewScript.scriptInfo;
                newScriptData.scriptInfo = newScript;
                newScript.startingTownsfolks = script.startingTownsfolks;
                newScript.startingOutsiders = script.startingOutsiders;
                newScript.startingMinions = script.startingMinions;
                newScript.startingDemons = script.startingDemons;
                newScript.characterCounts = w_NewScript.scriptInfo.characterCounts;
                //newScript.startingDemons = new Il2CppSystem.Collections.Generic.List<CharacterData>();
                //newScript.startingDemons.Add(data);
                var newPSD = advancedAscension.possibleScriptsData.Append(newScriptData);
                if (configAmount != 1)
                {
                    for (int i = 0; i < configAmount - 1; i++)
                    {
                        newPSD = newPSD.Append(newScriptData);
                    }
                }
                advancedAscension.possibleScriptsData = newPSD.ToArray();
                return;
            }
        }
    }
    public void addCharacterDataToList(string ID, List<CharacterData> Characters)
    {
        foreach (CharacterData targetChar in Gameplay.Instance.GetAllAscensionCharacters())
        {
            if (targetChar.characterId == ID)
            {
                Characters.Append(targetChar);
            }
        }
    }
    public void replaceScriptChars(List<CharacterData> Characters, CustomScriptData w_TargetScript)
    {
        w_TargetScript.scriptInfo.startingTownsfolks.Clear();
        w_TargetScript.scriptInfo.startingOutsiders.Clear();
        w_TargetScript.scriptInfo.startingMinions.Clear();
        w_TargetScript.scriptInfo.startingDemons.Clear();
        foreach (CharacterData targetChar in Characters)
        {
            if (targetChar.type == ECharacterType.Villager)
            {
                w_TargetScript.scriptInfo.startingTownsfolks.Add(targetChar);
            }
            if (targetChar.type == ECharacterType.Outcast)
            {
                w_TargetScript.scriptInfo.startingOutsiders.Add(targetChar);
            }
            if (targetChar.type == ECharacterType.Minion)
            {
                w_TargetScript.scriptInfo.startingMinions.Add(targetChar);
            }
            if (targetChar.type == ECharacterType.Demon)
            {
                w_TargetScript.scriptInfo.startingDemons.Add(targetChar);
            }
        }
    }
    public void addRole(Il2CppSystem.Collections.Generic.List<CharacterData> list, CharacterData data)
    {
        if (list.Contains(data))
        {
            return;
        }
        list.Add(data);
    }
    public void addRoleEvenIfDupe(Il2CppSystem.Collections.Generic.List<CharacterData> list, CharacterData data)
    {
        list.Add(data);
    }
    public void addRoleIfNotJinxed(Il2CppSystem.Collections.Generic.List<CharacterData> list, CharacterData data, Il2CppSystem.Collections.Generic.List<CharacterData> jinxList, Il2CppSystem.Collections.Generic.List<CharacterData> jinxCheckList)
    {
        if (list.Contains(data))
        {
            return;
        }
        bool jinxed = false;
        foreach (CharacterData character in jinxList)
        {
            foreach (CharacterData character2 in jinxCheckList)
            {
                if (character2 == character)
                {
                    jinxed = true;
                }
            }
        }
        if (jinxed) return;
        list.Add(data);
    }
    public CharacterData[] allDatas = System.Array.Empty<CharacterData>();
    public override void OnUpdate()
    {
        if (allDatas.Length == 0)
        {
            var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
            if (loadedCharList != null)
            {
                allDatas = new CharacterData[loadedCharList.Length];
                for (int i = 0; i < loadedCharList.Length; i++)
                {
                    allDatas[i] = loadedCharList[i]!.Cast<CharacterData>();
                }
            }
        }
        if (Statics.charactersArray.Length == 0)
        {
            var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
            if (loadedCharList != null)
            {
                Statics.charactersArray = new CharacterData[loadedCharList.Length];
                for (int i = 0; i < loadedCharList.Length; i++)
                {
                    CharacterData data = loadedCharList[i]!.Cast<CharacterData>();
                    Statics.CheckAddRole(data);
                    Statics.charactersArray[i] = data;
                }
            }
            if (Statics.charactersArray.Length > 0)
            {
                this.OnFirstUpdate();
            }
        }
    }
    public CharacterData[] InsertAfterAct(string previous, CharacterData data)
    {
        MelonLogger.Msg($"Adding {data.name.ToString()} after {previous}");
        CharacterData[] actList = Characters.Instance.startGameActOrder;

        int actSize = actList.Length;
        CharacterData[] newActList = new CharacterData[actSize + 1];
        bool inserted = false;
        for (int i = 0; i < actSize; i++)
        {
            if (inserted)
            {
                newActList[i + 1] = actList[i];
            }
            else
            {
                if (actList[i] != null)
                {
                    newActList[i] = actList[i];
                    if (actList[i].name == previous)
                    {
                        newActList[i + 1] = data;
                        inserted = true;
                    }
                }
            }
        }
        if (!inserted)
        {
            LoggerInstance.Msg("");
        }
        return newActList;
    }
    public CharacterData[] InsertAtStartOfActOrder(CharacterData data)
    {
        MelonLogger.Msg($"Adding {data.name.ToString()} to start of act order");
        CharacterData[] actList = Characters.Instance.startGameActOrder;
        int actSize = actList.Length;
        CharacterData[] newActList = new CharacterData[actSize + 1];
        for (int i = 0; i < actSize; i++)
        {
            newActList[i + 1] = actList[i];
        }
        newActList[0] = data;
        return newActList;
    }
    public CharacterData[] InsertAtEndOfActOrder(CharacterData data)
    {
        MelonLogger.Msg($"Adding {data.name.ToString()} to end of act order");
        CharacterData[] actList = Characters.Instance.startGameActOrder;
        int actSize = actList.Length;
        CharacterData[] newActList = new CharacterData[actSize + 1];
        for (int i = 0; i < actSize; i++)
        {
            newActList[i] = actList[i];
        }
        newActList[actSize] = data;
        return newActList;
    }
    public CharacterData[] insertBeforeAct(string next, CharacterData data)
    {
        MelonLogger.Msg($"insertBeforeAct called adding {data.name.ToString()} before {next}");
        int actSize = Characters.Instance.startGameActOrder.Length;
        Il2CppSystem.Collections.Generic.List<CharacterData> newActList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        bool added = false;
        foreach (CharacterData character in Characters.Instance.startGameActOrder)
        {
            MelonLogger.Msg($"Attempting to add {character.name.ToString()} to act order");
            if (character.name.ToString() == next) MelonLogger.Msg($"Found target {character.name.ToString()}");
            if (character.name.ToString() == next && added == false)
            {
                MelonLogger.Msg($"Adding target {data.name.ToString()} to newActList");
                newActList.Add(data);
                MelonLogger.Msg($"Added {data.name.ToString()} to newActList");
            }
            MelonLogger.Msg($"Adding {character.name.ToString()} to newActList");
            newActList.Add(character);
        }
        CharacterData[] newActArray = new CharacterData[actSize + 1];
        int counter = 0;
        MelonLogger.Msg($"Beginning loop");
        foreach (CharacterData character in newActList)
        {
            Debug.Log(string.Format("Adding {0} to act order at array position {1}", character.name.ToString(), counter));
            newActArray[counter] = character;
            counter += 1;
        }
        return newActArray;
    }
    public static Il2CppSystem.Collections.Generic.List<CharacterData> JinxCharacter(Il2CppSystem.Collections.Generic.List<CharacterData> inputList, string ID)
    {
        Il2CppSystem.Collections.Generic.List<CharacterData> outputList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        foreach (CharacterData character in inputList)
        {
            if (character.characterId != ID)
            {
                outputList.Add(character);
            }
        }
        return outputList;
    }
    public void OnFirstUpdate()
    {
        PatchVanillaCharacterDescriptions();

        wx_SavedScripts sharedScripts = new wx_SavedScripts();
        sharedScripts.DebugMessage("Description patcher finished, looking for chars transform");
        Transform chars = GameObject.Find("Game/Gameplay/Content/Canvas/Panel/Characters").transform;
        if (chars)
        {
            sharedScripts.DebugMessage("Found chars transform");
        }
        else
        {
            sharedScripts.DebugMessage("Didn't find chars transform, expect an error");
        }
        for (int i = 12; i < 50; i++)
        {
            Statics.checkCreateCircle(chars, i);
        }
        for (int j = 2; j < 5; j++)
        {
            Statics.checkCreateCircle(chars, j);
        }

    }

    public static class HiddenRoleStatus
    {
        public static ECharacterStatus hiddenRole = (ECharacterStatus)999;
    }

    string customHint(string type, string parameter)
    {
        string hint = "Custom hint not working, please report to Wingidon";
        if (type == "Ability Refresh Hint")
        {
            if (parameter == "Each Night")
            {
                hint = "My ability refreshes each night and may be used again each day.";
            }
            if (parameter == "Once Per Game")
            {
                hint = "My ability does not refresh each night.";
            }
        }
        if (type == "Outcast Disguise Hint")
        {
            if (parameter == "Simple")
            {
                hint = "My Disguise choice follows standard Minion Disguise rules.";
            }
            if (parameter == "Advanced")
            {
                hint = "My Disguise choice follows standard Minion Disguise rules.\nThis means I may Disguise as an in-play or out-of-play character, and may even Disguise as another face-up Outcast.";
            }
        }
        if (type == "Interactions")
        {
            if (parameter == "Clone Evil")
            {
                hint = $"Due to the possibility of there being multiple of me, certain characters may have weird interactions with us.\nThe {roleColour("Villager")}Scout</color> may give info that's correct for one of us, but wrong for the other. This applies to Lying {roleColour("Villager")}Scouts</color> too.";
            }
            if (parameter == "Good Swarm")
            {
                hint = $"I am a Good Minion. As a result of this, a Lying {roleColour("Villager")}Oracle</color> may occasionally yield true info about me due to the way her Lying logic works.\nI can also be the other half of a Truthful {roleColour("Villager")}Oracle</color> ping on another Evil, including Evil {roleColour("Minion")}Swarm</color>.";
            }
            if (parameter == "Good Minion")
            {
                hint = $"I am a Good Minion. As a result of this, a Lying {roleColour("Villager")}Oracle</color> may occasionally yield true info about me due to the way her Lying logic works.\nI can also be the other half of a Truthful {roleColour("Villager")}Oracle</color> ping on another Evil.";
            }
        }
        if (type == "Keyword")
        {
            if (parameter == "Setup")
            {
                hint = $"<b>Setup:</b>\nThis ability applies <i>before</i> <b>Game Start</b> abilities. It only works if the current Demon is the primary Demon of the current board.\nThese effects are reflected in the role counts.";
            }
            if (parameter == "Bluff")
            {
                hint = $"<b>Bluff</b>:\nCharacters think I have the attribute that I am {formattedKeyText("Bluffing")}.";
            }
            if (parameter == "Poison")
            {
                hint = $"<b>Poison</b>:\nThis character is Corrupted & acts as such.\nAfter a certain number of {formattedKeyText("Reveals")}, they die.\nThe {roleColour("Villager")}Alchemist</color> can Cure the Corruption, but can't stop the {formattedKeyText("Poison")} from killing the victim.";
            }
            if (parameter == "Cycle")
            {
                hint = $"<b>Cycle X</b>:\nThis ability happens every X times any character is {formattedKeyText("Revealed")}.";
            }
            if (parameter == "TrustLong")
            {
                hint = $"<b>Trust</b>:\nA measure of how much you can {formattedKeyText("Trust")} a character.\nVillagers, Outcasts, Minions and Demons are 5x, 3x, 3x and 1x as {formattedKeyText("Trustworthy")} respectively.\nGood characters are 3x as {formattedKeyText("Trustworthy")}.\n{formattedKeyText("Truthful")} characters are 3x as {formattedKeyText("Trustworthy")}.\n{formattedKeyText("Honest")} characters are 2.5x as {formattedKeyText("Trustworthy")}.";
            }
            if (parameter == "TrustShort")
            {
                hint = $"<b>Trust</b>:\nA measure of how much you can {formattedKeyText("Trust")} a character.\nGenerally speaking, the more innocent traits a character exhibits, the more {formattedKeyText("Trustworthy")} they are.";
            }
            if (parameter == "Declare")
            {
                hint = $"<b>Declare</b>:\nThis character makes a statement that is always true, even if they're Lying.";
            }
        }
        return hint;
    }


    public static CharacterData newCharacter(string name, EAlignment alignment, ECharacterType type, bool bluffable, bool usuallyDisguised, string flavour, string placeholderArtID)
    {



        Il2CppSystem.Collections.Generic.List<string> refIDs = new Il2CppSystem.Collections.Generic.List<string>();
        refIDs = GetRolePlaceholderArt(type, alignment);
        MelonLogger.Msg($"refIDs[0] = {refIDs[0]}");
        MelonLogger.Msg($"refIDs[1] = {refIDs[1]}");
        CharacterData backgroundRef = ProjectContext.Instance.gameData.GetCharacterDataOfId(refIDs[0]);
        CharacterData artRef = ProjectContext.Instance.gameData.GetCharacterDataOfId(placeholderArtID);
        if (artRef == null) artRef = ProjectContext.Instance.gameData.GetCharacterDataOfId(refIDs[1]);
        if (backgroundRef == null)
        {
            MelonLogger.Msg("backgroundRef is null! Resetting to Bishop...");
            backgroundRef = ProjectContext.Instance.gameData.GetCharacterDataOfId("Bishop_58855542");
        }
        if (artRef == null)
        {
            MelonLogger.Msg("artRef is null! Resetting to Bishop...");
            artRef = ProjectContext.Instance.gameData.GetCharacterDataOfId("Bishop_58855542");
        }
        MelonLogger.Msg($"backgroundRef = {backgroundRef.characterName}");
        MelonLogger.Msg($"artRef = {artRef.characterName}");




        CharacterData newCharacter = new CharacterData();
        //CharacterData bishopData = new CharacterData();
        //bishopData = ProjectContext.Instance.gameData.GetCharacterDataOfId("Bishop_58855542");
        //newCharacter.art = bishopData.art;
        //newCharacter.backgroundArt = bishopData.backgroundArt;
        //newCharacter.roguelikeInfo = bishopData.roguelikeInfo;

        MelonLogger.Msg("");
        MelonLogger.Msg($"Creating role {name} of type {type} and alignment {alignment}.");
        MelonLogger.Msg($"Name: {name}");
        newCharacter.name = name;
        newCharacter.characterName = name;
        MelonLogger.Msg($"Setting base desc...");
        newCharacter.description = "";
        MelonLogger.Msg($"Flavour: {flavour}");
        newCharacter.flavorText = flavour;
        newCharacter.hints = "";
        newCharacter.ifLies = "";
        newCharacter.picking = false;
        MelonLogger.Msg($"Alignment: {alignment.ToString()}");
        newCharacter.startingAlignment = alignment;
        MelonLogger.Msg($"Type: {type.ToString()}");
        newCharacter.type = type;
        MelonLogger.Msg($"Bluffable?: {bluffable.ToString()}");
        newCharacter.bluffable = bluffable;
        newCharacter.characterId = $"WING_Dupery_{name}";
        newCharacter.artBgColor = getColour(type, alignment, "artBgColor");
        newCharacter.cardBgColor = getColour(type, alignment, "cardBgColor");
        newCharacter.cardBorderColor = getColour(type, alignment, "cardBorderColor");
        newCharacter.color = getColour(type, alignment, "color");
        MelonLogger.Msg($"Finished getting colours.");
        MelonLogger.Msg($"Usually Disguised?: {usuallyDisguised.ToString()}");
        newCharacter.usuallyDisguised = usuallyDisguised;
        newCharacter.additionalFlavorTexts = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStringArray(1);
        newCharacter.additionalFlavorTexts[0] = flavour;
        newCharacter.gender = EGender.They;

        newCharacter.bundledCharacters = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        newCharacter.additionalPossibleCharacters = new AddedCharacterTypes();

        newCharacter.art_cute = artRef.art_cute;
        newCharacter.backgroundArt = backgroundRef.backgroundArt;

        newCharacter.localization_key = $"WINGMOD_{name}";

        return newCharacter;
    }

    public static CharacterCount NewPossibleCharacterCount(ECharacterType type, int amount)
    {
        CharacterCount returnVal = new CharacterCount();
        returnVal.type = type;
        returnVal.count = amount;
        return returnVal;
    }

    public static Il2CppSystem.Collections.Generic.List<string> GetRolePlaceholderArt(ECharacterType type, EAlignment alignment) // First item of the list is the background, second is the art.
    {
        Il2CppSystem.Collections.Generic.List<string> returnList = new Il2CppSystem.Collections.Generic.List<string>();
        if (alignment == EAlignment.Good)
        {
            returnList.Add("Bishop_58855542");
        }
        else
        {
            returnList.Add("Minion_71804875");
        }
        if (type == ECharacterType.Villager)
        {
            if (alignment == EAlignment.Good)
            {
                returnList.Add("Knight_47970624"); // Good Villager: Knight
            }
            if (alignment == EAlignment.Evil)
            {
                returnList.Add("Gambler_42592744"); // Evil Villager: Slayer
            }
        }
        if (type == ECharacterType.Outcast)
        {
            if (alignment == EAlignment.Good)
            {
                returnList.Add("Wretch_80988916"); // Good Outcast: Wretch
            }
            if (alignment == EAlignment.Evil)
            {
                returnList.Add("Bombardier_79093372"); // Evil Outcast: Bombardier
            }
        }
        if (type == ECharacterType.Minion)
        {
            if (alignment == EAlignment.Good)
            {
                returnList.Add("Witch_25286521"); // Good Minion: Witch
            }
            if (alignment == EAlignment.Evil)
            {
                returnList.Add("Poisoner_64796285"); // Evil Minion: Poisoner
            }
        }
        if (type == ECharacterType.Demon)
        {
            if (alignment == EAlignment.Good)
            {
                returnList.Add("Confessor_18741708"); // Good Demon: Confessor
            }
            if (alignment == EAlignment.Evil)
            {
                returnList.Add("Lillith_90453844"); // Evil Demon: Lilis
            }
        }
        return returnList;
    }

    string roleColour(string type)
    {
        switch (type)
        {
            // Types
            case "Villager": return formattedKeyText("VillagerColour");
            case "Outcast": return formattedKeyText("OutcastColour");
            case "Minion": return formattedKeyText("MinionColour");
            case "Demon": return formattedKeyText("DemonColour");
            case "EvilVillager": return formattedKeyText("EvilVillagerColour");
            case "EvilOutcast": return formattedKeyText("EvilOutcastColour");
            case "GoodMinion": return formattedKeyText("GoodMinionColour");
            case "GoodDemon": return formattedKeyText("GoodDemonColour");

            // Power Play
            case "Weather": return formattedKeyText("WeatherColour");
            case "Neutral": return formattedKeyText("NeutralColour");
        }
        return formattedKeyText("");
    }
    public static Color getColour(ECharacterType type, EAlignment alignment, string field)
    {
        // Type = character type
        // Alignment = character alignment
        // Field = "color" for text colour, "cardBgColor" for card background colour, "cardBorderColor" for the border colour and "artBgColor" for the art background colour.
        // In summary, field = "color", "cardBgColor", "cardBorderColor" or "artBgColor".
        Color returnColour = new Color(0, 0, 0);
        if (field == "artBgColor")
        {
            return getColour(type, alignment, "cardBorderColor");
        }
        if (type == ECharacterType.Villager)
        {
            if (alignment == EAlignment.Good)
            {
                switch (field)
                {
                    // Types
                    case "color": return new Color(1f, 0.9333f, 0.7294f);
                    case "cardBgColor": return new Color(0.2588f, 0.1529f, 0.3411f);
                    case "cardBorderColor": return new Color(0.7137f, 0.3372f, 0.8666f);
                }
            }
            else
            {
                switch (field)
                {
                    // Types
                    case "color": return new Color(0.9098f, 0.7764f, 1f); // E8C6FF
                    case "cardBgColor": return new Color(0.1647f, 0.1058f, 0.2f); // 2A1B33
                    case "cardBorderColor": return new Color(0.6078f, 0.1843f, 0.6823f); // 9B2FAE
                }
            }
        }
        if (type == ECharacterType.Outcast)
        {
            if (alignment == EAlignment.Good)
            {
                switch (field)
                {
                    // Types
                    case "color": return new Color(0.9647f, 1, 0.447f);
                    case "cardBgColor": return new Color(0.1019f, 0.0666f, 0.0392f);
                    case "cardBorderColor": return new Color(0.7843f, 0.6470f, 0);
                }
            }
            else
            {
                switch (field)
                {
                    // Types
                    case "color": return new Color(1, 0.6666f, 0.9568f); // E8C6FF
                    case "cardBgColor": return new Color(0.2509f, 0, 0.2156f); // 2A1B33
                    case "cardBorderColor": return new Color(1, 0, 0.8666f); // FF00DD
                }
            }
        }
        if (type == ECharacterType.Minion)
        {
            if (alignment == EAlignment.Evil)
            {
                switch (field)
                {
                    // Types
                    case "color": return new Color(0.8509f, 0.4549f, 0);
                    case "cardBgColor": return new Color(0.094f, 0.0431f, 0.04313f);
                    case "cardBorderColor": return new Color(0.8196f, 0, 0.0235f);
                }
            }
            else
            {
                switch (field)
                {
                    // Types
                    case "color": return new Color(0.7882f, 1, 0.9490f); // E8C6FF
                    case "cardBgColor": return new Color(0.0588f, 0.1647f, 0.1647f); // 2A1B33
                    case "cardBorderColor": return new Color(0.2f, 0.8196f, 0.7764f); // 33D1C6
                }
            }
        }
        if (type == ECharacterType.Demon)
        {
            if (alignment == EAlignment.Evil)
            {
                switch (field)
                {
                    // Types
                    case "color": return new Color(1, 0.3803f, 0.3803f);
                    case "cardBgColor": return new Color(0.0941f, 0.0431f, 0.0431f);
                    case "cardBorderColor": return new Color(0.8196f, 0, 0.0235f);
                }
            }
            else
            {
                switch (field)
                {
                    // Types
                    case "color": return new Color(1f, 0.9607f, 0.8784f); // E8C6FF
                    case "cardBgColor": return new Color(0.1019f, 0.0588f, 0.1803f); // 2A1B33
                    case "cardBorderColor": return new Color(0.4784f, 0.3607f, 1f); // 7A5CFF
                }
            }
        }
        return returnColour;
    }

    /*
    string characterColour(string character)
    {
        switch (character)
        {
            // Vanilla Villagers
            case "Alchemist": return "<color=#D2F7E4>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";

            // Vanilla Outcasts
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";

            // Vanilla Minions
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";

            // Vanilla Demons
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";

            // WEP Villagers
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";

            // WEP Outcasts
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";

            // WEP Minions
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";

            // WEP Demons
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
            case "": return "<color=#>";
        }
        return formattedKeyText("");
    }
    */
    string formattedKeyText(string target)
    {
        switch (target)
        {
            // Keywords
            case "Honest": return "<color=#7AC6FF>Honest</color>";
            case "Pure": return "<color=#7AFBFF>Pure</color>";
            case "Cure": return "<color=#7AFBFF>Cure</color>";
            case "Cured": return "<color=#7AFBFF>Cured</color>";
            case "Heal": return "<color=#2EFF43>Heal</color>";
            case "Max Health": return "<color=#7AFBFF>Max Health</color>";
            case "Health": return "<color=#7AFBFF>Health</color>";
            case "Damage": return "<color=#C72424>Damage</color>";
            case "True Role": return "<color=#57E69C>True Role</color>";
            case "Truthful": return "<color=#3A95D6>Truthful</color>";
            case "Truth": return "<color=#3A95D6>Truth</color>";
            case "Reveal": return "<color=#A1E6E2>Reveal</color>";
            case "Reveals": return "<color=#A1E6E2>Reveals</color>";
            case "Revealed": return "<color=#A1E6E2>Revealed</color>";
            case "Hidden": return "<color=#697D91>Hidden</color>";
            case "Unrevealed": return "<color=#697D91>Unrevealed</color>";
            case "Bluff": return "<color=#D96EDB>Bluff</color>";
            case "Bluffs": return "<color=#D96EDB>Bluffs</color>";
            case "Bluffing": return "<color=#D96EDB>Bluffing</color>";
            case "Attack": return "<color=#FF0037>Attack</color>";
            case "Attacked": return "<color=#FF0037>Attacked</color>";
            case "Kill": return "<color=#FF0037>Kill</color>";
            case "Killed": return "<color=#FF0037>Killed</color>";
            case "Killing": return "<color=#FF0037>Killing</color>";
            case "Dead": return "<color=#B36979>Dead</color>";
            case "Die": return "<color=#B36979>Die</color>";
            case "Dies": return "<color=#B36979>Dies</color>";
            case "Alive": return "<color=#A4EDB7>Alive</color>";
            case "Living": return "<color=#A4EDB7>Living</color>";
            case "Deck": return "<color=#789AF0>Deck</color>";
            case "Lose": return "<color=#FF0000>Lose</color>";
            case "Unmask": return "<color=#B5E9FF>Unmask</color>";
            case "Declare": return "<color=#FFFF00>Declare</color>";
            // case "Alignment": return "<color=#99FF99>Align</color><color=#FF9999>ment</color>"; // Making an alternate one for Alignment

            // Cycle is gonna be a long one because of the fancy gradient I'm doing
            case "Cycle": return "<color=#99ff99>C</color><color=#99e6b3>y</color><color=#99cccc>c</color><color=#99b3e6>l</color><color=#9999ff>e</color>";
            case "Cycle 1": return "<color=#99ff99>C</color><color=#99e6b3>y</color><color=#99cccc>c</color><color=#99b3e6>l</color><color=#9999ff>e 1</color>";
            case "Cycle 2": return "<color=#99ff99>C</color><color=#99e6b3>y</color><color=#99cccc>c</color><color=#99b3e6>l</color><color=#9999ff>e 2</color>";
            case "Cycle 3": return "<color=#99ff99>C</color><color=#99e6b3>y</color><color=#99cccc>c</color><color=#99b3e6>l</color><color=#9999ff>e 3</color>";
            case "Cycle 4": return "<color=#99ff99>C</color><color=#99e6b3>y</color><color=#99cccc>c</color><color=#99b3e6>l</color><color=#9999ff>e 4</color>";
            case "Cycle 5": return "<color=#99ff99>C</color><color=#99e6b3>y</color><color=#99cccc>c</color><color=#99b3e6>l</color><color=#9999ff>e 5</color>";
            case "Cycle 6": return "<color=#99ff99>C</color><color=#99e6b3>y</color><color=#99cccc>c</color><color=#99b3e6>l</color><color=#9999ff>e 6</color>"; // Cycles beyond 6 are pointless

            // I'm doing gradients or multicolours for these, so they'll end up being fairly long.
            case "Alignment": return "<color=#99ff99>A</color><color=#b7f382>l</color><color=#cfe573>i</color><color=#e3d76c>g</color><color=#f2c96d>n</color><color=#fdba73>m</color><color=#ffad7e>e</color><color=#ffa28b>n</color><color=#ff9999>t</color>";
            case "Type": return "<color=#B656DD>T</color><color=#C8A500>y</color><color=#D97400>p</color><color=#FF6161>e</color>";
            case "Truthfulness": return "<color=#3a95d6>T</color><color=#0ca3da>r</color><color=#00b1da>u</color><color=#00bdd5>t</color><color=#00c9ce>h</color><color=#25d4c4>f</color><color=#51deb8>u</color><color=#76e7ad>l</color><color=#98efa3>n</color><color=#bbf69b>e</color><color=#ddfb98>s</color><color=#ffff99>s</color>";
            case "Honesty": return "<color=#7ac6ff>H</color><color=#5cd3f2>o</color><color=#5fddd9>n</color><color=#7fe2bc>e</color><color=#aae4a3>s</color><color=#d6e296>t</color><color=#ffdd99>y</color>";
            case "Purity": return "<color=#7afbff>P</color><color=#61ecff>u</color><color=#71daff>r</color><color=#80c8ff>i</color><color=#94b2ff>t</color><color=#b199ff>y</color>";

            // Custom role keywords
            case "Poison": return "<color=#3F8538>Poison</color>"; // For unused Toxomancer role.
            case "Poisoned": return "<color=#3F8538>Poisoned</color>";
            case "Trick": return "<color=#70E8FF>Trick</color>"; // Used by Faerie.
            case "Tricked": return "<color=#70E8FF>Tricked</color>";
            case "Bewildered": return "<color=#70E8FF>Bewil</color><color=#FF00DD>dered</color>"; // Also used by Faerie.
            case "Misled": return "<color=#FF00AE>Misled</color>"; // Used by Venelum and Vidiyon.
            case "Trustworthy": return "<color=#9999FF>Trustworthy</color>"; // Used by Empath
            case "Trustworthiness": return "<color=#9999FF>Trustworthiness</color>";
            case "Trust": return "<color=#9999FF>Trust</color>";


            // Devs
            case "Normandia": return "<color=#CE1119>Normandia</color>";
            case "Uzabi": return "<color=#CE1119>Uzabi</color>";

            // Modders
            case "@wingidon": return "<color=#7289DA>@</color><color=#C080FF>wingidon</color>";
            case "Wingidon": return "<color=#C080FF>Wingidon</color>";
            case "WWW": return "<color=#3BA55C>WWW is not taken</color>";
            case "@WWW": return "<color=#7289DA>@</color><color=#3BA55C>wwwisnottaken</color>";
            case "Carlz": return "<color=#5FC4F9>Carlz</color>";
            case "@Carlz": return "<color=#7289DA>@</color><color=#5FC4F9>carlz54339</color>";

            // Art credits
            case "Blue Cheesed": return "<color=#D8D8D8>Blue Cheesed</color>"; // Arithmetician
            case "@Blue Cheesed": return "<color=#7289DA>@</color><color=#D8D8D8>hydethefish</color>";
            case "WeekendWolf": return "<color=#5476ff>WeekendWolf</color>"; // Forager, Sentinel, Lunatic
            case "@weekendwolf": return "<color=#7289DA>@</color><color=#5476ff>hellzalley</color>";
            case "Astery": return "<color=#d506c7>Astery</color>"; // Gemcrafter
            case "@astery": return "<color=#7289DA>@</color><color=#d506c7>astery__</color>";
            case "LostIllustrator": return "<color=#45e0f8>Lost Illustrator</color>"; // Scavenger
            case "@lostillustrator": return "<color=#7289DA>@</color><color=#45e0f8>lostillustrator</color>";
            case "Hiraeth": return "<color=#4b53d5>Hiraeth</color>"; // Warden
            case "@hiraeth": return "<color=#7289DA>@</color><color=#4b53d5>lullabiesmourn</color>";
            case "Panda": return "<color=#cadee6>Panda</color>"; // Spy
            case "@Panda": return "<color=#7289DA>@</color><color=#cadee6>@pandacharly</color>";
            case "Derpy_Feesh": return "<color=#7948d7>Derpy_Feesh</color>"; // Leviathan
            case "@derpy_feesh": return "<color=#7289DA>@</color><color=#7948d7>derpy_feesh</color>"; // Leviathan
            case "Cycler": return "<color=#45E0F8>Cycler</color>"; // Cycler
            case "@skillcycler": return "<color=#7289DA>@</color><color=#45E0F8>skillcycler</color>"; // Cycler
            case "LimeOn": return "<color=#7289DA>@</color><color=#94EECC>LimeOn</color>"; // Empath
            case "@limeon": return "<color=#7289DA>@</color><color=#94EECC>lime_0n1337</color>"; // Empath

            // Special thanks
            case "NoLucksGiven": return "<color=#FFC07B>NoLucksGiven</color>"; // Played mod on YouTube, brought attention to it.
            case "D_NoLucksGiven": return "<color=#7289DA>@</color><color=#FFC07B>nolucksgiven</color>";
            case "Y_NoLucksGiven": return $"<color=#FFC07B>https://www.{formattedKeyText("YouTube")}.com/c/NoLucksGiven</color>";
            case "Fi": return "<color=#96EAFF>Fi the Dragonfly</color>"; // Faerie character is literally Fi lmao
            case "@fithedragonfly": return "<color=#96EAFF>@fithedragonfly</color>";

            // Colours
            case "VillagerColour": return "<color=#B656DD>";
            case "VillagerAltColour": return "<color=#C080FF>";
            case "OutcastColour": return "<color=#F6FF72>";
            case "OutcastAltColour": return "<color=#C8A500>";
            case "MinionColour": return "<color=#D97400>";
            case "DemonColour": return "<color=#FF6161>";

            // Colours, Alignment Flip
            case "EvilVillagerColour": return "<color=#9B2FAE>";
            case "EvilOutcastColour": return "<color=#FF00DD>";
            case "GoodMinionColour": return "<color=#33D1C6>";
            case "GoodDemonColour": return "<color=#7A5CFF>";

            // Colours, Other Mods
            case "WeatherColour": return "<color=#FF7AE0>"; // Weather (Power Play)
            case "NeutralColour": return "<color=#8FA7B3>"; // Neutral (Power Play)

            // Platforms
            case "Discord": return "<color=#7289DA>Discord</color>";
            case "Tumblr": return "<color=#36465D>Tumblr</color>";
            case "YouTube": return "<color=#FE0000>YouTube</color>";
            case "Youtube": return "<color=#FE0000>YouTube</color>";
        }
        return "Formatted key text invalid, please report this to Wingidon.";
    }


    public void PatchVanillaCharacterDescriptions()
    {
        Il2CppSystem.Collections.Generic.List<string> maleCharacters = new Il2CppSystem.Collections.Generic.List<string>();
        Il2CppSystem.Collections.Generic.List<string> femaleCharacters = new Il2CppSystem.Collections.Generic.List<string>();
        Il2CppSystem.Collections.Generic.List<string> enbyCharacters = new Il2CppSystem.Collections.Generic.List<string>();

        // Most vanilla roles don't have .gender defined, so I'm just gonna correct them here briefly.
        // Genders sourced from http://docs.google.com/document/d/1p36GvJFJBMuST9mfEBzVH1L6V9zcPLe5oESwLZ1hruw/edit?pli=1&tab=t.0
        maleCharacters.Add("Alchemist");
        maleCharacters.Add("Architect");
        enbyCharacters.Add("Baker"); // Original Baker is she/her, but other Bakers are they/them.
        femaleCharacters.Add("Bard");
        femaleCharacters.Add("Bishop");
        femaleCharacters.Add("Confessor");
        femaleCharacters.Add("Dreamer");
        femaleCharacters.Add("Druid");
        femaleCharacters.Add("Empress");
        femaleCharacters.Add("Enlightened");
        femaleCharacters.Add("Fortune Teller");
        femaleCharacters.Add("Gemcrafter");
        maleCharacters.Add("Hunter");
        maleCharacters.Add("Investigator");
        maleCharacters.Add("Jester");
        maleCharacters.Add("Judge");
        maleCharacters.Add("Knight");
        femaleCharacters.Add("Knitter");
        femaleCharacters.Add("Lover");
        femaleCharacters.Add("Medium");
        femaleCharacters.Add("Oracle");
        femaleCharacters.Add("Poet");
        maleCharacters.Add("Scout");
        maleCharacters.Add("Slayer");
        maleCharacters.Add("Witness");

        maleCharacters.Add("Bombardier");
        enbyCharacters.Add("Doppelganger");
        maleCharacters.Add("Drunk");
        maleCharacters.Add("Lycanthrope");
        maleCharacters.Add("Plague Doctor");
        maleCharacters.Add("Rambler");
        enbyCharacters.Add("Wretch");

        maleCharacters.Add("Chancellor");
        maleCharacters.Add("Minion");
        maleCharacters.Add("Poisoner");
        maleCharacters.Add("Puppeteer");
        enbyCharacters.Add("Puppet");
        femaleCharacters.Add("Shaman");
        femaleCharacters.Add("Twin Minion");
        maleCharacters.Add("Werewolf");
        femaleCharacters.Add("Witch");

        maleCharacters.Add("Baa");
        femaleCharacters.Add("Lilis");
        femaleCharacters.Add("Pooka");
        for (int i = 0; i < allDatas.Count(); i++)
        {
            MelonLogger.Msg($"Role Patcher: Found {allDatas[i].name.ToString()}");
            if (allDatas[i].characterId == "Gambler_42592744")
            {
                allDatas[i].role = new w_Dupe_SlayerPatch();
                MelonLogger.Msg($"Patched Slayer.");
            }
            if (maleCharacters.Contains(allDatas[i].characterName)) allDatas[i].gender = EGender.Male;
            if (femaleCharacters.Contains(allDatas[i].characterName)) allDatas[i].gender = EGender.Female;
            if (enbyCharacters.Contains(allDatas[i].characterName)) allDatas[i].gender = EGender.They;
        }
    }



    //int toxomancerPoisonTimer = 0;
    //int toxomancerDeathTimer = 0;


    /*private void OnCharacterRevealed(Character revealed)
    {
        toxomancerPoisonTimer -= 1;
        toxomancerDeathTimer -= 1;
        CharacterData charData = revealed.dataRef;
        Il2CppSystem.Collections.Generic.List<Character> allChars = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);

        int revealCount = 0;
        for (int i = 0; i < allChars.Count; i++)
        {
            if (allChars[i].revealed == true)
            {
                revealCount++;
            }
        }
        if (revealCount == 1)
        {
            toxomancerPoisonTimer = 2;
            toxomancerDeathTimer = 4;
        }

        bool toxomancerInPlay = false;
        Character toxomancer = new Character();
        for (int i = 0; i < allChars.Count; i++)
        {
            if (allChars[i].dataRef.characterId == "Toxomancer_WING" && allChars[i].state != ECharacterState.Dead)
            {
                toxomancerInPlay = true;
                break;
            }
            if (allChars[i].dataRef.characterId == "Toxomancer_WING")
            {
                toxomancer = allChars[i];
            }
        }
        if (toxomancerInPlay)
        {
            if (toxomancerPoisonTimer == 0)
            {
                Il2CppSystem.Collections.Generic.List<Character> possiblePoisonTargets = new Il2CppSystem.Collections.Generic.List<Character>();
                foreach (Character character in allChars)
                {
                    if (character.GetRegisterAs().type == ECharacterType.Villager && character.GetRegisterAlignment() == EAlignment.Good && character.state != ECharacterState.Dead)
                    {
                        possiblePoisonTargets.Add(character);
                    }
                }
                Character poisonTarget = possiblePoisonTargets[UnityEngine.Random.RandomRangeInt(0, possiblePoisonTargets.Count)];
                poisonTarget.statuses.AddStatus(ECharacterStatus.Corrupted, toxomancer);
                poisonTarget.statuses.AddStatus(w_Toxomancer.ToxomancerPoison.toxomancerPoison, toxomancer);
                toxomancerPoisonTimer = 3;
                toxomancerDeathTimer = 2;
            }
        }
        if (toxomancerDeathTimer == 0)
        {
            foreach (Character character in allChars)
            {
                if (character.statuses.Contains(w_Toxomancer.ToxomancerPoison.toxomancerPoison))
                {
                    PlayerController.PlayerInfo.health.Damage(1);
                    character.RevealAllReal();
                    character.KillByDemon(toxomancer);
                }
            }
        }

    }*/

























    /*
    [HarmonyPatch(typeof(Gossip), nameof(Gossip.Act))]
    private static class GetPoetTrueInfo
    {
        private static bool Prefix(Gossip __instance, ETriggerPhase trigger, Character charRef)
        {
            if (trigger != ETriggerPhase.Day) return true;
            if (charRef.bluff)
            {
                if (charRef.bluff.characterId != "Gossip_85354100")
                {
                    return true;
                }
            }
            else if (charRef.dataRef.characterId != "Gossip_85354100")
            {
                return true;
            }
            Il2CppSystem.Collections.Generic.List<Role> infoRoles = new Il2CppSystem.Collections.Generic.List<Role>();
            infoRoles.Add(new Empath());
            infoRoles.Add(new Scout());
            infoRoles.Add(new Investigator());
            infoRoles.Add(new BountyHunter());
            infoRoles.Add(new Lookout());
            infoRoles.Add(new Knitter());
            infoRoles.Add(new Tracker());
            infoRoles.Add(new Shugenja());
            infoRoles.Add(new Noble());
            infoRoles.Add(new Bishop());
            infoRoles.Add(new Archivist());
            infoRoles.Add(new Acrobat2());
            infoRoles.Add(new w_Arithmetician());
            infoRoles.Add(new w_Chiromancer());
            infoRoles.Add(new w_Clairvoyant());
            infoRoles.Add(new w_Detective());
            infoRoles.Add(new w_Introvert());
            infoRoles.Add(new w_Jewelsmith());
            infoRoles.Add(new w_Lamb());
            infoRoles.Add(new w_Prince());
            infoRoles.Add(new w_Ranger());
            infoRoles.Add(new w_Sentinel());
            infoRoles.Add(new w_Sheriff());
            infoRoles.Add(new w_Spy());
            ActedInfo myInfo = infoRoles[UnityEngine.Random.RandomRangeInt(0, infoRoles.Count)].GetInfo(charRef);
            __instance.onActed?.Invoke(myInfo);
            return false;
        }
    }


    [HarmonyPatch(typeof(Gossip), nameof(Gossip.BluffAct))]
    private static class GetPoetFalseInfo
    {
        private static bool Prefix(Gossip __instance, ETriggerPhase trigger, Character charRef)
        {
            if (trigger != ETriggerPhase.Day) return true;
            if (charRef.bluff)
            {
                if (charRef.bluff.characterId != "Gossip_85354100")
                {
                    return true;
                }
            }
            else if (charRef.dataRef.characterId != "Gossip_85354100")
            {
                return true;
            }
            Il2CppSystem.Collections.Generic.List<Role> infoRoles = new Il2CppSystem.Collections.Generic.List<Role>();
            infoRoles.Add(new Empath());
            infoRoles.Add(new Scout());
            infoRoles.Add(new Investigator());
            infoRoles.Add(new BountyHunter());
            infoRoles.Add(new Lookout());
            infoRoles.Add(new Knitter());
            infoRoles.Add(new Tracker());
            infoRoles.Add(new Shugenja());
            infoRoles.Add(new Noble());
            infoRoles.Add(new Bishop());
            infoRoles.Add(new Archivist());
            infoRoles.Add(new Acrobat2());
            infoRoles.Add(new w_Arithmetician());
            infoRoles.Add(new w_Chiromancer());
            infoRoles.Add(new w_Clairvoyant());
            infoRoles.Add(new w_Detective());
            infoRoles.Add(new w_Introvert());
            infoRoles.Add(new w_Jewelsmith());
            infoRoles.Add(new w_Lamb());
            infoRoles.Add(new w_Prince());
            infoRoles.Add(new w_Ranger());
            infoRoles.Add(new w_Sentinel());
            infoRoles.Add(new w_Sheriff());
            infoRoles.Add(new w_Spy());
            ActedInfo myInfo = infoRoles[UnityEngine.Random.RandomRangeInt(0, infoRoles.Count)].GetBluffInfo(charRef);
            __instance.onActed?.Invoke(myInfo);
            return false;
        }
    }
    */


    /* Was causing crashes.
    [HarmonyPatch(typeof(Investigator), nameof(Investigator.BluffAct))]
    private static class GetOracleFalseInfo // Practically identical, save for the fact that it can't see Good Minions. Should fix problems with Good Swarm.
    {
        private static bool Prefix(Gossip __instance, ETriggerPhase trigger, Character charRef)
        {
            if (trigger != ETriggerPhase.Day) return true;
            if (charRef.bluff)
            {
                if (charRef.bluff.characterId != "Oracle_07039445")
                {
                    return true;
                }
            }
            else if (charRef.dataRef.characterId != "Oracle_07039445")
            {
                return true;
            }
            Il2CppSystem.Collections.Generic.List<Character> possibleInfoTargets = new Il2CppSystem.Collections.Generic.List<Character>();
            Il2CppSystem.Collections.Generic.List<Character> infoTargets = new Il2CppSystem.Collections.Generic.List<Character>();
            Il2CppSystem.Collections.Generic.List<CharacterData> deckMinions = Gameplay.Instance.GetScriptCharactersOfType(ECharacterType.Minion);
            CharacterData chosenMinion = new CharacterData();
            if (deckMinions.Count == 0)
            {
                foreach (CharacterData character in Gameplay.Instance.GetAllAscensionCharacters())
                {
                    if (character.type == ECharacterType.Minion)
                    {
                        deckMinions.Add(character);
                    }
                }
            }
            chosenMinion = deckMinions[UnityEngine.Random.RandomRangeInt(0, deckMinions.Count)];
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                if (character.GetRegisterAlignment() == EAlignment.Good && character.GetCharacterType() != ECharacterType.Minion)
                {
                    possibleInfoTargets.Add(character);
                }
            }
            string actInfo = "";
            if (possibleInfoTargets.Count < 2)
            {
                actInfo = "This village confuses me.";
            }
            infoTargets.Add(possibleInfoTargets[UnityEngine.Random.RandomRangeInt(0, possibleInfoTargets.Count)]);
            possibleInfoTargets.Remove(infoTargets[0]);
            infoTargets.Add(possibleInfoTargets[UnityEngine.Random.RandomRangeInt(0, possibleInfoTargets.Count)]);

            if (infoTargets[0].id < infoTargets[1].id)
            {
                actInfo = string.Format("#{0} or #{1} is a {2}", infoTargets[0].id, infoTargets[1].id, chosenMinion.name.ToString());
            }
            else
            {
                actInfo = string.Format("#{0} or #{1} is a {2}", infoTargets[1].id, infoTargets[0].id, chosenMinion.name.ToString());
            }
            ActedInfo myInfo = new ActedInfo(actInfo, infoTargets);
            __instance.onActed?.Invoke(myInfo);
            return false;
        }
    }
    */


    [HarmonyPatch(typeof(ObjectivesUI), nameof(ObjectivesUI.UpdateObjectives))]
    public static class ChangeCounter
    {
        public static void Postfix(ObjectivesUI __instance)
        {
            //bool LilisInPlay = false;
            int minions = Gameplay.CurrentScript.minion;
            int demons = Gameplay.CurrentScript.demon;
            int MaxEvils = minions + demons;
            var deadCharacters = Gameplay.DeadCharacters;
            Il2CppSystem.Collections.Generic.List<Character> allCurrentCharacters = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
            Il2CppSystem.Collections.Generic.List<CharacterData> allCurrentCharactersData = new Il2CppSystem.Collections.Generic.List<CharacterData>(Gameplay.Instance.GetScriptCharacters().Pointer);
            Il2CppSystem.Collections.Generic.List<string> Evils = new();
            //Il2CppSystem.Collections.Generic.List<string> allCurrentCharactersNames;
            //Il2CppSystem.Collections.Generic.List<string> allCurrentCharactersDataNames;

            //allCurrentCharactersNames = sortByName(allCurrentCharacters);
            //allCurrentCharactersDataNames = sortByName(allCurrentCharactersData);






            int minEvilsKilled = 0;
            int maxEvilsKilled = 0;
            int AddedEvils = 0;
            //int AddedEvils1 = 0;
            //int AddedEvils2 = 0;

            foreach (var deadCharacter in deadCharacters)
            {
                if (deadCharacter.alignment == EAlignment.Evil || deadCharacter.statuses.Contains(HiddenRoleStatus.hiddenRole))
                {
                    maxEvilsKilled++;
                    if (!deadCharacter.statuses.Contains(HiddenRoleStatus.hiddenRole))
                    {
                        minEvilsKilled++;
                    }
                }
            }


            //foreach (var character in allCurrentCharacters)
            //{

            //string characterData = allCurrentCharactersData[i].name.ToString();
            //string character;

            /*if (i <= allCurrentCharacters.Count - 1)
            {
               character = allCurrentCharactersNames[i];
            }
            else
            {
                character = "";
            }*/
            //MelonLogger.Msg("Character: " + character.dataRef.name.ToString());

            /*if (character == "Belias" || character == "Mayor" || character == "Good Twin" || character == "Puppeteer" || character == "Hypnotist" || character == "Executioner")
            {

                AddedEvils1++;
            }*/


            //if (character.dataRef.name == "Belias" || character.dataRef.name == "Mayor" || character.dataRef.name == "Good Twin" || character.dataRef.name == "Puppeteer" || character.dataRef.name == "Hypnotist" || character.dataRef.name == "Executioner")
            //{
            //if (Evils.Contains(character.dataRef.name.ToString()))
            //{
            //    AddedEvils++;
            //}

            //else
            //{
            //   Evils.Add(character.dataRef.name.ToString());
            //    AddedEvils++;
            //}

            //}

            //}

            //foreach (var characterData in allCurrentCharactersData)
            //{
            //   if (characterData.name.ToString() == "Hellspawn")
            //        MaxEvils++;
            //    if (characterData.name == "Belias" || characterData.name == "Mayor" || characterData.name == "Good Twin" || characterData.name == "Puppeteer" || characterData.name == "Hypnotist" || characterData.name == "Executioner")
            //    {

            //        if (!Evils.Contains(characterData.name.ToString()))
            //        {
            //            Evils.Add(characterData.name.ToString());
            //            AddedEvils++;
            //        }
            //    }

            //}

            /*if(AddedEvils2 > AddedEvils1)
            {
                AddedEvils = AddedEvils2;
            }

            else
            {
                AddedEvils = AddedEvils1;
            }*/

            //string EvilsKilledText = EvilsKilled.ToString();
            //string MaxEvilsAmount = AddedEvils.ToString();

            //if (MaxEvils < minions + demons)
            // MaxEvils++;
            if (minEvilsKilled == maxEvilsKilled)
            {
                __instance.evilsKilled.text = System.String.Format("<color=grey>Evils killed:</color> <color=red>{0}", minEvilsKilled);
            }
            else
            {
                __instance.evilsKilled.text = System.String.Format("<color=grey>Evils killed:</color> <color=red>{0}-{1}", minEvilsKilled, maxEvilsKilled);
            }


            /* else if(MaxEvils < minions + demons)
             {
                 MaxEvilsText = System.String.Format("<color=red>{0}-{1}", MaxEvils, minions + demons);
             }*/

            //if(LilisInPlay)
            // {
            //    EvilsKilledText = "?";
            // }

            // LilisInPlay = false;

            string minionCountText = "Minions";
            if (minions == 1)
            {
                minionCountText = "Minion";
            }
            string demonCountText = "Demons";
            if (demons == 1)
            {
                demonCountText = "Demon";
            }
            __instance.objective.text = System.String.Format("Find and Execute all Evil Characters<br><color=grey><size=18>(<color=orange>{0}+ {2}</color> and <color=red>{1}+ {3} </color>)", minions, demons, minionCountText, demonCountText);

        }
    }


    public static class Statics
    {
        public static Dictionary<string, CharacterData> roles = new Dictionary<string, CharacterData>();
        public static CharacterData[] charactersArray = Il2CppSystem.Array.Empty<CharacterData>();

        public static void checkCreateCircle(Transform parent, int size)
        {
            string name = "Circle_" + size;
            Transform t = parent.FindChild(name);
            if (t != null)
            {
                MelonLogger.Msg("Object Already exists!: " + name);
                return;
            }
            CreateCircle(size);
        }
        /*
        public static GameObject createCircle(int size) // I'm just gonna wait for WWW to figure this out
        {
            GameObject circle = new GameObject();
            circle.name = "Circle_" + size;
            circle.transform.SetParent(Characters.Instance.gameObject.transform);
            RectTransform rect = circle.AddComponent<RectTransform>();
            CharactersPool circPool = circle.AddComponent<CharactersPool>();
            GameObject circ6 = Characters.Instance.gameObject.transform.Find("Circle_6").gameObject;
            CharactersPool circ6Pool = circ6.GetComponent<CharactersPool>();
            circPool.characterPrefab = circ6Pool.characterPrefab;
            circPool.characters = new Character[0];
            circPool.cardPlaceHolders = new CardPlaceholder[size];
            for (int i = 0; i < size; i++)
            {
                GameObject cardHolder = new GameObject();
                cardHolder.transform.SetParent(circle.transform);
                string name = "CardPlaceholder";
                if (i > 0)
                {
                    name += " (" + i + ")";
                }
                cardHolder.name = name;
                RectTransform cardRect = cardHolder.AddComponent<RectTransform>();
                cardRect.anchoredPosition3D = new Vector3(0f, 0f, 0f);
                CardPlaceholder placeholder = cardHolder.AddComponent<CardPlaceholder>();
                int angle = i * 360 / size;
                if (angle <= 30)
                {
                    placeholder.actedSide = EActedSide.Down;
                }
                else if (angle <= 149)
                {
                    placeholder.actedSide = EActedSide.Left;
                }
                else if (angle <= 210)
                {
                    placeholder.actedSide = EActedSide.Up;
                }
                else if (angle <= 329)
                {
                    placeholder.actedSide = EActedSide.Right;
                }
                else
                {
                    placeholder.actedSide = EActedSide.Down;
                }
                circPool.cardPlaceHolders[i] = placeholder;
            }
            circle.transform.position = new UnityEngine.Vector3(0f, 1f, 85.9444f);
            circle.transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
            circle.SetActive(false);
            addToCharsPool(circPool);
            return circle;
        }
        */
        public static void addToCharsPool(CharactersPool pool)
        {
            CharactersPool[] pools = Characters.Instance.characterPool;
            CharactersPool[] newPools = new CharactersPool[pools.Length + 1];
            for (int i = 0; i < pools.Length; i++)
            {
                newPools[i] = pools[i];
            }
            newPools[pools.Length] = pool;
            Characters.Instance.characterPool = newPools;
        }

        public static void GetStartingRoles()
        {
            AscensionsData allCharactersAscension = ProjectContext.Instance.gameData.allCharactersAscension;
            foreach (CharacterData data in allCharactersAscension.startingTownsfolks)
            {
                CheckAddRole(data);
            }
            foreach (CharacterData data in allCharactersAscension.startingOutsiders)
            {
                CheckAddRole(data);
            }
            foreach (CharacterData data in allCharactersAscension.startingMinions)
            {
                CheckAddRole(data);
            }
            foreach (CharacterData data in allCharactersAscension.startingDemons)
            {
                CheckAddRole(data);
            }
        }
        public static void CheckAddRole(CharacterData data)
        {
            string name = data.name;
            if (!roles.ContainsKey(name))
            {
                roles.Add(name, data);
            }
        }

    }


    public static GameObject CreateCircle(int size)
    {
        GameObject circle = new GameObject();
        circle.name = "Circle_" + size;
        circle.transform.SetParent(Characters.Instance.gameObject.transform);
        RectTransform rt = circle.AddComponent<RectTransform>();
        CharactersPool cp = circle.AddComponent<CharactersPool>();
        GameObject gameObject = Characters.Instance.gameObject.transform.Find("Circle_6").gameObject;
        CharactersPool component = gameObject.GetComponent<CharactersPool>();
        cp.characterPrefab = component.characterPrefab;
        cp.characters = System.Array.Empty<Character>();
        cp.cardPlaceHolders = new CardPlaceholder[size];
        for (int i = 0; i < size; i++)
        {
            GameObject card = new GameObject();
            card.transform.SetParent(circle.transform);
            string text = "CardPlaceholder";
            if (i > 0)
            {
                text = text + " (" + i + ")";
            }
            card.name = text;
            RectTransform card_rt = card.AddComponent<RectTransform>();
            card_rt.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            CardPlaceholder cardPlaceholder = card.AddComponent<CardPlaceholder>();
            int num = i * 360 / size;
            if (num <= 30)
            {
                cardPlaceholder.actedSide = EActedSide.Down;
            }
            else if (num <= 149)
            {
                cardPlaceholder.actedSide = EActedSide.Left;
            }
            else if (num <= 210)
            {
                cardPlaceholder.actedSide = EActedSide.Up;
            }
            else if (num <= 329)
            {
                cardPlaceholder.actedSide = EActedSide.Right;
            }
            else
            {
                cardPlaceholder.actedSide = EActedSide.Down;
            }
            cp.cardPlaceHolders[i] = cardPlaceholder;
        }
        circle.transform.position = new Vector3(0f, 1f, 85.9444f);
        circle.transform.localScale = new Vector3(1f, 1f, 1f);
        circle.SetActive(false);
        addToCharsPool(cp);
        return circle;
    }
    public static void addToCharsPool(CharactersPool pool)
    {
        CharactersPool[] oldpool = Characters.Instance.characterPool;
        CharactersPool[] newPool = new CharactersPool[oldpool.Length + 1];
        for (int i = 0; i < oldpool.Length; i++)
        {
            newPool[i] = oldpool[i];
        }
        newPool[oldpool.Length] = pool;
        Characters.Instance.characterPool = newPool;
    }
    /*
    public MelonPreferences_Category randomInfoConfigCategory = null!;
    public void CreateRandomInfoConfigFile()
    {

        randomInfoConfigCategory = MelonPreferences.CreateCategory("WingModSettings_RandomInfo");
        randomInfoConfigCategory.CreateEntry("", 2, description: "");
        randomInfoConfigCategory.SetFilePath(Path.Combine(MelonEnvironment.UserDataDirectory, "WingModSettings_RandomInfo.cfg"));
        randomInfoConfigCategory.SaveToFile();
    }
    */
}