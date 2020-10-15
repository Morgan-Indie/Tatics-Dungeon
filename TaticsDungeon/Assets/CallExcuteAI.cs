using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CallExcuteAI : MonoBehaviour
    {
        public EnemyController controller;
        public CastPhysical _selectedSkill;

        public void Start()
        {
            controller = GetComponent<EnemyController>();
        }

        public void ExcuteSkill()
        {
            _selectedSkill = (CastPhysical)controller.selectedSkill;
            _selectedSkill.Excute(Time.deltaTime);
        }
    }
}

