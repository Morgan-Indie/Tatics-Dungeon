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

        public abstract void AttachToCharacter(CharacterStats characterStats,
            AnimationHandler animationHandler, TaticalMovement taticalMovement);

        public abstract void Activate(float delta);

        public abstract void Excute(float delta, GridCell targetCell);
    }
}

