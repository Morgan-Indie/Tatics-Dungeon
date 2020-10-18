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
        public PlayerManager playerManager;

        [Header("Not Required")]
        public GameObject currentInventoryObject;

        public void Start()
        {
            equipmentSlotManager = GetComponentInParent<EquipmentSlotManager>();
            playerManager = GetComponentInParent<PlayerManager>();
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
                playerManager.AP.SetMaxAPFromStamina();
                playerManager.health.SetMaxHealthFromVitality();
            }

            else
            {
                equipmentSlotManager.LoadEquipmentOnSlot(item, item.slotType);
                equipmentSlotManager.LoadEquipementOnEquipMenu(item, item.slotType);
                item.equipped = true;                
                modHandler.ApplyEquipmentModifiers(item);
                playerManager.AP.SetMaxAPFromStamina();
                playerManager.health.SetMaxHealthFromVitality();
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

