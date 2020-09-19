using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class TestInventoryItemScript : MonoBehaviour
    {
        public List<EquipableItem> items;
        public InventoryHandler inventoryHandler;
        public EquipmentModifiersHandler modHandler;
        public CharacterStats characterStats;
        public EquipmentSlotManager equipmentSlotManager;

        public void Awake()
        {
            foreach (EquipableItem item in items)
                item.Init();
            inventoryHandler = GetComponent<InventoryHandler>();
            modHandler = GetComponent<EquipmentModifiersHandler>();
            characterStats = GetComponent<CharacterStats>();
            equipmentSlotManager = GetComponent<EquipmentSlotManager>();
        }

        // Start is called before the first frame update
        public void AddItems()
        {
            foreach (EquipableItem item in items)
            {
                equipmentSlotManager.LoadEquipmentOnSlot(item, item.slotType);
                equipmentSlotManager.LoadEquipementOnEquipMenu(item, item.slotType);
                item.equipped = true;
                modHandler.ApplyEquipmentModifiers(item);
                characterStats.SetMaxAPFromStamina();
                characterStats.SetMaxHealthFromVitality();
            }
        }
    }
}

