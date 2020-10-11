using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PrototypeGame
{
    public class EnemyController : MonoBehaviour
    {
        public CharacterStats characterStats;
        public CharacterStateManager stateManager;
        public AnimationHandler animationHandler;
        public TaticalMovement taticalMovement;
        public AISkillSlotHandler skillHandler;
        public DamageSkill damageSkill;
        public bool UsingSkill = false;
        public SkillAbstract selectedSkill = null;

        public void Start()
        {
            characterStats = GetComponent<CharacterStats>();
            stateManager = GetComponent<CharacterStateManager>();
            taticalMovement = GetComponent<TaticalMovement>();
            animationHandler = GetComponent<AnimationHandler>();
            skillHandler = GetComponent<AISkillSlotHandler>();
            damageSkill = GetComponent<DamageSkill>();
        }

        public void SelectSkill()
        {
            int choice = Random.Range(0, skillHandler.skills.Count - 1);
            selectedSkill = skillHandler.skills[choice];
        }

        public void Act(float delta)
        {

            damageSkill.CastSkill(selectedSkill, Time.deltaTime);
        }
    }
}
