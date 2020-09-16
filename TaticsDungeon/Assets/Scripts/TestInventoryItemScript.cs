using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class TestInventoryItemScript : MonoBehaviour
    {
        public List<EquipableItem> items;
        public InventoryHandler inventoryHandler;

        public void Awake()
        {
            foreach (EquipableItem item in items)
                item.Init();
            inventoryHandler = GetComponent<InventoryHandler>();
        }

        // Start is called before the first frame update
        public void AddItem()
        {
            foreach (EquipableItem item in items)
            {
                inventoryHandler.AddItem(item);
            }
        }
    }
}

