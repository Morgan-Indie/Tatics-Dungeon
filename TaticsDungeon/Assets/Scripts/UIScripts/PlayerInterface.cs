using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class PlayerInterface : MonoBehaviour
    {
        public GameObject player;

        public void SetPlayerInventory(GameObject attachedPlayer)
        {
            player = attachedPlayer;
        }

    }

}
