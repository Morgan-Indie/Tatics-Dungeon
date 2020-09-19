using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class EquipmentSlotManager : MonoBehaviour
    {
        EquipmentHolderSlot leftHandSlot;
        EquipmentHolderSlot rightHandSlot;
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
        public PlayerEquipment playerEquipment;

        [Header("Required")]
        public List<EquipmentHolderSlot> equipmentHolderSlots;
        public List<InventorySlot> equipedItems;
        public GameObject EquipedItems;

        public void Awake()
        {
            inventoryHandler = GetComponent<InventoryHandler>();
            playerEquipment = GetComponent<PlayerEquipment>();

            #region Assign Equipment Holder Slots On Character Model
            EquipmentHolderSlot[] equipmentHolderSlots = GetComponentsInChildren<EquipmentHolderSlot>();
            foreach (EquipmentHolderSlot equipmentHolderSlot in equipmentHolderSlots)
            {
                switch (equipmentHolderSlot.slotType)
                {
                    case SlotType.leftHandSlot:
                        leftHandSlot = equipmentHolderSlot;
                        break;
                    case SlotType.rightHandSlot:
                        rightHandSlot = equipmentHolderSlot;
                        break;
                    case SlotType.amulet:
                        amuletSlot = equipmentHolderSlot;
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
                case SlotType.amulet:
                    if (amuletSlot.currentModel != null)
                        amuletSlot.UnloadEquipment();
                    amuletSlot.LoadEquipmentModel(item);
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

                default:
                    if (playerEquipment.gender == Gender.Male)
                    {
                        for (int i = 0; i < item.itemMeshesMale.Count; i++)
                            playerEquipment.LoadEquipmentMesh(item.itemMeshesMale[i], item.itemMeshLocs[i]);
                    }
                    else
                    {
                        for (int i = 0; i < item.itemMeshesFemale.Count; i++)
                            playerEquipment.LoadEquipmentMesh(item.itemMeshesFemale[i], item.itemMeshLocs[i]);
                    }
                    break;
            }
        }

        public void UnloadEquipmentOnSlot(EquipableItem item, SlotType slotType)
        {
            switch (slotType)
            {
                case SlotType.amulet:
                    if (amuletSlot.currentModel != null)
                        amuletSlot.UnloadEquipment();
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

                default:
                    foreach (meshLocation loc in item.itemMeshLocs)
                        playerEquipment.ResetToBaseEquipment(loc);
                    break;
            }
        }
        #endregion
    }
}

