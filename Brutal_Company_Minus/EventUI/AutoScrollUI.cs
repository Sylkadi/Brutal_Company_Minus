using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Brutal_Company_Minus.EventUI
{
    public class AutoScrollUI : MonoBehaviour
    {

        void Update()
        {
            if(UI.Instance != null)
            {
                if(UI.Instance.showCaseEvents)
                {
                    UI.Instance.curretShowCaseEventTime -= Time.deltaTime; // Decrement timer
                    if (UI.Instance.curretShowCaseEventTime <= UI.Instance.showCaseEventTime * 0.6f) UI.Instance.panelScrollBar.value -= (1 / (UI.Instance.showCaseEventTime * 0.8f)) * Time.deltaTime * 2.0f;
                    // End showcase events
                    if (UI.Instance.curretShowCaseEventTime < 0.0f)
                    {
                        UI.Instance.panelScrollBar.value = 1.0f; // Reset to top
                        UI.Instance.showCaseEvents = false;
                        UI.Instance.panelBackground.SetActive(false);
                    }
                }
            }
        }
    }
}
