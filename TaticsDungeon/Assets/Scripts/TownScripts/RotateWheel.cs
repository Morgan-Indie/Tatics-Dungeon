using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class RotateWheel : MonoBehaviour
    {
        public GameObject ob;
        public enum Axis { X, Y, Z};
        public Axis axis;
        public float speed = 1;

        private void Update()
        {
            switch (axis)
            {
                case Axis.X:
                    ob.transform.Rotate(new Vector3(speed * Time.deltaTime, 0, 0));
                    break;
                case Axis.Y:
                    ob.transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
                    break;
                case Axis.Z:
                    ob.transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
                    break;
            }
        }
    }
}