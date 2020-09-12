using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CallExcute : MonoBehaviour
    {
        public PlayerManager playerManager;
        public CastPhysical _selectedSkill;

        public void Start()
        {
            playerManager = GetComponent<PlayerManager>();
        }

        public void ExcuteSkill()
        {
            _selectedSkill=(CastPhysical)playerManager.selectedSkill;
            _selectedSkill.Excute(Time.deltaTime);
        }
    }
}

