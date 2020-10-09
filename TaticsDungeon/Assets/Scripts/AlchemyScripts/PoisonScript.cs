using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class PoisonScript : VFXEffectScript
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
            float damage =characterStats.maxHealth * .1f;
            combatUtils.DealDamage(characterStats, new DamageStruct(0, 0, damage, CombatStatType.poisonDamage));
        }
    }
}
