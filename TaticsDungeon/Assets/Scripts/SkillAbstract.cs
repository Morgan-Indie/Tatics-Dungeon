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

    public abstract class SkillAbstract : MonoBehaviour
    {
        public Skill skill;

        public abstract void Activate(CharacterStats characterStats, 
            AnimationHandler animationHandler, TaticalMovement taticalMovement, 
            float delta);

        public abstract void Excute(CharacterStats characterStats,
            AnimationHandler animationHandler, TaticalMovement taticalMovement,
            float delta, GridCell targetCell);
    }
}

