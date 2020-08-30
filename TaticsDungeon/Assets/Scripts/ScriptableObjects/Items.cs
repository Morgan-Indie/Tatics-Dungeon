using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PrototypeGame
{
    public enum ItemType
    {
        Equippable,
        Consumable
    }
    public class Items : ScriptableObject
    {
        [Header("Item Information")]

        public Sprite itemIcon;
        public string itemName;
        public int ID;
        public ItemType type;
        public string description;
        public bool pickedUp;
        public GameObject modelPrefab;
    }
}
