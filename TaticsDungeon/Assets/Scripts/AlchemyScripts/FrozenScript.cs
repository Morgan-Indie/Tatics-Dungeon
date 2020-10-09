using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FrozenScript : VFXEffectScript
    {
        public CharacterStateManager stateManager;
        public SkillSlotsHandler skillSlots;
        public AnimationHandler animationHandler;
        public Rigidbody characterRigidBody;
        public TaticalMovement taticalMovement;

        void Start()
        {
            stateManager = gameObject.GetComponentInParent<CharacterStateManager>();
            skillSlots = gameObject.GetComponentInParent<SkillSlotsHandler>();
            animationHandler = gameObject.GetComponentInParent<AnimationHandler>();
            characterRigidBody = gameObject.GetComponentInParent<Rigidbody>();
            taticalMovement = gameObject.GetComponentInParent<TaticalMovement>();
            stateManager.characterState = CharacterState.Disabled;


            if (stateManager.characterAction == CharacterAction.Moving || stateManager.characterAction == CharacterAction.ShieldCharge)
            {
                characterRigidBody.constraints = RigidbodyConstraints.FreezeAll;
                taticalMovement.currentPathIndex = 0;
                taticalMovement.SetCurrentNavDict();
                characterRigidBody.velocity = Vector3.zero;
                stateManager.characterAction = CharacterAction.None;
            }

            animationHandler.PlayTargetAnimation("Frozen");
            if (stateManager.gameObject.tag == "Player")
                skillSlots.DisableAllSlots();
        }

        void OnDestroy()
        {
            stateManager.characterState = CharacterState.Ready;
            animationHandler.PlayTargetAnimation("CombatIdle");
        }

        public override void ActivateEffect()
        {

        }
    }
}

