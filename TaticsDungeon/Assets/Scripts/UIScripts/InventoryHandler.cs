using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PrototypeGame
{
    public class InventoryHandler : MonoBehaviour
    {
        [HideInInspector]
        private int allSlots=10;
        private int enabledSlots;
        private InventorySlot[] slots;
        PlayerManager playerManager;
        CharacterStateManager stateManager;
        public Camera UIcam;
        public Light UIcamLight;
        public InventorySlot first_slot;

        [Header("Required")]
        public GameObject slotHolder, itemUseDropDown, inventoryUI;

        [Header("For Testing Only")]
        public TestInventoryItemScript testInventoryItemScript;

        public void Start()
        {
            slots = slotHolder.GetComponentsInChildren<InventorySlot>();
            first_slot = slots[0];

            foreach (InventorySlot slot in slots)
            {
                if (slot.item == null)
                    slot.empty = true;
            }

            inventoryUI.SetActive(false);
            itemUseDropDown.gameObject.SetActive(false);

            stateManager = GetComponent<CharacterStateManager>();
            playerManager = GetComponent<PlayerManager>();
            UIcam = Camera.main.GetComponentsInChildren<Camera>()[1];
            UIcamLight = UIcam.GetComponentInChildren<Light>();

            //test script for equiping items
            testInventoryItemScript = GetComponent<TestInventoryItemScript>();
            testInventoryItemScript.AddItem();
        }

        public void ActivateInventoryUI()
        {
            if (stateManager.characterState!= CharacterState.IsInteracting)
            {
                if (InputHandler.instance.gamepadNorthInput && stateManager.characterState!=CharacterState.InMenu)
                {
                    inventoryUI.SetActive(true);
                    InputHandler.instance.gamepadNorthInput = false;

                    UIcam.enabled = true;
                    UIcamLight.enabled = true;

                    EventSystem.current.SetSelectedGameObject(first_slot.gameObject);
                    stateManager.characterState = CharacterState.InMenu;
                    playerManager.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 0);
                    //Time.timeScale = 0f;
                }

                else if (InputHandler.instance.gamepadNorthInput && stateManager.characterState == CharacterState.InMenu)
                {
                    inventoryUI.SetActive(false);
                    InputHandler.instance.gamepadNorthInput = false;
                    itemUseDropDown.SetActive(false);

                    UIcam.enabled = false;
                    UIcamLight.enabled = false;

                    stateManager.characterState = CharacterState.Ready;
                    playerManager.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 1);
                }
            }
        }

        public void AddItem(dynamic item)
        {
            for (int i = 0; i <allSlots; i++)
            {
                if (slots[i].empty)
                {
                    item.pickedUp = true;
                    slots[i].item = item;
                    
                    slots[i].UpdateSlot();
                    slots[i].empty = false;
                    //item.SetActive(false);
                    return;
                }
            }
        }
    }
}

