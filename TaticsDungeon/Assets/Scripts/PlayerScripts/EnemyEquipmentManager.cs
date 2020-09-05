using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class EnemyEquipmentManager : MonoBehaviour
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

        [Header("Required")]
        public List<EquipmentHolderSlot> equipmentHolderSlots;
        public List<EquipableItem> Equipments;

        EquipmentModifiersHandler modHandler;
        
        public void Start()
        {
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

            modHandler = GetComponent<EquipmentModifiersHandler>();

            foreach (EquipableItem item in Equipments)
                EquipItem(item);
        }

        #region Equipment Holder Slot Loading/Unloading On Character Model

        public void LoadEquipmentOnSlot(EquipableItem item, SlotType slotType)
        {
            switch (slotType)
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

        private void EquipItem(EquipableItem item)
        {
            LoadEquipmentOnSlot(item, item.slotType);
            item.equipped = true;
            modHandler.ApplyEquipmentModifiers(item);
        }
    }
}

