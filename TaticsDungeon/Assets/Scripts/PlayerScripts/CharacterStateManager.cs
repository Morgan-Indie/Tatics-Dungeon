using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CharacterStateManager : MonoBehaviour
    {
        [Header("Character State Flags")]
        public string characterState = "ready";
        public string characterAction = "None";
        public bool skillColliderTiggered = false;
    }
}

