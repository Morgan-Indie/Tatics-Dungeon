using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class Move : SkillAbstract
    {
        public override void Activate(CharacterStats characterStats,
                    AnimationHandler animationHandler, TaticalMovement taticalMovement,
                    float delta)
        {
            taticalMovement.ExcuteMovement(delta);
        }

        public override void Excute(CharacterStats characterStats,
            AnimationHandler animationHandler, TaticalMovement taticalMovement,
            float delta, GridCell targetCell)
        {
        }
    }
}
