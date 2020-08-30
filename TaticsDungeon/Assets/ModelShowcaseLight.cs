using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class ModelShowcaseLight : MonoBehaviour
    {
        private void Update()
        {
            transform.position = transform.parent.position + Vector3.up * 2;
        }
    }
}

