using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform playerTransform;
        public Transform cameraTransform;
        public Transform pivotTransform;
        public Transform frameTransform;
        InputHandler inputHandler;
        private Vector3 pivotOffset;
        public float followSpeed = .1f;
        public float lookSpeed = 360f;
        public float pivotSpeed = 60f;

        public float maximumPivot = 60f;
        public static CameraHandler instance=null;
        public Quaternion pivotRotation;
        public Quaternion lookRotation;
        Vector3 pivotDirection;
        public Camera UIcam;
        public Light UIcamLight;
        // Start is called before the first frame update

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        void Start()
        {
            frameTransform = transform;
            UIcam = GetComponentsInChildren<Camera>()[1];
            UIcamLight = UIcam.GetComponentInChildren<Light>();
            UIcam.enabled = false;
            UIcamLight.enabled = false;
        }

        public void FollowPlayer(float delta)
        {
            Vector3 currentVelocity = Vector3.zero;
            frameTransform.position = Vector3.SmoothDamp(frameTransform.position, playerTransform.position, 
                ref currentVelocity,delta/followSpeed);
        }

        public void HandleRotation(float delta)
        {
            frameTransform.RotateAround(frameTransform.position, Vector3.up, InputHandler.instance.MouseX* lookSpeed*delta);

            float pivotAngle = -Mathf.Clamp(InputHandler.instance.MouseY * pivotSpeed, -maximumPivot, maximumPivot);
            pivotTransform.Rotate(pivotAngle*delta,0f,0f,Space.Self);
        }

        public void HandleCamera(float delta)
        {
            FollowPlayer(delta);
            HandleRotation(delta);
        }
    }

}
