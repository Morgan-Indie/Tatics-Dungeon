using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PrototypeGame
{
    public enum SlotType
    {
        rightHandSlot,
        helmet,
        amulet,
        boots,
        leggings,
        leftHandSlot,
        torso,
        gloves,
        normal,
        quiver
    }

    public class InventorySlot : UIButtonScript
    {
        [Header("Not Required")]
        public bool empty;
        public dynamic item;
        public Transform slotIconPanel;
        public RectTransform rectTransform;

        [Header("Required")]
        public GameObject itemUseDropDown;
        public RectTransform dropDownTransform;
        public Sprite defaultSprite;

        public SlotType slotType;

        private void Awake()
        {
            slotIconPanel = transform.GetChild(0);
            itemUseDropDown.SetActive(false);
            rectTransform = gameObject.GetComponent<RectTransform>();
            defaultSprite = GetComponentsInChildren<Image>()[1].sprite;
        }

        public void UpdateSlot()
        {
            if (item == null)
                slotIconPanel.GetComponent<Image>().sprite = defaultSprite;                
            else
                slotIconPanel.GetComponent<Image>().sprite = item.itemIcon;
        }

        public void ItemUIPopup()
        {
            if (item!=null)
            {
                itemUseDropDown.SetActive(true);
                GameObject button = itemUseDropDown.GetComponentsInChildren<Button>()[0].gameObject;
                EventSystem.current.SetSelectedGameObject(button);
                button.GetComponent<ItemUseSelection>().item = item;
                button.GetComponent<ItemUseSelection>().currentSlot = this.transform.gameObject;

                dropDownTransform = itemUseDropDown.GetComponent<RectTransform>();
                dropDownTransform.anchoredPosition = rectTransform.anchoredPosition + Vector2.right * 200;
            }
        }
    }
}

