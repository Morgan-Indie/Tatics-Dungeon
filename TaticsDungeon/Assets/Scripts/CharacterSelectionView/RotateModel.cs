using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class RotateModel : MonoBehaviour
    {
        [SerializeField]
        private float rotationSpeed = 15f;
        public Transform model;

        // Update is called once per frame
        void Update()
        {
            model.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
        }
    }
}

