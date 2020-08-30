using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CameraModeSwitch : MonoBehaviour
    {
        [Header("Required")]
        public IsometricCamera isometricCamera;

        // Start is called before the first frame update
        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            //Camera.main.enabled = true;
        }

        // Update is called once per frame
        public void CheckMode()
        {
            // optionInput is the input button to go to combatMode
            if (InputHandler.instance.optionInput && GameManager.instance.CombatMode)
            {
                GameManager.instance.ExitCombatMode();
                isometricCamera.GetComponent<Camera>().enabled = false;
                Cursor.lockState = CursorLockMode.Locked;

                //Camera.main.enabled = true;
            }

            else if (InputHandler.instance.optionInput)
            {
                GameManager.instance.EnterCombatMode();
                isometricCamera.FocusOnCurrentPlayer();
                isometricCamera.GetComponent<Camera>().enabled = true;
                Cursor.lockState = CursorLockMode.Confined;

                //Camera.main.enabled = false;
            }
            InputHandler.instance.optionInput = false;

        }
    }
}

