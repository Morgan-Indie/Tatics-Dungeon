using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public interface ISkill
    {
        void Activate(CharacterStats characterStats,
            AnimationHandler animationHandler,
            TaticalMovement taticalMovement, Skill skill,
            float delta);
    }
}

