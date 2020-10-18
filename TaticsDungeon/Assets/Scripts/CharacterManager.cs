using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypeGame
{
    public abstract class CharacterManager : MonoBehaviour
    {
        public AnimationHandler animationHandler;
        public CharacterStats stats;
        public CharacterCombatStats combatStats;
        public CharacterAP AP;
        public CharacterHealth health;
        public CharacterLocation location;
        public bool isCurrentCharacter;
        public Text panelName;
        public CharacterStateManager stateManager;

        public abstract void HandleDeath();
        public abstract void CharacterUpdate(float delta);
        public abstract void DisableCharacter();
        public SkillAbstract selectedSkill = null;
    }
}

