using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FireStormImpact : MonoBehaviour
    {
        public void Initalize(GridCell cell)
        {
            AlchemyManager.Instance.ApplyHeat(cell.alchemyState);
            Destroy(gameObject, 1f);
        }
    }
}
