using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler instance = null;

        public float MouseX, MouseY,MoveX,MoveY;
        PlayerControls inputActions;
        public bool rightTriggerInput;
        public bool leftTriggerInput;
        public bool gamepadNorthInput;
        public bool gamepadSouthInput;
        public bool rightShoulderInput;
        public bool leftShoulderInput;
        public bool optionInput;
        public bool tacticsXInput;
        public bool characterSelectInputNext;
        public bool characterSelectInputPrevious;

        Vector2 movementInput, cameraInput;

        public void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.MovementControls.performed += i => movementInput = i.ReadValue<Vector2>();
                inputActions.PlayerMovement.CameraControls.performed += j => cameraInput = j.ReadValue<Vector2>();
                inputActions.PlayerActions.Block.started += _ => leftTriggerInput = true;
                inputActions.PlayerActions.Block.canceled += _ => leftTriggerInput = false;
                inputActions.PlayerActions.Attack.started += _ => rightTriggerInput = true;
                inputActions.PlayerActions.Attack.canceled += _ => rightTriggerInput = false;
                inputActions.PlayerMovement.CameraZoomIn.started += _ => rightTriggerInput = true;
                inputActions.PlayerMovement.CameraZoomOut.started += _ => leftTriggerInput = true;
                inputActions.PlayerMovement.CameraZoomIn.canceled += _ => rightTriggerInput = false;
                inputActions.PlayerMovement.CameraZoomOut.canceled += _ => leftTriggerInput = false;
                inputActions.Menu.Inventory.canceled += _ => gamepadNorthInput=true;
                inputActions.Menu.Click.started += _ => gamepadSouthInput = true;
                inputActions.Menu.Click.canceled += _ => tacticsXInput = true;
                inputActions.PlayerMovement.CameraRotateClockwise.started += _ => rightShoulderInput = true;
                inputActions.PlayerMovement.CameraRotateCounterClockwise.started += _ => leftShoulderInput = true;
                inputActions.PlayerMovement.CameraRotateClockwise.canceled += _ => rightShoulderInput = false;
                inputActions.PlayerMovement.CameraRotateCounterClockwise.canceled += _ => leftShoulderInput = false;
                inputActions.CameraMode.ModeSwitch.started += _ => optionInput = true;
                inputActions.CharacterSelect.NextCharacter.started += _ => characterSelectInputNext = true;
                inputActions.CharacterSelect.PreviousCharacter.started += _ => characterSelectInputPrevious = true;
            }
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        //this is to make sure the inputs all have the same delta
        public void TickInput(float delta)
        {
            GetMoveInputs(delta);
        }

        public void GetMoveInputs(float delta)
        {
            MouseX = cameraInput.x;
            MouseY = cameraInput.y;
            MoveX = movementInput.x;
            MoveY = movementInput.y;
        }
    }
}

