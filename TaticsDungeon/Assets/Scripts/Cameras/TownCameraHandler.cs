using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class TownCameraHandler : MonoBehaviour
    {
        public CameraTransition townView;
        public bool traveling = false;

        private Transform lookAtPoint;
        private float lookInterpolationRate = 0.1f;

        public Transform origin;

        private void Start()
        {
            origin = townView.transform;
        }

        private void Update()
        {
            //Look at point
            if (lookAtPoint != null)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(lookAtPoint.position - transform.position),
                    lookInterpolationRate
                    );
            }
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
            lookAtPoint = null;
        }

        public void LookAtPoint(Transform p, float i = 0.1f)
        {
            lookAtPoint = p;
            lookInterpolationRate = i;
        }

        public bool IsViewingTown() { return origin == townView.transform; }
    }
}