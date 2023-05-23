using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace CustomUtil
{
    
    /*
     IL_00fe: ldarg.0      // this
    IL_00ff: ldnull
    IL_0100: stfld        class ItemDrop/ItemData Humanoid::m_hiddenLeftItem

    IL_0105: br           IL_047b

    // [568 10 - 568 75]
    IL_010a: ldarg.1      // item
     */
    public class CustomUtil_Transpilers
    {
        [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.EquipItem))]
        public static class EquipItemTranspiler
        {
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> EquipItemsTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var cm = new CodeMatcher(instructions)
                    .MatchForward(useEnd: false,
                        new CodeMatch(OpCodes.Ldarg_0),
                        new CodeMatch(OpCodes.Ldnull),
                        new CodeMatch(OpCodes.Stfld, AccessTools.Field(typeof(Humanoid), nameof(Humanoid.m_hiddenLeftItem))),
                        new CodeMatch(OpCodes.Br)
                        )
                    .Advance(9)
                    .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
                    .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_1))
                    .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_2))
                    .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(EquipItemTranspiler), nameof(EquipItemTranspiler.TestMethod))))
                    .InstructionEnumeration();
                return cm;
            }

            public static void TestMethod(Humanoid humanoid, ItemDrop.ItemData data, bool useEffects)
            {
                if (data.m_dropPrefab.name.StartsWith("Lantern"))
                {
                    Debug.LogWarning("Found the lantern you should do shit with it");
                }
            }
        }
    }
}