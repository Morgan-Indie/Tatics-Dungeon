using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class IsometricCamera : MonoBehaviour
    {
        public float cameraSensitivity = 10f;
        public float cameraSpeed = 10f;
        public float rotateSpeed = 100f;

        public float zoomSpeed = 10f;
        public float maxZoom = 3f, minZoom=10f;
        public static IsometricCamera instance = null;
        public Quaternion pivotRotation;
        public Quaternion lookRotation;
        public Camera isometricCamera;
        // Start is called before the first frame update

        private void Awake()
        {
            if (instance == null)
                instance = this;

            isometricCamera = GetComponent<Camera>();
            isometricCamera.enabled = false;
        }

        public void OnEnable()
        {

        }

        public void FocusOnCurrentPlayer()
        {
            Vector3 currentVelocity = Vector3.zero;
            Transform playerTransform = GameManager.instance.currentCharacter.transform;
            Vector3 targetPoistion = playerTransform.position + Vector3.up * 5 - playerTransform.forward * 2;
            targetPoistion.y = transform.position.y;
            transform.position = Vector3.SmoothDamp(transform.position,targetPoistion,ref currentVelocity, Time.deltaTime / cameraSpeed);
            transform.LookAt(playerTransform.position);
        }

        public void FocusOnCurrentEnemy()
        {
            Vector3 currentVelocity = Vector3.zero;
            Transform enemyTransform = GameManager.instance.currentEnemy.transform;
            Vector3 targetPoistion = enemyTransform.position + Vector3.up * 5 - enemyTransform.forward * 2;
            targetPoistion.y = transform.position.y;
            transform.position = Vector3.SmoothDamp(transform.position, targetPoistion, ref currentVelocity, Time.deltaTime/ cameraSpeed);
            transform.LookAt(enemyTransform.position);
        }

        public void HandleCameraMove(float delta)
        {
            Vector3 currentVelocity = Vector3.zero;
            Vector3 camUp = transform.up;
            camUp.y = 0;
            camUp.Normalize();
            Vector3 moveDirection = (camUp * InputHandler.instance.MoveY + transform.right * InputHandler.instance.MoveX).normalized;
            transform.position = Vector3.SmoothDamp(transform.position,
                transform.position+ moveDirection * cameraSensitivity*delta,
                ref currentVelocity, delta / cameraSpeed);
        }

        public void HandleZoom(float delta)
        {
            Vector3 refVelocity = Vector3.zero;
            if (InputHandler.instance.rightTriggerInput)
            {
                Vector3 zoomPos = Vector3.down * zoomSpeed * delta+transform.position;
                zoomPos.y = Mathf.Clamp(zoomPos.y, maxZoom, minZoom);
                transform.position = Vector3.SmoothDamp(transform.position,
                                        zoomPos, ref refVelocity, delta / cameraSpeed);
            }
                                 

            else if (InputHandler.instance.leftTriggerInput)
            {
                Vector3 zoomPos = Vector3.up * zoomSpeed * delta + transform.position;
                zoomPos.y = Mathf.Clamp(zoomPos.y, maxZoom, minZoom);
                transform.position = Vector3.SmoothDamp(transform.position,
                                        zoomPos, ref refVelocity, delta / cameraSpeed);
            }    
        }

        public void HandleRotation(float delta)
        {
            if (InputHandler.instance.rightShoulderInput)
                transform.Rotate(0f, rotateSpeed * delta, 0f, Space.World);
            else if (InputHandler.instance.leftShoulderInput)
                transform.Rotate(0f, -rotateSpeed * delta, 0f, Space.World);
        }

        public void HandleCamera(float delta)
        {
            HandleCameraMove(delta);
            HandleZoom(delta);
            HandleRotation(delta);
        }
    }

}
