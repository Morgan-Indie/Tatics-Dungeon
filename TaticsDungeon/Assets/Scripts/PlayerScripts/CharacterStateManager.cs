using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        public int burnDamage;

        public void Start()
        {
            characterStats = GetComponent<CharacterStats>();
            burnDamage = (int)burnDamageOverTime.Value;

        }
        //Called once at turn start 
    public void UpdateTurns()
        {
            foreach (Object damageSource in DamageSourceTurns.Keys.ToArray())
            {
                DamageSourceTurns[damageSource] = (DamageSourceTurns[damageSource].Item1,
                    DamageSourceTurns[damageSource].Item2-1);
                if (DamageSourceTurns[damageSource].Item2 == 0)
                {
                    switch (DamageSourceTurns[damageSource].Item1)
                    {
                        case CombatStatType.fireDamage:
                            burnDamageOverTime.RemoveAllModifiersFromSource(damageSource);
                            if (statusEffects.Contains(StatusEffect.Burning))
                            {
                                Destroy(GameObject.FindGameObjectWithTag("FireEffect"));
                                Destroy(GameObject.FindGameObjectWithTag("InfernoEffect"));
                            }
                            break;
                        case CombatStatType.poisonDamage:
                            poisonDamageOverTime.RemoveAllModifiersFromSource(damageSource);
                            break;                        
                    }

                    DamageSourceTurns.Remove(damageSource);
                }
            }

            if (poisonDamageOverTime.Value>0)
                characterStats.TakeDamage((int)poisonDamageOverTime.Value);
            if (burnDamageOverTime.Value > 0)
            {
                Debug.Log("here");
                characterStats.TakeDamage((int)burnDamageOverTime.Value - (int)characterStats.resistance.Value);
            }
        }
    }
}

