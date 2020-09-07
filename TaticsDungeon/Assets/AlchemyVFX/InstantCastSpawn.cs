using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame {
    public class InstantCastSpawn : MonoBehaviour
    {
        public GridCell destination;
        public GameObject projectilePrefab;
        public void Initalize(GridCell cell, IntVector2 index)
        {
            destination = cell;
        }
    }
}