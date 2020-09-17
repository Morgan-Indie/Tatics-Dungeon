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
        None, Burning, Inferno, Frozen, Shocked, Poisoned, Cursed, Blessed, Wet , Oiled, Electricuted
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
        public Dictionary<StatusEffect, int> statusTimers = new Dictionary<StatusEffect, int>();
        public Dictionary<StatusEffect, GameObject> statusVFX = new Dictionary<StatusEffect, GameObject>();
        public CombatStat burnDamageOverTime = new CombatStat(0f,CombatStatType.fireDamage);
        public CombatStat poisonDamageOverTime = new CombatStat(0f, CombatStatType.poisonDamage);
        public Dictionary<object, (CombatStatType,int)> DamageSourceTurns = new Dictionary<object, (CombatStatType, int)>();
        public int burnDamage;
        public int poisonDamage;

        public void Start()
        {
            characterStats = GetComponent<CharacterStats>();
            burnDamage = (int)burnDamageOverTime.Value;
            poisonDamage = (int)poisonDamageOverTime.Value;
        }

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
                characterStats.TakeDamage((int)poisonDamageOverTime.Value - (int)characterStats.poisonResistance.Value);
            if (burnDamageOverTime.Value > 0)
            {
                characterStats.TakeDamage((int)burnDamageOverTime.Value - (int)characterStats.fireResistance.Value);
            }

            IncrementStatuses();
        }

        public void IncrementStatuses()
        {
            List<StatusEffect> removes = new List<StatusEffect>();
            foreach (StatusEffect status in statusEffects)
            {
                statusTimers[status]--;
                if (statusTimers[status] <= 0)
                {
                    Destroy(statusVFX[status]);
                    statusVFX.Remove(status);
                    statusTimers.Remove(status);
                    removes.Add(status);
                }
            }
            foreach (StatusEffect status in removes) { statusEffects.Remove(status); }
        }

        public void AddStatus(StatusEffect status, int turns)
        {
            if (!statusEffects.Contains(status))
            {
                statusEffects.Add(status);
                statusTimers.Add(status, turns);
                GameObject newEffect = null;
                switch (status)
                {
                    case StatusEffect.Burning: newEffect = ActivateVFX.Instance.FireVFX; break;
                    case StatusEffect.Wet: newEffect = ActivateVFX.Instance.WetVFX; break;
                    case StatusEffect.Oiled: newEffect = ActivateVFX.Instance.OilVFX; break;
                    case StatusEffect.Inferno: newEffect = ActivateVFX.Instance.InfernoVFX; break;
                    case StatusEffect.Poisoned: newEffect = ActivateVFX.Instance.PoisonVFX; break;
                    case StatusEffect.Shocked: newEffect = ActivateVFX.Instance.ShockVFX; break;
                }
                if (newEffect != null)
                {
                    GameObject ob = Instantiate(newEffect, transform.position, Quaternion.identity);
                    ob.transform.SetParent(transform);
                    statusVFX.Add(status, ob);
                }
            } else
            {
                statusTimers[status] = turns;
            }
        }

        public void RemoveStatus(StatusEffect status)
        {
            if (statusEffects.Contains(status))
            {
                statusEffects.Remove(status);
                statusTimers.Remove(status);
                statusVFX.Remove(status);
            }
        }

        public void ApplyHeat()
        {
            bool statusChanged = false;
            if (statusEffects.Contains(StatusEffect.Wet))
            {
                RemoveStatus(StatusEffect.Wet);
                statusChanged = true;
            }
            if (statusEffects.Contains(StatusEffect.Oiled))
            {
                RemoveStatus(StatusEffect.Oiled);
                AddStatus(StatusEffect.Inferno, AlchemyManager.Instance.playerInfernoTime);
                statusChanged = true;
            }
            if (statusEffects.Contains(StatusEffect.Frozen))
            {
                RemoveStatus(StatusEffect.Frozen);
                AddStatus(StatusEffect.Wet, AlchemyManager.Instance.playerWetTime);
                statusChanged = true;
            }
            if (!statusChanged)
                AddStatus(StatusEffect.Burning, AlchemyManager.Instance.playerBurnTime);
        }

        public void ApplyChill()
        {
           // bool statusChanged = false;
            if (statusEffects.Contains(StatusEffect.Burning))
                RemoveStatus(StatusEffect.Burning);
                //statusChanged = true;
            if (statusEffects.Contains(StatusEffect.Inferno))
                RemoveStatus(StatusEffect.Inferno);
                //statusChanged = true;
            if (statusEffects.Contains(StatusEffect.Wet))
                RemoveStatus(StatusEffect.Wet);
                AddStatus(StatusEffect.Frozen, AlchemyManager.Instance.playerFrozenTime);
               // statusChanged = true;

        }

        public void ApplyWet()
        {
            bool applyWater = true;
            if (statusEffects.Contains(StatusEffect.Burning))
                RemoveStatus(StatusEffect.Burning);
                applyWater = false;
            if (statusEffects.Contains(StatusEffect.Inferno))
                RemoveStatus(StatusEffect.Inferno);
                applyWater = false;
            if (statusEffects.Contains(StatusEffect.Oiled))
                RemoveStatus(StatusEffect.Oiled);
            if (applyWater)
                AddStatus(StatusEffect.Wet, AlchemyManager.Instance.playerWetTime);
        }

        public void ApplyOil()
        {
            bool applyOil = true;
            if (statusEffects.Contains(StatusEffect.Inferno))
                applyOil = false;
            if (statusEffects.Contains(StatusEffect.Burning))
                RemoveStatus(StatusEffect.Burning);
                AddStatus(StatusEffect.Inferno, AlchemyManager.Instance.playerInfernoTime);
                applyOil = false;
            if (statusEffects.Contains(StatusEffect.Wet))
                RemoveStatus(StatusEffect.Wet);
            if (applyOil)
                AddStatus(StatusEffect.Oiled, AlchemyManager.Instance.playerOilTime);
        }

        public void ApplyPoison()
        {
            AddStatus(StatusEffect.Poisoned, AlchemyManager.Instance.playerPoisonTime);
        }

        public void ApplyShock()
        {
            AddStatus(StatusEffect.Shocked, AlchemyManager.Instance.playerShockTime);
        }
    }
}
