using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public enum SkillType
    {
        MeleeAttack,
        Move,
        ShieldCharge,
        RangeAttack,
        Castable
    }

    public abstract class SkillAbstract:MonoBehaviour
    {
        public Skill skill;
        public CharacterStats characterStats;
        public AnimationHandler animationHandler;
        public TaticalMovement taticalMovement;

        public abstract SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, Skill _skill);

        public abstract void Activate(float delta);

        public abstract void Excute(float delta, GridCell targetCell);
    }
}

