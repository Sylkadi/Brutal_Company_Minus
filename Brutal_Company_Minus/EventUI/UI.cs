using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using Brutal_Company_Minus._Event;
using UnityEngine.UI;
using Unity.Collections;
using UnityEngine.PlayerLoop;
using MonoMod.Cil;
using DunGen;
using UnityEngine.InputSystem;

namespace Brutal_Company_Minus.EventUI
{
    internal class UI : MonoBehaviour
    {
        public static UI Instance { get; private set; }
        public static GameObject eventUIObject { get; set; }

        public GameObject panelBackground;
        public TextMeshProUGUI panelText, letter;
        public Scrollbar panelScrollBar;

        public string key = "K";

        public bool showCaseEvents = false;

        public float showCaseEventTime = 20.0f;
        public float curretShowCaseEventTime = 0.0f;

        public void Start()
        {
            Instance = this;

            Server.Instance.textUI.OnValueChanged += (previous, current) => panelText.text = current.ToString(); // For Text update

            Component[] components = UI.eventUIObject.GetComponentsInChildren<Component>(true);
            foreach (Component comp in components)
            {
                try
                {
                    switch (comp.name)
                    {
                        case "EventPanel":
                            if (panelBackground == null) panelBackground = comp.gameObject;
                            break;
                        case "EventText":
                            if (panelText == null) panelText = comp.GetComponent<TextMeshProUGUI>();
                            break;
                        case "Letter":
                            if (letter == null)
                            {
                                letter = comp.GetComponent<TextMeshProUGUI>();
                                key = Configuration.UIKey.Value.ToUpper();
                                letter.text = key;
                            }
                            break;
                        case "LetterPanel":
                            if(!Configuration.ShowUILetterBox.Value || !Configuration.EnableUI.Value) comp.gameObject.SetActive(false);
                            break;
                        case "Scrollbar":
                            if (panelScrollBar == null) panelScrollBar = comp.GetComponent<Scrollbar>();
                            break;
                    }
                } catch
                {
                    Log.LogError("Failed to capture EventUI component/s.");
                }
            }


        }

        public static void SpawnObject()
        {
            if (eventUIObject != null) return;

            eventUIObject = (GameObject)Plugin.bundle.LoadAsset("EventGUI");
            eventUIObject.AddComponent<UI>();
            eventUIObject.AddComponent<OpenCloseUI>();
            eventUIObject.AddComponent<AutoScrollUI>();

            eventUIObject = Instantiate(eventUIObject, Vector3.zero, Quaternion.identity);
        }

        public static void GenerateText(List<Event> events)
        {
            // Generate Text
            string text = "<br>Events:<br>";
            foreach (Event e in events) text += string.Format("-<color={0}>{1}</color><br>", e.ColorHex, e.Description);

            // Extra properties
            if(Configuration.ShowExtraProperties.Value)
            {
                float ScrapValueMultiplier = RoundManager.Instance.scrapValueMultiplier;
                if (Configuration.NormaliseScrapValueDisplay.Value) ScrapValueMultiplier *= 2.5f;
                text += string.Format("<br>Map:<br> Scrap:<br>  -Value: x{0}<br>  -Amount: x{1}<br><br> Factory:<br>  -Size: x{2}",
                    ScrapValueMultiplier.ToString("F2"), RoundManager.Instance.scrapAmountMultiplier.ToString("F2"), RoundManager.Instance.currentLevel.factorySizeMultiplier.ToString("F2"));
            }

            Server.Instance.textUI.Value = new FixedString4096Bytes(text);
            if(Configuration.PopUpUI.Value && Configuration.EnableUI.Value) Server.Instance.ShowCaseEventsClientRpc();
        }

        public static void ClearText()
        {
            Server.Instance.textUI.Value = new FixedString4096Bytes("<br>No Events...");
        }
    }
}
