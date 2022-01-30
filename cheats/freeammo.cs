using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;

namespace warmode_cheat.cheats
{
    [HarmonyPatch]
    public class freeammo
    {
        public static bool activated = false;

        [HarmonyPatch(typeof(FirstPersonPlayer), "SubtractAmmo")]
        [HarmonyPrefix]
        public static void Prefix(ref bool __runOriginal)
        {

            if (activated)
            {
                __runOriginal = false;
            }
            
           
            

            
          
        }

    }
}
