using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class EquipmentSlotManager : MonoBehaviour
    {
        EquipmentHolderSlot leftHandSlot;
        EquipmentHolderSlot rightHandSlot;
        EquipmentHolderSlot helmetSlot;
        EquipmentHolderSlot leggingSlot;
        EquipmentHolderSlot torsoSlot;
        EquipmentHolderSlot gloveSlot;
        EquipmentHolderSlot bootSlot;
        EquipmentHolderSlot amuletSlot;
        EquipmentHolderSlot quiverSlot;

        InventorySlot EquipedAmulet;
        InventorySlot EquipedHelmet;
        InventorySlot EquipedRightHand;
        InventorySlot EquipedLeftHand;
        InventorySlot EquipedTorso;
        InventorySlot EquipedBoots;
        InventorySlot EquipedLegging;
        InventorySlot EquipedGloves;

        [Header("Not Required")]
        public InventoryHandler inventoryHandler;

        [Header("Required")]
        public List<EquipmentHolderSlot> equipmentHolderSlots;
        public List<InventorySlot> equipedItems;
        public GameObject EquipedItems;

        public void Start()
        {
            inventoryHandler = GetComponent<InventoryHandler>();

            #region Assign Equipment Holder Slots On Character Model
            EquipmentHolderSlot[] equipmentHolderSlots = GetComponentsInChildren<EquipmentHolderSlot>();
            foreach (EquipmentHolderSlot equipmentHolderSlot in equipmentHolderSlots)
            {
                switch (equipmentHolderSlot.slotType)
                {
                    case SlotType.helmet:
                        helmetSlot = equipmentHolderSlot;
                        break;
                    case SlotType.gloves:
                        gloveSlot = equipmentHolderSlot;
                        break;
                    case SlotType.leggings:
                        leggingSlot = equipmentHolderSlot;
                        break;
                    case SlotType.torso:
                        torsoSlot = equipmentHolderSlot;
                        break;
                    case SlotType.leftHandSlot:
                        leftHandSlot = equipmentHolderSlot;
                        break;
                    case SlotType.rightHandSlot:
                        rightHandSlot = equipmentHolderSlot;
                        break;
                    case SlotType.amulet:
                        amuletSlot = equipmentHolderSlot;
                        break;
                    case SlotType.boots:
                        bootSlot = equipmentHolderSlot;
                        break;
                    case SlotType.quiver:
                        quiverSlot = equipmentHolderSlot;
                        break;
                }
            }
            #endregion

            #region Assign Equipment Item Slot on Inventory Menu
            InventorySlot[] equipedItems = EquipedItems.GetComponentsInChildren<InventorySlot>();
            foreach (InventorySlot equipedItem in equipedItems)
            {
                switch (equipedItem.slotType)
                {
                    case SlotType.amulet:
                        EquipedAmulet = equipedItem;
                        break;
                    case SlotType.gloves:
                        EquipedGloves = equipedItem;
                        break;
                    case SlotType.leftHandSlot:
                        EquipedLeftHand = equipedItem;
                        break;
                    case SlotType.rightHandSlot:
                        EquipedRightHand = equipedItem;
                        break;
                    case SlotType.leggings:
                        EquipedLegging = equipedItem;
                        break;
                    case SlotType.boots:
                        EquipedBoots = equipedItem;
                        break;
                    case SlotType.helmet:
                        EquipedHelmet = equipedItem;
                        break;
                    case SlotType.torso:
                        EquipedTorso = equipedItem;
                        break;
                }
            }
            #endregion
        }

        #region Loading/Unloading On Equipment Menu
        public void LoadEquipementOnEquipMenu(EquipableItem item, SlotType slotType)
        {
            switch (slotType)
            {
                case SlotType.helmet:
                    EquipedHelmet.item = item;
                    EquipedHelmet.UpdateSlot();
                    break;
                case SlotType.amulet:
                    EquipedAmulet.item = item;
                    EquipedAmulet.UpdateSlot();
                    break;
                case SlotType.leggings:
                    EquipedLegging.item = item;
                    EquipedLegging.UpdateSlot();
                    break;
                case SlotType.gloves:
                    EquipedGloves.item = item;
                    EquipedGloves.UpdateSlot();
                    break;
                case SlotType.boots:
                    EquipedBoots.item = item;
                    EquipedBoots.UpdateSlot();
                    break;
                case SlotType.torso:
                    EquipedTorso.item = item;
                    EquipedTorso.UpdateSlot();
                    break;
                case SlotType.leftHandSlot:
                    EquipedLeftHand.item = item;
                    EquipedLeftHand.UpdateSlot();
                    break;
                case SlotType.rightHandSlot:
                    EquipedRightHand.item = item;
                    EquipedRightHand.UpdateSlot();
                    break;
                case SlotType.quiver:
                    EquipedRightHand.item = item;
                    EquipedRightHand.UpdateSlot();
                    break;
            }
        }

        public void UnloadEquipementOnEquipMenu(EquipableItem item, SlotType slotType)
        {
            switch (slotType)
            {
                case SlotType.helmet:
                    EquipedHelmet.item = null;
                    EquipedHelmet.UpdateSlot();
                    break;
                case SlotType.amulet:
                    EquipedAmulet.item = null;
                    EquipedAmulet.UpdateSlot();
                    break;
                case SlotType.leggings:
                    EquipedLegging.item = null;
                    EquipedLegging.UpdateSlot();
                    break;
                case SlotType.gloves:
                    EquipedGloves.item = null;
                    EquipedGloves.UpdateSlot();
                    break;
                case SlotType.boots:
                    EquipedBoots.item = null;
                    EquipedBoots.UpdateSlot();
                    break;
                case SlotType.torso:
                    EquipedTorso.item = null;
                    EquipedTorso.UpdateSlot();
                    break;
                case SlotType.leftHandSlot:
                    EquipedLeftHand.item = null;
                    EquipedLeftHand.UpdateSlot();
                    break;
                case SlotType.rightHandSlot:
                    EquipedRightHand.item = null;
                    EquipedRightHand.UpdateSlot();
                    break;
                case SlotType.quiver:
                    EquipedRightHand.item = null;
                    EquipedRightHand.UpdateSlot();
                    break;
            }
        }
        #endregion

        #region Equipment Holder Slot Loading/Unloading On Character Model

        public void LoadEquipmentOnSlot(EquipableItem item, SlotType slotType)
        {
            switch(slotType)
            {
                case SlotType.helmet:
                    if (helmetSlot.currentModel != null)
                        helmetSlot.UnloadEquipment();
                    helmetSlot.LoadEquipmentModel(item);
                    break;
                case SlotType.amulet:
                    if (amuletSlot.currentModel != null)
                        amuletSlot.UnloadEquipment();
                    amuletSlot.LoadEquipmentModel(item);
                    break;
                case SlotType.leggings:
                    if (leggingSlot.currentModel != null)
                        leggingSlot.UnloadEquipment();
                    leggingSlot.LoadEquipmentModel(item);
                    break;
                case SlotType.gloves:
                    if (gloveSlot.currentModel != null)
                        gloveSlot.UnloadEquipment();
                    gloveSlot.LoadEquipmentModel(item);
                    break;
                case SlotType.boots:
                    if (bootSlot.currentModel != null)
                        bootSlot.UnloadEquipment();
                    bootSlot.LoadEquipmentModel(item);
                    break;
                case SlotType.torso:
                    if (torsoSlot.currentModel != null)
                        torsoSlot.UnloadEquipment();
                    torsoSlot.LoadEquipmentModel(item);
                    break;
                case SlotType.leftHandSlot:
                    if (leftHandSlot.currentModel != null)
                        leftHandSlot.UnloadEquipment();
                    leftHandSlot.LoadEquipmentModel(item);
                    break;
                case SlotType.rightHandSlot:
                    if (rightHandSlot.currentModel != null)
                        rightHandSlot.UnloadEquipment();
                    rightHandSlot.LoadEquipmentModel(item);
                    break;
                case SlotType.quiver:
                    if (quiverSlot.currentModel != null)
                        quiverSlot.UnloadEquipment();
                    quiverSlot.LoadEquipmentModel(item);
                    break;
            }
        }

        public void UnloadEquipmentOnSlot(EquipableItem item, SlotType slotType)
        {
            switch (slotType)
            {
                case SlotType.helmet:
                    if (helmetSlot.currentModel != null)
                        helmetSlot.UnloadEquipment();
                    break;
                case SlotType.amulet:
                    if (amuletSlot.currentModel != null)
                        amuletSlot.UnloadEquipment();
                    break;
                case SlotType.leggings:
                    if (leggingSlot.currentModel != null)
                        leggingSlot.UnloadEquipment();
                    break;
                case SlotType.gloves:
                    if (gloveSlot.currentModel != null)
                        gloveSlot.UnloadEquipment();
                    break;
                case SlotType.boots:
                    if (bootSlot.currentModel != null)
                        bootSlot.UnloadEquipment();
                    break;
                case SlotType.torso:
                    if (torsoSlot.currentModel != null)
                        torsoSlot.UnloadEquipment();
                    break;
                case SlotType.leftHandSlot:
                    if (leftHandSlot.currentModel != null)
                        leftHandSlot.UnloadEquipment();
                    break;
                case SlotType.rightHandSlot:
                    if (rightHandSlot.currentModel != null)
                        rightHandSlot.UnloadEquipment();
                    break;
                case SlotType.quiver:
                    if (quiverSlot.currentModel != null)
                        quiverSlot.UnloadEquipment();
                    break;
            }
        }
        #endregion
    }
}

