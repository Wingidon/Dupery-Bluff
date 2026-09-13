using DuperyBluff;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using Il2CppTMPro;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[HarmonyPatch]
public static class VanillaPatch // Code by That Town Of Salem Player
{

}
[HarmonyPatch(typeof(Character), nameof(Character.Act))]
public static class InfoViewPatch // This is one of TToSP's shared functions. This is literally only here for the Spectre.
{
    private static readonly Dictionary<Character, Il2CppSystem.Action<ActedInfo, ETriggerPhase>>
 _hooks = new();

    [HarmonyPostfix]
    public static void Postfix(Character __instance, ETriggerPhase trigger)
    {
        if (_hooks.ContainsKey(__instance))
            return;
        if (__instance == null)
            return;
        //This used ChatGPT unfortunately. I didnt know how to access ActedInfo from here.
        // W: ...Ah. Well, if it works, I guess. Still don't support AI though.
        if (__instance.statuses.statuses.Contains(Obscured.Obscure))
        {

            var original = __instance.onAboutToAct;

            System.Action<ActedInfo, ETriggerPhase> callback = (info, phase) =>
            {
                ActedInfo aboutToActInfo = info;
                if (aboutToActInfo != null)
                {
                    info.desc = obscureWords(info.desc);
                }
                original?.Invoke(info, phase);
            };

            var converted = DelegateSupport.ConvertDelegate<Il2CppSystem.Action<ActedInfo, ETriggerPhase>
   >(callback);

            _hooks[__instance] = converted;
            __instance.onAboutToAct = converted;



        }

    }
    // Salem gave me the above code and said it should now work.
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
        //nums.Add("#");
        nums.Add("\n");
        nums.Add("\t");
        return nums;
    }
}


