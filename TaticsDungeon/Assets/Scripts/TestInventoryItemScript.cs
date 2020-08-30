using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class TestInventoryItemScript : MonoBehaviour
    {
        public EquipableItem item;
        public EquipableItem item1;
        public InventoryHandler inventoryHandler;

        public void Awake()
        {
            if (item!=null)
                item.Init();
            if (item1 != null)
                item1.Init();
            inventoryHandler = GetComponent<InventoryHandler>();
        }

        // Start is called before the first frame update
        public void AddItem()
        {
            if (item != null)
                inventoryHandler.AddItem(item);
            if (item1 != null)
                inventoryHandler.AddItem(item1);
        }
    }
}

