﻿using System.Collections;
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
        None, Burning, Frozen, Shocked, Poisoned, Cursed, Blessed,Wet,Oiled, Electricuted,Inferno
    }

    public enum CharacterAction
    {
        None, Attacking, Moving,LyingDown, ShieldCharge
    }

    public enum PassiveActions
    {
        Guardian,
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
        public Dictionary<object, (CombatStatType,int)> DamageSourceTurns = new Dictionary<object, (CombatStatType, int)>();
        public int burnDamage;
        public int poisonDamage;
        public Dictionary<PassiveActions,int> passiveActions = new Dictionary<PassiveActions, int>();

        public void Start()
        {
            characterStats = GetComponent<CharacterStats>();
            burnDamage = (int)burnDamageOverTime.Value;
            poisonDamage = (int)poisonDamageOverTime.Value;
        }

        public void UpdateStatusDamageTurns()
        {
            foreach (Object damageSource in DamageSourceTurns.Keys.ToArray())
            {
                DamageSourceTurns[damageSource] = (DamageSourceTurns[damageSource].Item1,
                    DamageSourceTurns[damageSource].Item2 - 1);

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
        }

        public void UpdatePassiveActions()
        {
            foreach (PassiveActions action in passiveActions.Keys.ToArray())
            {
                passiveActions[action] -= 1;
                if (passiveActions[action] == 0)
                    passiveActions.Remove(action);
            }
        }

        public void UpdateTurns()
        {
            UpdateStatusDamageTurns();
            if (poisonDamageOverTime.Value>0)
                characterStats.TakeDamage((int)poisonDamageOverTime.Value - (int)characterStats.poisonResistance.Value);
            if (burnDamageOverTime.Value > 0)
            {
                characterStats.TakeDamage((int)burnDamageOverTime.Value - (int)characterStats.fireResistance.Value);
            }
            UpdatePassiveActions();
        }
    }
}

