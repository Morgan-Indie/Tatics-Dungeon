using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CameraController : MonoBehaviour
    {
        public Transform pivot;
        public float cameraSpeed = 10f;
        public float scrollSense = 10f;
        public float rotSense = 10f;

        private void Update()
        {
            if (Input.GetAxis("Horizontal") > 0.5f)
            {
                transform.position = transform.position + transform.right * cameraSpeed * Time.deltaTime;
            }
            else if (Input.GetAxis("Horizontal") < -0.5f)
            {
                transform.position = transform.position - transform.right * cameraSpeed * Time.deltaTime;
            }
            Vector3 forwardDirection = transform.forward;
            forwardDirection.y = 0;
            forwardDirection.Normalize();
            if (Input.GetAxis("Vertical") > 0.5f)
            {
                transform.position = transform.position + forwardDirection * cameraSpeed * Time.deltaTime;
            }
            else if (Input.GetAxis("Vertical") < -0.5f)
            {
                transform.position = transform.position - forwardDirection * cameraSpeed * Time.deltaTime;
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0.05f)
            {
                transform.position = transform.position + transform.forward * scrollSense * Time.deltaTime;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < -0.05f)
            {
                transform.position = transform.position - transform.forward * scrollSense * Time.deltaTime;
            }

            if (Input.GetMouseButton(2))
            {
                pivot.SetParent(null);
                transform.Translate(Vector3.right * Input.GetAxis("Mouse X") * rotSense);
                transform.Translate(Vector3.down * Input.GetAxis("Mouse Y") * rotSense);
                transform.LookAt(pivot);
                pivot.SetParent(transform);
            }
        }
    }

}
