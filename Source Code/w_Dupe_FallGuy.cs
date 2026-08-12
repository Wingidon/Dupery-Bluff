using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;

namespace DuperyBluff;

[RegisterTypeInIl2Cpp]
public class w_Dupe_FallGuy : w_DupeZ_RoleBase
{
    public CharacterData[] allDatas = Il2CppSystem.Array.Empty<CharacterData>();
    public override string Description
    {
        get
        {
            return "Registers as Evil";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            new wx_SavedScripts().DebugMessage($"Fall Guy initialised at #{charRef.id}");
        }
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            new wx_SavedScripts().DebugMessage($"Fall Guy at #{charRef.id} Registering as an {charRef.GetRegisterAlignment()} {charRef.GetRegisterAs().type} {charRef.GetRegisterAs().characterName}");
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init) new wx_SavedScripts().DebugMessage($"Lying Fall Guy initialised at #{charRef.id}");
    }
    public override CharacterData GetRegisterAsRole(Character charRef)
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
        CharacterData myRegister = charRef.GetRegisterAs();
        for (int i = 0; i < allDatas.Length; i++)
        {
            if (allDatas[i].characterId == "WING_Dupery_Fall Guy MinionRegister")
            {
                myRegister = allDatas[i];
                break;
            }
        }
        return myRegister;
    }
    public w_Dupe_FallGuy() : base(ClassInjector.DerivedConstructorPointer<w_Dupe_FallGuy>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public w_Dupe_FallGuy(System.IntPtr ptr) : base(ptr)
    {
    }
}