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

        CharacterStats characterStats;
        EquipmentModifiersHandler modHandler;
        
        public void Start()
        {
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
                    case SlotType.quiver:
                        quiverSlot = equipmentHolderSlot;
                        break;
                }
            }
            #endregion

            modHandler = GetComponent<EquipmentModifiersHandler>();
            characterStats = GetComponent<CharacterStats>();

            foreach (EquipableItem item in Equipments)
                EquipItem(item);
        }

        #region Equipment Holder Slot Loading/Unloading On Character Model

        public void LoadEquipmentOnSlot(EquipableItem item, SlotType slotType)
        {
            switch (slotType)
            {              
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
            characterStats.SetMaxAPFromStamina();
            characterStats.SetMaxHealthFromVitality();
        }
    }
}

