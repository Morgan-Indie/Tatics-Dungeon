using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public enum SkillType
    {
        PyhsicalAttack,
        Move,
        Fire,
        Water,
        Poison,
        Shock,
        Curse,
        Bless,
        Castable,
        Pinned,
        Chill
    }

    public abstract class SkillAbstract:MonoBehaviour
    {
        public Skill skill;
        public CharacterStats characterStats;
        public AnimationHandler animationHandler;
        public TaticalMovement taticalMovement;
        public CombatStat normalDamage;
        public CombatStat peirceDamage;
        public CombatStat alchemicalDamage;
        public CombatStat armor;
        public CombatStat resistance;

        public abstract SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, Skill _skill);

        public abstract void Activate(float delta);

        public abstract void Excute(float delta, GridCell targetCell);
    }
}

