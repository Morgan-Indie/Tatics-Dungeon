using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public abstract class CastPhysical : SkillAbstract
    {
        public float scaleValue;
        public AttributeType scaleType;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
                        TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill)
        { return null; }

        public override void Cast(float delta, IntVector2 targetIndex) { Debug.Log("Need To Implement This Method In Child"); }
    }
}