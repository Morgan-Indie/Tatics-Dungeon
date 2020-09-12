using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class AttackAnimationEvent : MonoBehaviour
    {
        public MeleeAttack meleeAttack;

        public void CallExcute()
        {
            meleeAttack.Excute(Time.deltaTime);
        }
    }
}

