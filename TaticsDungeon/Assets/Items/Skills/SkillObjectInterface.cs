﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class SkillObjectInterface : MonoBehaviour
    {
        public SkillAbstract skillScript;

        public void Activate(CharacterStats characterStats,AnimationHandler animationHandler, 
            TaticalMovement taticalMovement,float delta)
        {
            skillScript.Activate( characterStats,animationHandler,  taticalMovement,  delta);
        }

        public void Excute(CharacterStats characterStats,AnimationHandler animationHandler,
            TaticalMovement taticalMovement,float delta, GridCell targetCell)
        {
            skillScript.Excute(characterStats,animationHandler, taticalMovement, delta, targetCell);
        }
    }
}
