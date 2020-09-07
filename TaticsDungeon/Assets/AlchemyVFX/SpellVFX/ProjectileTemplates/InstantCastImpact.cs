using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class InstantCastImpact : MonoBehaviour
    {
        GridCell destination;
        public float lifeTime = 1f;

        public void Initalize(GridCell cell)
        {
            destination = cell;
            Destroy(gameObject, lifeTime);
        }
    }
}