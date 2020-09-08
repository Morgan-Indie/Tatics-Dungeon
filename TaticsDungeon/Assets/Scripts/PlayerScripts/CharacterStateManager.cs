using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public enum CharacterState
    {
        Ready,InMenu, IsInteracting,Dead
    }

    public enum StatusEffect
    {
        None, Burning, Frozen, Shocked, Poisoned, Cursed, Blessed,Wet,Oiled
    }

    public enum CharacterAction
    {
        None, Attacking, Moving,LyingDown, ShieldCharge
    }

    public class CharacterStateManager : MonoBehaviour
    {
        [Header("Character State Flags")]
        public CharacterState characterState = CharacterState.Ready;
        public CharacterAction characterAction = CharacterAction.None;
        public CharacterStats characterStats;
        public bool skillColliderTiggered = false;
        public List<StatusEffect> statusEffects = new List<StatusEffect>();
        public CombatStat burnDamageOverTime = new CombatStat(0f,CombatStatType.fireDamage);
        public CombatStat poisonDamageOverTime = new CombatStat(0f, CombatStatType.poisonDamage);
        public Dictionary<Object, (CombatStatType,int)> DamageSourceTurns = new Dictionary<Object, (CombatStatType, int)>();

        public void Start()
        {
            characterStats = GetComponent<CharacterStats>();
        }

        public void UpdateTurns()
        {
            foreach (var damageSources in DamageSourceTurns)
            {
                DamageSourceTurns[damageSources.Key] = (DamageSourceTurns[damageSources.Key].Item1,
                    DamageSourceTurns[damageSources.Key].Item2-1);
                if (DamageSourceTurns[damageSources.Key].Item2 == 0)
                {
                    switch (DamageSourceTurns[damageSources.Key].Item1)
                    {
                        case CombatStatType.fireDamage:
                            burnDamageOverTime.RemoveAllModifiersFromSource(damageSources.Key);
                            if (!statusEffects.Contains(StatusEffect.Burning))
                            {
                                Destroy(GameObject.FindGameObjectWithTag("FireEffect"));
                                Destroy(GameObject.FindGameObjectWithTag("InfernoEffect"));
                            }
                            break;
                        case CombatStatType.poisonDamage:
                            poisonDamageOverTime.RemoveAllModifiersFromSource(damageSources.Key);
                            break;                        
                    }

                    DamageSourceTurns.Remove(damageSources.Key);
                }
            }
            if (poisonDamageOverTime.Value>0)
                characterStats.TakeDamage((int)poisonDamageOverTime.Value);
            if (burnDamageOverTime.Value > 0)
                characterStats.TakeDamage((int)burnDamageOverTime.Value-(int)characterStats.resistance.Value);
        }
    }
}

