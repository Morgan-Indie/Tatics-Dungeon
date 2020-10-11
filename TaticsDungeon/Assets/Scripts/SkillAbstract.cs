using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public abstract class SkillAbstract:MonoBehaviour
    {
        public Skill skill;
        public CharacterStats characterStats;
        public AnimationHandler animationHandler;
        public TaticalMovement taticalMovement;
        public CombatUtils combatUtils;
        public CombatStat normalDamage;
        public CombatStat peirceDamage;
        public CombatStat alchemicalDamage;
        public CombatStatType alchemicalDamgeType;
        public CombatStat armor;
        public CombatStat resistance;
        public HeatState heatState;
        public string skillAnimation;
        public SkillSlot slot;

        IntVector2 prevIndex;
        
        public void Activate(float delta)
        {
            IntVector2 index = taticalMovement.GetMouseIndex();
            if (!index.Equals(prevIndex))
                GridManager.Instance.HighlightCastableRange(taticalMovement.currentIndex, index, skill);
            int distance = taticalMovement.currentIndex.GetDistance(index);

            if (index.x >= 0 && characterStats.currentAP >= skill.APcost && 
                index.GetDistance(taticalMovement.currentIndex) <= skill.castableSettings.range)
            {
                if (Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput &&
                    characterStats.stateManager.characterState != CharacterState.IsInteracting)
                {
                    InputHandler.instance.tacticsXInput = false;
                    Cast(delta, index);
                }
            }
            prevIndex = index;
        }

        public abstract SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
                        TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill, SkillSlot _slot);
        public abstract void Cast(float delta, IntVector2 targetIndex);
        public abstract void Excute(float delta, GridCell targetCell);
    }
}
