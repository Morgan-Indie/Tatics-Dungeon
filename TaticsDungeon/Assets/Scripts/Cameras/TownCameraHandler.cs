using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class TownCameraHandler : MonoBehaviour
    {
        public CameraTransition townView;
        public bool traveling = false;

        public Transform origin;

        private void Start()
        {
            origin = townView.transform;
        }

        public void Transition(CameraTransition cameraTransition)
        {
            if (IsViewingTown())
            {
                cameraTransition.Transition(this);
                traveling = true;
            }
        }

        public void TransitionToTownView()
        {
            townView.Transition(this);
            traveling = true;
        }

        public void CompleteTransition(Transform newTransform)
        {
            origin = newTransform;
            traveling = false;
        }

        public bool IsViewingTown() { return origin == townView.transform; }
    }
}