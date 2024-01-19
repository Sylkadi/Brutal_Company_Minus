using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Brutal_Company_Minus.EventUI
{
    public class OpenCloseUI : MonoBehaviour
    {
        public static OpenCloseUI Instance;

        public bool keyPressEnabledTyping = true, keyPressEnabledTerminal = true, keyPressEnabledSettings = true;

        public Keyboard keyboard;

        void Start()
        {
            Instance = this;
            keyboard = Keyboard.current;
            if (keyboard != null) keyboard.onTextInput += OnKeyboardInput;
        }

        public void OnKeyboardInput(char input)
        {
            if (input.ToString().ToUpper() == UI.Instance.key && keyPressEnabledTyping && keyPressEnabledTerminal && keyPressEnabledSettings)
            {
                bool newState = !UI.Instance.panelBackground.activeSelf;

                if(!newState && UI.Instance.showCaseEvents)
                {
                    UI.Instance.showCaseEvents = false;
                    UI.Instance.panelScrollBar.value = 1.0f; // Reset to top
                }

                UI.Instance.panelBackground.SetActive(newState);
            }
        }

        public void UnsubscribeFromKeyboardEvent()
        {
            keyboard.onTextInput -= OnKeyboardInput;
        }
    }
}
