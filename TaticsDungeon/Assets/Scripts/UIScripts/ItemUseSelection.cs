using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using TMPro;

namespace PrototypeGame
{
    public class ItemUseSelection : UIButtonScript
    {
        List<Action> itemFunctions = new List<Action>(3);
        public dynamic item;

        [Header("Required")]
        public GameObject dropDownMenu;
        [Header("Not Required")]
        public GameObject currentSlot;
        public ItemUseMethod itemUseMethod;

        public void Start()
        {
            itemFunctions.Add(UseItem);
            itemFunctions.Add(GiveItem);
            itemFunctions.Add(DropItem);

            itemUseMethod = GetComponent<ItemUseMethod>();
        }

        public void SelectUsage(int val)
        {
            itemFunctions[val]();        
        }

        public void UseItem()
        {
            if (item != null)
            {
                if (item.type == ItemType.Equippable)
                    itemUseMethod.currentInventoryObject = currentSlot;

                itemUseMethod.Use(item);
                dropDownMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(currentSlot);
                currentSlot.GetComponent<InventorySlot>().item = null;
                currentSlot.GetComponent<InventorySlot>().empty = true;
                currentSlot.GetComponent<InventorySlot>().UpdateSlot();
            }
            else
            {
                Debug.Log("No items in slot");
                dropDownMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(currentSlot);
            }
        }

        public void GiveItem()
        {
            //give item to another player
            if (item != null)
            {
                dropDownMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(currentSlot);
                currentSlot.GetComponent<InventorySlot>().item = null;
                currentSlot.GetComponent<InventorySlot>().empty = true;
                currentSlot.GetComponent<InventorySlot>().UpdateSlot();
            }
            else
            {
                Debug.Log("No items in slot");
                dropDownMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(currentSlot);
            }
        }

        public void DropItem()
        {
            if (item != null)
            {
                Destroy(item);
                dropDownMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(currentSlot);
                currentSlot.GetComponent<InventorySlot>().item = null;
                currentSlot.GetComponent<InventorySlot>().empty = true;
                currentSlot.GetComponent<InventorySlot>().UpdateSlot();
            }
            else
            {
                Debug.Log("No items in slot");
                dropDownMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(currentSlot);
            }
        }
    }
}
