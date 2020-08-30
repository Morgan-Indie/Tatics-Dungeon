using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class PlayerEquipment : MonoBehaviour
    {
        EquipmentSlotManager equipmentSlotManager;

        private void Start()
        {
            equipmentSlotManager = GetComponentInChildren<EquipmentSlotManager>();
            foreach (InventorySlot itemSlot in equipmentSlotManager.equipedItems)
            {
                if (itemSlot.item != null)
                    equipmentSlotManager.LoadEquipmentOnSlot(itemSlot.item, itemSlot.item.slotType);
            }
        }
    }
}

