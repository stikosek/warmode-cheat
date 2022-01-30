using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using UnhollowerRuntimeLib;
using HarmonyLib;
using UnityEngine;


namespace warmode_cheat
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Plugin : BepInEx.IL2CPP.BasePlugin
    {
        public const string
            MODNAME = "warmode_cheat",
            AUTHOR = "stikosek",
            GUID = "net." + AUTHOR + "." + MODNAME,
            VERSION = "0.0.1";







        public override void Load()
        {

            var harmony = new Harmony(GUID);
            harmony.PatchAll();
            Log.LogInfo("Plugin WARMODE CHEAT is loaded!");

            CodeStage.AntiCheat.Detectors.SpeedHackDetector.StopDetection();
            CodeStage.AntiCheat.Detectors.SpeedHackDetector.Dispose();

            gui.MainGuiStrut.CreateInstance(this);

        }





    }
}
