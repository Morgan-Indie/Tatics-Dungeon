using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class UICam : MonoBehaviour
    {
        // Start is called before the first frame update
        public Transform cameraTransform;
        public Vector3 offset = new Vector3(.77f, -2.14f, 5.86f);
        public Camera cam;
        public LayerMask mask;

        public void Start()
        {
            cam = GetComponent<Camera>();
        }

        public void HandleUICam()
        {
            switch(GameManager.instance.currentCharacter.playerNumber)
            {                
                case (PlayerNumber.player1):
                    mask = (1<<9);
                    break;
                case (PlayerNumber.player2):
                    mask = (1 << 10);
                    break;
                case (PlayerNumber.player3):
                    mask = (1 << 11);
                    break;
                case (PlayerNumber.player4):
                    mask = (1 << 12);
                    break;

            }

            cam.cullingMask = mask;

            Transform targetTransform = GameManager.instance.currentCharacter.transform;
            Vector3 targetPosition = targetTransform.position + targetTransform.forward*2.5f + offset;
            transform.position = targetPosition;
            transform.LookAt(GameManager.instance.currentCharacter.transform.position);
        }
    }
}

