using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;
using BepInEx;
using HarmonyLib;

namespace warmode_cheat.cheats
{
    [HarmonyPatch]
    public class noshake
    {
        public static bool activated = false;
        public static bool ractivated = false;

        [DllImport("GameAssembly",CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void il2cpp_raise_exception(IntPtr exc);

        [HarmonyPatch(typeof(vp_FPCamera), "AddRecoilForce")]
        [HarmonyPrefix]
        public static void Prefix(ref bool __runOriginal)
        {
            if (ractivated)
            {
                il2cpp_raise_exception(new Il2CppSystem.Exception().Pointer);
            }

            if (activated)
            {
                __runOriginal = false;
            }
            
           
            

            
          
        }

    }
}
