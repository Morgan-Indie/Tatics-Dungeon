using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class EnemyController : FSM
    {
        public CharacterStats characterStats;
        public CharacterStateManager stateManager;
        public AnimationHandler animationHandler;
        public TaticalMovement taticalMovement;
        public Dictionary<SkillType, Skill> skillDict;
        public DogDudeSkillList skillList;

        public override void Initialize()
        {
            characterStats = GetComponent<CharacterStats>();
            stateManager = GetComponent<CharacterStateManager>();
            taticalMovement = GetComponent<TaticalMovement>();
            animationHandler = GetComponent<AnimationHandler>();
            skillList = GetComponent<DogDudeSkillList>();
            skillDict = skillList.skillDict;
            ConstructFSM();
        }

        public override void FSMFixedUpdate(float delta)
        {
            CurrentState.HandleTransitions();
            CurrentState.Act(delta);
        }

        public void SetTransition(Transition t)
        {
            PerformTransition(t);
        }

        private void ConstructFSM()
        {
            AggressiveState aggState = new AggressiveState(characterStats, 
                stateManager,taticalMovement,animationHandler,skillDict);
            AddFSMState(aggState);
        }
    }
}
