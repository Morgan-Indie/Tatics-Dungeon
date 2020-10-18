using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public abstract class SkillAbstract:MonoBehaviour
    {
        public Skill skill;
<<<<<<< Updated upstream
        public CharacterStats characterStats;
        public AnimationHandler animationHandler;
        public TaticalMovement taticalMovement;
        public CombatUtils combatUtils;
        public CombatStat normalDamage;
        public CombatStat peirceDamage;
        public CombatStat alchemicalDamage;
        public CombatStat armor;
        public CombatStat resistance;
=======
        public List<DamageStat> damageStatsList;
        public HeatState heatState;
        public string skillAnimation;
        public SkillSlot slot;
        public CharacterManager character;
        public float scaleMultiplier;
>>>>>>> Stashed changes

        public void Activate(float delta)
        {
<<<<<<< Updated upstream
            IntVector2 index = taticalMovement.GetMouseIndex();
            GridManager.Instance.HighlightCastableRange(taticalMovement.currentIndex, index, skill);
            int distance = taticalMovement.currentIndex.GetDistance(index);

            if (index.x >= 0 && characterStats.currentAP >= skill.APcost)
=======
            IntVector2 index = MovementUtils.Instance.GetMouseIndex();
            if (!index.Equals(prevIndex))
                GridManager.Instance.HighlightCastableRange(character.location.currentIndex, index, skill);
            int distance = character.location.currentIndex.GetDistance(index);

            if (index.x >= 0 && character.AP.currentAP >= skill.APcost && 
                index.GetDistance(character.location.currentIndex) <= skill.castableSettings.range)
>>>>>>> Stashed changes
            {
                if (Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput &&
                    character.stateManager.characterState != CharacterState.IsInteracting)
                {
                    InputHandler.instance.tacticsXInput = false;
                    Cast(delta, index);
                }
            }
        }

<<<<<<< Updated upstream
        public abstract SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
                        TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill);
=======
        public abstract SkillAbstract AttachSkill(CharacterManager _character, Skill _skill, SkillSlot _slot);
>>>>>>> Stashed changes
        public abstract void Cast(float delta, IntVector2 targetIndex);
        public abstract void Excute(float delta, GridCell targetCell);
    }
}
