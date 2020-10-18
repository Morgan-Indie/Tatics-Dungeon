using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class HitReactionAnimation : MonoBehaviour
    {
        public AnimationHandler animationHandler;
        public CharacterStateManager stateManager;

        // Start is called before the first frame update
        void Start()
        {
            animationHandler = GetComponent<AnimationHandler>();
            stateManager = GetComponent<CharacterStateManager>();
        }

        public void ReactionAnimation()
        {
            if (stateManager.characterAction == CharacterAction.LyingDown)
                animationHandler.PlayTargetAnimation("LyingHitReaction");
            else
                animationHandler.PlayTargetAnimation("MinorHitReaction");
        }
    }
}

