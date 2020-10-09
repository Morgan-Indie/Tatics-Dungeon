using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class InfernoScript : VFXEffectScript
    {
        public CombatUtils combatUtils;
        public CharacterStateManager stateManager;
        public CharacterStats characterStats;

        public void Start()
        {
            stateManager = gameObject.GetComponentInParent<CharacterStateManager>();
            combatUtils = gameObject.GetComponentInParent<CombatUtils>();
            characterStats = gameObject.GetComponentInParent<CharacterStats>();
        }

        public override void ActivateEffect()
        {
            combatUtils.DealDamage(characterStats, new DamageStruct(0, 0, 20f, CombatStatType.fireDamage));
        }
    }
}
