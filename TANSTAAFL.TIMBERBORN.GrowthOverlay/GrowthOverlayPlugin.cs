using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;

namespace TANSTAAFL.TIMBERBORN.GrowthOverlay
{
    [HarmonyPatch]
    public class GrowthOverlayPlugin : IModEntrypoint
    {
        internal static IConsoleWriter Log;

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Log = consoleWriter;
            var harmony = new Harmony("tanstaafl.plugins.growthoverlay");
            harmony.PatchAll();

            consoleWriter.LogInfo("GrowthOverlay is loaded.");
        }
    }
}
