using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PrototypeGame
{
    public enum CharacterState
    {
        Ready,InMenu, IsInteracting,Dead, Disabled
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
        public List<int> statusEffects = new List<int>();
        public Dictionary<int, int> statusTurns = new Dictionary<int, int>();
        public Dictionary<StatusEffect, GameObject> statusVFXDict = new Dictionary<StatusEffect, GameObject>();
        public Dictionary<AlchemicalState, GameObject> stateVFXDict = new Dictionary<AlchemicalState, GameObject>()
        {
            {AlchemicalState.liquid,null },{AlchemicalState.solid,null}
        };

        public Dictionary<AlchemicalState, AlchemicalSubstance> characterSubstances; 
        public Dictionary<object, (CombatStatType,int)> DamageSourceTurns = new Dictionary<object, (CombatStatType, int)>();
        public HeatState heatState;

        public void Start()
        {
            characterStats = GetComponent<CharacterStats>();
            heatState = new HeatState(HeatValue.neutral);
            characterSubstances = new Dictionary<AlchemicalState, AlchemicalSubstance>()
            {
                {AlchemicalState.gas,new AlchemicalSubstance(AlchemicalState.None) },
                {AlchemicalState.liquid,new AlchemicalSubstance(AlchemicalState.None) },
                {AlchemicalState.solid,new AlchemicalSubstance(AlchemicalState.None) },
            };
        }

        public void AddStatus(StatusEffect status)
        {
            if (!statusEffects.Contains((int)status))
            {
                statusEffects.Add((int)status);
                statusTurns.Add((int)status, AlchemyEngine.instance.statusTurnsDict[status]);
            }
        }


        public void RemoveStatus(StatusEffect status)
        {
            if (statusEffects.Contains((int)status))
            {
                statusEffects.Remove((int)status);
                statusTurns.Remove((int)status);
                if (statusVFXDict.ContainsKey(status))
                {
                    Destroy(statusVFXDict[status]);
                    statusVFXDict.Remove(status);
                }
            }
        }

        public void UpdateTurns()
        {
            foreach (int key in statusTurns.Keys.ToList())
            {
                statusVFXDict[(StatusEffect)key].GetComponent<VFXEffectScript>().ActivateEffect();
                statusTurns[key] -= 2;
                if (statusTurns[key]<=0)
                {
                    RemoveStatus((StatusEffect)key);
                }
            }

            foreach (AlchemicalState key in stateVFXDict.Keys.ToList())
            {
                if (stateVFXDict[key]!=null)
                {
                    characterSubstances[key].turnsLeft -= 2;
                    if (characterSubstances[key].turnsLeft <= 0)
                    {
                        Destroy(stateVFXDict[key]);
                        stateVFXDict[key] = null;
                        characterSubstances[key]= new AlchemicalSubstance(AlchemicalState.None);
                    }
                }
            }
        }
    }
}
