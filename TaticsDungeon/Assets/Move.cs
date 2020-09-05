using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class Move : SkillAbstract
    {
        public TaticalMovement taticalMovement;

        public override void AttachToCharacter(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement)
        {
            taticalMovement = _taticalMovement;
        }

        public override void Activate(float delta)
        {
            taticalMovement.ExcuteMovement(delta);
        }

        public override void Excute(float delta, GridCell targetCell)
        {
        }
    }
}
