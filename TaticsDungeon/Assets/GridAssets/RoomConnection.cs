using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class RoomConnection : MonoBehaviour
    {
        public IntVector2 direction;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(inputRay, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                        GridManager.Instance.TranistionRoom(direction);
                }
            }

        }
    }
}

