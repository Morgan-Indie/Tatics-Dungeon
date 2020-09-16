using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class ItemUseMethod : MonoBehaviour
    {
        [Header("Required")]
        public EquipmentSlotManager equipmentSlotManager;
        public InventoryHandler inventoryHandler;
        public EquipmentModifiersHandler modHandler;
        public StatPreviewSetText statpreview;
        public CharacterStats characterStats;

        [Header("Not Required")]
        public GameObject currentInventoryObject;

        public void Start()
        {
            equipmentSlotManager = GetComponentInParent<EquipmentSlotManager>();
            characterStats = GetComponentInParent<CharacterStats>();
            modHandler = GetComponentInParent<EquipmentModifiersHandler>();
            inventoryHandler = GetComponentInParent<InventoryHandler>();
        }

        #region Item Use Dispatch On Equipable Items
        public void Use(EquipableItem item)
        {
            if (currentInventoryObject.tag == "EquipmentSlot")
            {
                equipmentSlotManager.UnloadEquipmentOnSlot(item, item.slotType);
                equipmentSlotManager.UnloadEquipementOnEquipMenu(item, item.slotType);
                item.equipped = false;
                modHandler.RemoveAllModifiers(item);
                inventoryHandler.AddItem(item);
                characterStats.SetMaxAPFromStamina();
                characterStats.SetMaxHealthFromVitality();
            }

            else
            {
                equipmentSlotManager.LoadEquipmentOnSlot(item, item.slotType);
                equipmentSlotManager.LoadEquipementOnEquipMenu(item, item.slotType);
                item.equipped = true;                
                modHandler.ApplyEquipmentModifiers(item);
                characterStats.SetMaxAPFromStamina();
                characterStats.SetMaxHealthFromVitality();
            }
            statpreview.updateStatTexts();
        }
        #endregion

        #region Item Use Dispatch On Consumable Items

        public void Use(Items item)
        {
        }

        #endregion        
    }
}

