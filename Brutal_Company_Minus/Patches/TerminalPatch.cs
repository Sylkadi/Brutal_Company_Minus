using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Brutal_Company_Minus;
using Brutal_Company_Minus.EventUI;
using HarmonyLib;
using UnityEngine.Rendering;

namespace Brutal_Company_Minus.Patches
{
    [HarmonyPatch]
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatch
    {

        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        public static void OnUpdate(ref bool ___terminalInUse)
        {
            try
            {
                OpenCloseUI.Instance.keyPressEnabledTerminal = !___terminalInUse;
            } catch { }
        }

        [HarmonyPostfix]
        [HarmonyPatch("TextPostProcess")]
        public static void OnLoadNewNode(ref string __result)
        {
            if(Configuration.useWeatherMultipliers.Value)
            {
                string s = __result;

                // Are we on moon tab?
                int index = s.IndexOf("exomoons");
                if (index > 0)
                {
                    // Make format text
                    index = s.IndexOf("INFO.");
                    if (index > 0) s = s.Insert(index + 5, "\nFormat: (xScrapValue, xScrapAmount, xFactorySize)");

                    // Create index list of '*' on all moons
                    List<int> indexList = new List<int>();
                    index = s.IndexOf("*") + 1;
                    while (index > 0)
                    {
                        index = s.IndexOf("*", index + 1);
                        indexList.Add(index);
                    }
                    indexList.Remove(-1);

                    // Create moon text list
                    List<string> moonTextList = new List<string>();
                    for(int i = 0; i < indexList.Count - 1; i++)
                    {
                        moonTextList.Add(s.Substring(indexList[i], indexList[i + 1] - indexList[i] - 1));
                    }
                    moonTextList.Add(s.Substring(indexList[indexList.Count - 1], s.Length - indexList[indexList.Count - 1] - 1));

                    string[] oldMoonTextList = moonTextList.ToArray();
                    // Make new moon text list
                    for (int i = 0; i < moonTextList.Count; i++) 
                    {
                        // Remove new line occurence
                        bool RemovedNewLine = false;
                        moonTextList[i] = Regex.Replace(moonTextList[i], @"\r|\n", "");
                        if (moonTextList[i] != oldMoonTextList[i]) RemovedNewLine = true;

                        // Check if weather is 'none'
                        index = moonTextList[i].IndexOf("(");
                        if(index > 0) // Moon has weather
                        {
                            foreach (Weather w in Server.Instance.currentWeatherMultipliers)
                            {
                                if (moonTextList[i].Contains(w.weatherType.ToString()))
                                {
                                    string multiplierText =
                                        " (x" + w.scrapValueMultiplier.ToString("F2") + 
                                        ", x" + w.scrapAmountMultiplier.ToString("F2") + 
                                        ", x" + w.factorySizeMultiplier.ToString("F2") + ")";
                                    moonTextList[i] = moonTextList[i].Insert(moonTextList[i].Length, multiplierText);
                                }
                            }
                        } else // Moon weather is 'none'
                        {
                            string multiplierText = 
                                "x" + Server.Instance.currentWeatherMultipliers[0].scrapValueMultiplier.ToString("F2") +
                                ", x" + Server.Instance.currentWeatherMultipliers[0].scrapAmountMultiplier.ToString("F2") + 
                                ", x" + Server.Instance.currentWeatherMultipliers[0].factorySizeMultiplier.ToString("F2");
                            moonTextList[i] = moonTextList[i].Insert(moonTextList[i].Length - 1, " (" + multiplierText + ")");
                        }

                        // Give back new line
                        if (RemovedNewLine) moonTextList[i] += "\r\n";
                    }

                    // Update text
                    for(int i = 0; i < oldMoonTextList.Length; i++)
                    {
                        s = s.Replace(oldMoonTextList[i], moonTextList[i]);
                    }
                }

                __result = s;
            }
        }

    }
}
