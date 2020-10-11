using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class Move : CastPhysical
    {
        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill, SkillSlot _slot)
        {
            Move move = _characterStats.gameObject.AddComponent<Move>();
            move.characterStats = _characterStats;
            move.animationHandler = _animationHandler;
            move.taticalMovement = _taticalMovement;
            move.skill = _skill;
            move.combatUtils = _combatUtils;
            move.slot = _slot;
            return move;
        }

        public override void Cast(float delta, IntVector2 targetIndex)
        {
            
        }

        public override void Excute(float delta, GridCell targetCell)
        {
        }

        public override void Excute(float delta)
        {
        }
    }
}
