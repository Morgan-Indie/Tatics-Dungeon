using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class IceBombImpact : MonoBehaviour
    {
        public void Initalize(GridCell cell)
        {
            AlchemyManager.Instance.ApplyChill(cell.alchemyState);
            Destroy(gameObject, 0.75f);
        }
    }
}