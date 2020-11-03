using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public abstract class BuildingAbstract : MonoBehaviour
    {
        public GameObject[] highlightEnabledObjects;
        public CameraTransition cameraTransition;
        public string buildingName;

        public virtual void Highlight()
        {
            foreach (GameObject ob in highlightEnabledObjects)
            {
                ob.SetActive(true);
            }
        }

        public virtual void RemoveHighlight()
        {
            foreach (GameObject ob in highlightEnabledObjects)
            {
                ob.SetActive(false);
            }
        }
    }
}