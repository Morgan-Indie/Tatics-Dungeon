// GENERATED AUTOMATICALLY FROM 'Assets/Player Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace PrototypeGame
{
    public class @PlayerControls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Controls"",
    ""maps"": [
        {
            ""name"": ""PlayerMovement"",
            ""id"": ""ec04e837-eebd-4288-a446-48f88d83a771"",
            ""actions"": [
                {
                    ""name"": ""MovementControls"",
                    ""type"": ""PassThrough"",
                    ""id"": ""aea45133-cd8f-444a-80d2-5824190817ed"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraControls"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7f1ee886-92e0-4efb-879e-e07dc3eecf1e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraZoomIn"",
                    ""type"": ""PassThrough"",
                    ""id"": ""07b841c5-8a87-4dd7-88ff-0e4d85ff575c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraZoomOut"",
                    ""type"": ""Button"",
                    ""id"": ""1b76691f-79e8-4ea2-ba4b-8d4c4ab9bcf5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraRotateClockwise"",
                    ""type"": ""PassThrough"",
                    ""id"": ""95c2b176-6280-4c41-968d-d02dae4a4d8f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""CameraRotateCounterClockwise"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2ba588bb-a5d1-48d2-b463-c3da5005d269"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""f5c106f3-75f7-4b55-b74b-30e5b9664fd5"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""84e52914-f6d2-4e74-b7ad-d93c2e2dde0a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9178e639-eb0c-4680-a27c-3ac7634ed35e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""53d452d1-f4f9-4ef4-bf6f-fc346ee0b2b6"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2ff5f421-2d2a-4e34-b166-3c1e02523b28"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""a572b560-19da-41bf-b0ef-2bd3a803349a"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ec51ca27-6737-4086-aa38-f5998063b7c7"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2ab045db-bab1-4f90-a0f3-05483830acc2"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""deeacacf-9882-475a-8d14-d0e1061479a6"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1ae72675-90cc-4f24-b83a-8dd8876da6db"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""710b9b8f-e2a6-4cb1-9db4-905109e70ef9"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""CameraControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ce8c067-6bc1-4e2d-9a91-b9f54528e2a6"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": """",
                    ""action"": ""CameraControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""54f34599-e6c7-4157-857d-eb0e6f054e03"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraZoomIn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""de563019-6b0d-4063-90f6-2a9a5ace9e54"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraZoomOut"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""317e808f-9154-4c8c-a437-90fc40fe5997"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotateClockwise"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23519e2e-aac1-4f49-bc64-e36fccc07449"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotateClockwise"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a8f0244-3c41-4e84-9987-56e038c469a8"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotateCounterClockwise"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d2d5190-249c-47b5-ac7e-2c1a5c40d0af"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotateCounterClockwise"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerActions"",
            ""id"": ""9e7eba5d-e159-4d7d-8f1e-838593972644"",
            ""actions"": [
                {
                    ""name"": ""Attack"",
                    ""type"": ""PassThrough"",
                    ""id"": ""dac405dd-d7d7-4ffd-a5aa-8aad5d8b5aac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""PassThrough"",
                    ""id"": ""778734ff-e4f6-4b1a-aa22-5518a9025e60"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(pressPoint=0.1)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b41fa5d5-e6d1-4d5d-96df-f49994f2b7a7"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33904756-7bd1-41a0-ba7f-16255e3d5c28"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a86feb57-6a4e-447c-8f3a-0bcbd52b0384"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c426f6b-8c3f-4e8d-94ae-f45c0ffb4d2d"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""c77eb25b-fa85-471c-9000-4947a46786d0"",
            ""actions"": [
                {
                    ""name"": ""Inventory"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7fcd2579-3302-4ab9-add5-ba32cff3d9ea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""276591e9-b3f4-4cbc-a370-684eb8460366"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""18893b76-22b5-4586-936a-2f84ecbaf64e"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a850d6da-8255-4da8-ac2d-9d0fdd457c10"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8606ed01-c88a-45b2-90ad-883d3dae7a9e"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CameraMode"",
            ""id"": ""6aa71b82-604c-49bd-9114-3dfe588b4d17"",
            ""actions"": [
                {
                    ""name"": ""ModeSwitch"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7bdf7d74-d6a8-4ce1-a6d5-8d5c32087040"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6297422f-64cc-4cde-b05b-1029866a21ac"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ModeSwitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb096c47-dff7-4951-b7a8-1ac012a6b0f3"",
                    ""path"": ""<DualShockGamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ModeSwitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CharacterSelect"",
            ""id"": ""ed757f0b-a29a-4839-8753-de4fd0dd8e1f"",
            ""actions"": [
                {
                    ""name"": ""NextCharacter"",
                    ""type"": ""PassThrough"",
                    ""id"": ""501b0edc-8d11-42e0-8742-46c2f3e6b121"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PreviousCharacter"",
                    ""type"": ""PassThrough"",
                    ""id"": ""04d4b63d-023a-4e2d-9946-e59c63a0d3b7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6c34da27-bff8-4a80-b386-9eb02d78df79"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextCharacter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37fddedb-0e94-44d9-82db-cb649416679e"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextCharacter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""94272b18-5692-44db-b33b-234fa9f110ef"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PreviousCharacter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c6beac27-99ea-4a83-b6be-2b939bca58fa"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PreviousCharacter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // PlayerMovement
            m_PlayerMovement = asset.FindActionMap("PlayerMovement", throwIfNotFound: true);
            m_PlayerMovement_MovementControls = m_PlayerMovement.FindAction("MovementControls", throwIfNotFound: true);
            m_PlayerMovement_CameraControls = m_PlayerMovement.FindAction("CameraControls", throwIfNotFound: true);
            m_PlayerMovement_CameraZoomIn = m_PlayerMovement.FindAction("CameraZoomIn", throwIfNotFound: true);
            m_PlayerMovement_CameraZoomOut = m_PlayerMovement.FindAction("CameraZoomOut", throwIfNotFound: true);
            m_PlayerMovement_CameraRotateClockwise = m_PlayerMovement.FindAction("CameraRotateClockwise", throwIfNotFound: true);
            m_PlayerMovement_CameraRotateCounterClockwise = m_PlayerMovement.FindAction("CameraRotateCounterClockwise", throwIfNotFound: true);
            // PlayerActions
            m_PlayerActions = asset.FindActionMap("PlayerActions", throwIfNotFound: true);
            m_PlayerActions_Attack = m_PlayerActions.FindAction("Attack", throwIfNotFound: true);
            m_PlayerActions_Block = m_PlayerActions.FindAction("Block", throwIfNotFound: true);
            // Menu
            m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
            m_Menu_Inventory = m_Menu.FindAction("Inventory", throwIfNotFound: true);
            m_Menu_Click = m_Menu.FindAction("Click", throwIfNotFound: true);
            // CameraMode
            m_CameraMode = asset.FindActionMap("CameraMode", throwIfNotFound: true);
            m_CameraMode_ModeSwitch = m_CameraMode.FindAction("ModeSwitch", throwIfNotFound: true);
            // CharacterSelect
            m_CharacterSelect = asset.FindActionMap("CharacterSelect", throwIfNotFound: true);
            m_CharacterSelect_NextCharacter = m_CharacterSelect.FindAction("NextCharacter", throwIfNotFound: true);
            m_CharacterSelect_PreviousCharacter = m_CharacterSelect.FindAction("PreviousCharacter", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // PlayerMovement
        private readonly InputActionMap m_PlayerMovement;
        private IPlayerMovementActions m_PlayerMovementActionsCallbackInterface;
        private readonly InputAction m_PlayerMovement_MovementControls;
        private readonly InputAction m_PlayerMovement_CameraControls;
        private readonly InputAction m_PlayerMovement_CameraZoomIn;
        private readonly InputAction m_PlayerMovement_CameraZoomOut;
        private readonly InputAction m_PlayerMovement_CameraRotateClockwise;
        private readonly InputAction m_PlayerMovement_CameraRotateCounterClockwise;
        public struct PlayerMovementActions
        {
            private @PlayerControls m_Wrapper;
            public PlayerMovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @MovementControls => m_Wrapper.m_PlayerMovement_MovementControls;
            public InputAction @CameraControls => m_Wrapper.m_PlayerMovement_CameraControls;
            public InputAction @CameraZoomIn => m_Wrapper.m_PlayerMovement_CameraZoomIn;
            public InputAction @CameraZoomOut => m_Wrapper.m_PlayerMovement_CameraZoomOut;
            public InputAction @CameraRotateClockwise => m_Wrapper.m_PlayerMovement_CameraRotateClockwise;
            public InputAction @CameraRotateCounterClockwise => m_Wrapper.m_PlayerMovement_CameraRotateCounterClockwise;
            public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerMovementActions instance)
            {
                if (m_Wrapper.m_PlayerMovementActionsCallbackInterface != null)
                {
                    @MovementControls.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovementControls;
                    @MovementControls.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovementControls;
                    @MovementControls.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovementControls;
                    @CameraControls.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraControls;
                    @CameraControls.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraControls;
                    @CameraControls.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraControls;
                    @CameraZoomIn.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraZoomIn;
                    @CameraZoomIn.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraZoomIn;
                    @CameraZoomIn.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraZoomIn;
                    @CameraZoomOut.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraZoomOut;
                    @CameraZoomOut.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraZoomOut;
                    @CameraZoomOut.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraZoomOut;
                    @CameraRotateClockwise.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraRotateClockwise;
                    @CameraRotateClockwise.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraRotateClockwise;
                    @CameraRotateClockwise.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraRotateClockwise;
                    @CameraRotateCounterClockwise.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraRotateCounterClockwise;
                    @CameraRotateCounterClockwise.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraRotateCounterClockwise;
                    @CameraRotateCounterClockwise.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraRotateCounterClockwise;
                }
                m_Wrapper.m_PlayerMovementActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @MovementControls.started += instance.OnMovementControls;
                    @MovementControls.performed += instance.OnMovementControls;
                    @MovementControls.canceled += instance.OnMovementControls;
                    @CameraControls.started += instance.OnCameraControls;
                    @CameraControls.performed += instance.OnCameraControls;
                    @CameraControls.canceled += instance.OnCameraControls;
                    @CameraZoomIn.started += instance.OnCameraZoomIn;
                    @CameraZoomIn.performed += instance.OnCameraZoomIn;
                    @CameraZoomIn.canceled += instance.OnCameraZoomIn;
                    @CameraZoomOut.started += instance.OnCameraZoomOut;
                    @CameraZoomOut.performed += instance.OnCameraZoomOut;
                    @CameraZoomOut.canceled += instance.OnCameraZoomOut;
                    @CameraRotateClockwise.started += instance.OnCameraRotateClockwise;
                    @CameraRotateClockwise.performed += instance.OnCameraRotateClockwise;
                    @CameraRotateClockwise.canceled += instance.OnCameraRotateClockwise;
                    @CameraRotateCounterClockwise.started += instance.OnCameraRotateCounterClockwise;
                    @CameraRotateCounterClockwise.performed += instance.OnCameraRotateCounterClockwise;
                    @CameraRotateCounterClockwise.canceled += instance.OnCameraRotateCounterClockwise;
                }
            }
        }
        public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);

        // PlayerActions
        private readonly InputActionMap m_PlayerActions;
        private IPlayerActionsActions m_PlayerActionsActionsCallbackInterface;
        private readonly InputAction m_PlayerActions_Attack;
        private readonly InputAction m_PlayerActions_Block;
        public struct PlayerActionsActions
        {
            private @PlayerControls m_Wrapper;
            public PlayerActionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Attack => m_Wrapper.m_PlayerActions_Attack;
            public InputAction @Block => m_Wrapper.m_PlayerActions_Block;
            public InputActionMap Get() { return m_Wrapper.m_PlayerActions; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActionsActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActionsActions instance)
            {
                if (m_Wrapper.m_PlayerActionsActionsCallbackInterface != null)
                {
                    @Attack.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnAttack;
                    @Attack.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnAttack;
                    @Attack.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnAttack;
                    @Block.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnBlock;
                    @Block.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnBlock;
                    @Block.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnBlock;
                }
                m_Wrapper.m_PlayerActionsActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Attack.started += instance.OnAttack;
                    @Attack.performed += instance.OnAttack;
                    @Attack.canceled += instance.OnAttack;
                    @Block.started += instance.OnBlock;
                    @Block.performed += instance.OnBlock;
                    @Block.canceled += instance.OnBlock;
                }
            }
        }
        public PlayerActionsActions @PlayerActions => new PlayerActionsActions(this);

        // Menu
        private readonly InputActionMap m_Menu;
        private IMenuActions m_MenuActionsCallbackInterface;
        private readonly InputAction m_Menu_Inventory;
        private readonly InputAction m_Menu_Click;
        public struct MenuActions
        {
            private @PlayerControls m_Wrapper;
            public MenuActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Inventory => m_Wrapper.m_Menu_Inventory;
            public InputAction @Click => m_Wrapper.m_Menu_Click;
            public InputActionMap Get() { return m_Wrapper.m_Menu; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
            public void SetCallbacks(IMenuActions instance)
            {
                if (m_Wrapper.m_MenuActionsCallbackInterface != null)
                {
                    @Inventory.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnInventory;
                    @Inventory.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnInventory;
                    @Inventory.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnInventory;
                    @Click.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnClick;
                    @Click.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnClick;
                    @Click.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnClick;
                }
                m_Wrapper.m_MenuActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Inventory.started += instance.OnInventory;
                    @Inventory.performed += instance.OnInventory;
                    @Inventory.canceled += instance.OnInventory;
                    @Click.started += instance.OnClick;
                    @Click.performed += instance.OnClick;
                    @Click.canceled += instance.OnClick;
                }
            }
        }
        public MenuActions @Menu => new MenuActions(this);

        // CameraMode
        private readonly InputActionMap m_CameraMode;
        private ICameraModeActions m_CameraModeActionsCallbackInterface;
        private readonly InputAction m_CameraMode_ModeSwitch;
        public struct CameraModeActions
        {
            private @PlayerControls m_Wrapper;
            public CameraModeActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @ModeSwitch => m_Wrapper.m_CameraMode_ModeSwitch;
            public InputActionMap Get() { return m_Wrapper.m_CameraMode; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CameraModeActions set) { return set.Get(); }
            public void SetCallbacks(ICameraModeActions instance)
            {
                if (m_Wrapper.m_CameraModeActionsCallbackInterface != null)
                {
                    @ModeSwitch.started -= m_Wrapper.m_CameraModeActionsCallbackInterface.OnModeSwitch;
                    @ModeSwitch.performed -= m_Wrapper.m_CameraModeActionsCallbackInterface.OnModeSwitch;
                    @ModeSwitch.canceled -= m_Wrapper.m_CameraModeActionsCallbackInterface.OnModeSwitch;
                }
                m_Wrapper.m_CameraModeActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @ModeSwitch.started += instance.OnModeSwitch;
                    @ModeSwitch.performed += instance.OnModeSwitch;
                    @ModeSwitch.canceled += instance.OnModeSwitch;
                }
            }
        }
        public CameraModeActions @CameraMode => new CameraModeActions(this);

        // CharacterSelect
        private readonly InputActionMap m_CharacterSelect;
        private ICharacterSelectActions m_CharacterSelectActionsCallbackInterface;
        private readonly InputAction m_CharacterSelect_NextCharacter;
        private readonly InputAction m_CharacterSelect_PreviousCharacter;
        public struct CharacterSelectActions
        {
            private @PlayerControls m_Wrapper;
            public CharacterSelectActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @NextCharacter => m_Wrapper.m_CharacterSelect_NextCharacter;
            public InputAction @PreviousCharacter => m_Wrapper.m_CharacterSelect_PreviousCharacter;
            public InputActionMap Get() { return m_Wrapper.m_CharacterSelect; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CharacterSelectActions set) { return set.Get(); }
            public void SetCallbacks(ICharacterSelectActions instance)
            {
                if (m_Wrapper.m_CharacterSelectActionsCallbackInterface != null)
                {
                    @NextCharacter.started -= m_Wrapper.m_CharacterSelectActionsCallbackInterface.OnNextCharacter;
                    @NextCharacter.performed -= m_Wrapper.m_CharacterSelectActionsCallbackInterface.OnNextCharacter;
                    @NextCharacter.canceled -= m_Wrapper.m_CharacterSelectActionsCallbackInterface.OnNextCharacter;
                    @PreviousCharacter.started -= m_Wrapper.m_CharacterSelectActionsCallbackInterface.OnPreviousCharacter;
                    @PreviousCharacter.performed -= m_Wrapper.m_CharacterSelectActionsCallbackInterface.OnPreviousCharacter;
                    @PreviousCharacter.canceled -= m_Wrapper.m_CharacterSelectActionsCallbackInterface.OnPreviousCharacter;
                }
                m_Wrapper.m_CharacterSelectActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @NextCharacter.started += instance.OnNextCharacter;
                    @NextCharacter.performed += instance.OnNextCharacter;
                    @NextCharacter.canceled += instance.OnNextCharacter;
                    @PreviousCharacter.started += instance.OnPreviousCharacter;
                    @PreviousCharacter.performed += instance.OnPreviousCharacter;
                    @PreviousCharacter.canceled += instance.OnPreviousCharacter;
                }
            }
        }
        public CharacterSelectActions @CharacterSelect => new CharacterSelectActions(this);
        public interface IPlayerMovementActions
        {
            void OnMovementControls(InputAction.CallbackContext context);
            void OnCameraControls(InputAction.CallbackContext context);
            void OnCameraZoomIn(InputAction.CallbackContext context);
            void OnCameraZoomOut(InputAction.CallbackContext context);
            void OnCameraRotateClockwise(InputAction.CallbackContext context);
            void OnCameraRotateCounterClockwise(InputAction.CallbackContext context);
        }
        public interface IPlayerActionsActions
        {
            void OnAttack(InputAction.CallbackContext context);
            void OnBlock(InputAction.CallbackContext context);
        }
        public interface IMenuActions
        {
            void OnInventory(InputAction.CallbackContext context);
            void OnClick(InputAction.CallbackContext context);
        }
        public interface ICameraModeActions
        {
            void OnModeSwitch(InputAction.CallbackContext context);
        }
        public interface ICharacterSelectActions
        {
            void OnNextCharacter(InputAction.CallbackContext context);
            void OnPreviousCharacter(InputAction.CallbackContext context);
        }
    }
}
