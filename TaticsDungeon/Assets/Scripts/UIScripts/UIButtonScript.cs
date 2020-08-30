using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PrototypeGame
{
    public class UIButtonScript : MonoBehaviour
    {
        public void GamepadClick(GameObject buttonObject)
        {
            if (InputHandler.instance.gamepadSouthInput)
            {
                Button button = buttonObject.GetComponent<Button>();
                button.onClick.Invoke();
                InputHandler.instance.gamepadSouthInput = false;
            }
        }

        public void FixedUpdate()
        {
            if (GameManager.instance.playerState == "inMenu")
                GamepadClick(EventSystem.current.currentSelectedGameObject);
        }
    }
}
