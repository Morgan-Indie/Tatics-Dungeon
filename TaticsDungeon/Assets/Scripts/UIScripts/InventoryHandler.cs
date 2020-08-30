﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PrototypeGame
{
    public class InventoryHandler : MonoBehaviour
    {
        [HideInInspector]
        public bool inventoryUIEnabled=false;
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
            if (stateManager.characterState!="isInteracting")
            {
                if (InputHandler.instance.gamepadNorthInput && !inventoryUIEnabled)
                {
                    inventoryUIEnabled = true;
                    inventoryUI.SetActive(true);
                    InputHandler.instance.gamepadNorthInput = false;

                    UIcam.enabled = true;
                    UIcamLight.enabled = true;

                    EventSystem.current.SetSelectedGameObject(first_slot.gameObject);
                    GameManager.instance.playerState = "inMenu";
                    stateManager.characterState = "inMenu";
                    Cursor.lockState = CursorLockMode.Confined;
                    //Time.timeScale = 0f;
                }

                else if (InputHandler.instance.gamepadNorthInput && inventoryUIEnabled)
                {
                    inventoryUIEnabled = false;
                    inventoryUI.SetActive(false);
                    InputHandler.instance.gamepadNorthInput = false;
                    itemUseDropDown.SetActive(false);

                    UIcam.enabled = false;
                    UIcamLight.enabled = false;

                    stateManager.characterState = "ready";
                    GameManager.instance.playerState = "ready";
                    if (!GameManager.instance.CombatMode)
                        Cursor.lockState = CursorLockMode.Locked;
                    //Time.timeScale = 1f;
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
