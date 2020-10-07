using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PrototypeGame
{
    public enum AlchemicalState { solid, liquid, gas, None}
    public enum HeatValue { hot=1,cold=-1,neutral = 0};
    public enum StatusEffect
    {
        Burning, Inferno, Frozen, Shocked, Poisoned, Cursed, Blessed, Wet, Oiled, Electricuted,Chilled,None
    }

    public struct AlchemicalSubstance
    {
        public AlchemicalState alchemicalState;
        public List<int> auxiliaryStates;
        public int turnsLeft;
        public Dictionary<StatusEffect, int> statusTurns;

        public AlchemicalSubstance(AlchemicalState _alchemicalState)
        {
            alchemicalState = _alchemicalState;
            auxiliaryStates = new List<int>();
            turnsLeft = AlchemyEngine.instance.substanceTurnsDict[_alchemicalState];
            statusTurns = new Dictionary<StatusEffect, int>();
        }

        public AlchemicalSubstance(AlchemicalSubstance substance)
        {
            alchemicalState = substance.alchemicalState;
            auxiliaryStates = substance.auxiliaryStates;
            turnsLeft = substance.turnsLeft;
            statusTurns = substance.statusTurns;
        }

        public void AddAuxState(StatusEffect _auxiliaryState)
        {            
            auxiliaryStates.Add((int)_auxiliaryState);
            statusTurns[_auxiliaryState] = AlchemyEngine.instance.statusTurnsDict[_auxiliaryState];
        }

        public AlchemicalSubstance TransformState(int _alchemicalState)
        {
            alchemicalState = (AlchemicalState)_alchemicalState;
            turnsLeft = AlchemyEngine.instance.substanceTurnsDict[alchemicalState];
            return this;
        }

        public AlchemicalSubstance Reset()
        {
            return new AlchemicalSubstance(AlchemicalState.None);
        }
    }

    public struct HeatState
    {
        public HeatValue Value;

        public HeatState(HeatValue _value)
        {
            Value = _value;
        }

        public static HeatState operator + (HeatState h1, HeatState h2)
        {
            HeatValue value = (HeatValue)Mathf.Clamp((int)h1.Value + (int)h2.Value, -1, 1);
            return new HeatState(value);
        }
    }

    public class AlchemyEngine : MonoBehaviour
    {
        public static AlchemyEngine instance = null;
        public ActivateVFX activateVFX;
        public List<int> transferableEffects = new List<int>() { (int)StatusEffect.Shocked};

        public Dictionary<AlchemicalState, int> substanceTurnsDict = new Dictionary<AlchemicalState, int>()
        {
            {AlchemicalState.gas,4 },
            {AlchemicalState.liquid,4 },
            {AlchemicalState.solid,4 },
            {AlchemicalState.None,-1 },
        };
        public Dictionary<StatusEffect, int> statusTurnsDict = new Dictionary<StatusEffect, int>()
        {
            {StatusEffect.Poisoned,6 },
            {StatusEffect.Burning,4 },
            {StatusEffect.Inferno,4 },
            {StatusEffect.Shocked,2 },
            {StatusEffect.Oiled,6 },
        };

        public void Awake()
        {
            if (instance == null)
                instance = this;
            activateVFX = GetComponent<ActivateVFX>();
        }

        #region Apply Alchemical Transformations
        public (Dictionary<AlchemicalState, AlchemicalSubstance>, HeatValue) ApplyAlchemicalTransfomation(GridCell cell)
        {
            HeatValue heatValue = cell.heatState.Value;
            Dictionary<AlchemicalState, AlchemicalSubstance> substances = new Dictionary<AlchemicalState, AlchemicalSubstance>(cell.substances);

            if (heatValue == 0)
                return (substances, heatValue);

            List<AlchemicalSubstance> alchemicalSubstances = substances.Values.ToList();
            bool modifiedState = false;

            foreach (AlchemicalSubstance substance in alchemicalSubstances)
            {
                AlchemicalState state = substance.alchemicalState;
                if (state != AlchemicalState.None)
                {
                    AlchemicalState newState = (AlchemicalState)Mathf.Clamp((int)state + (int)heatValue,0,2);
                    if (newState != state)
                    {
                        substances[newState] = substances[state].TransformState((int)newState);
                        substances[state] = new AlchemicalSubstance(AlchemicalState.None);
                        modifiedState = true;
                    }
                }
            }

            heatValue = modifiedState ? 0 : heatValue;
            return (substances, heatValue);
        }

        public (Dictionary<AlchemicalState, AlchemicalSubstance>, Dictionary<AlchemicalState, AlchemicalSubstance> ,List<int>, HeatValue, HeatValue) 
            ApplyAlchemicalTransfomation(GridCell cell, CharacterStateManager character, SkillAbstract skill = null)
        {
            HeatState combinedHeatState = skill==null? cell.heatState+ character.heatState: cell.heatState + character.heatState + skill.heatState;
            Dictionary<AlchemicalState, AlchemicalSubstance> substances = new Dictionary<AlchemicalState, AlchemicalSubstance>(cell.substances);
            Dictionary<AlchemicalState, AlchemicalSubstance> characterSubstances = new Dictionary<AlchemicalState, AlchemicalSubstance>(character.characterSubstances);
            List<int> statusEffects = character.statusEffects.Union(cell.cellStatusEffects.Union(transferableEffects)).ToList();            

            if (combinedHeatState.Value == 0)
                return (substances,characterSubstances, statusEffects, HeatValue.neutral, HeatValue.neutral);

            bool modifiedState = false;
            characterSubstances = MergeSubstances(cell, character);

            foreach (AlchemicalState key in substances.Keys.ToList())
            {
                AlchemicalState cellState = substances[key].alchemicalState;
                AlchemicalState characterState = characterSubstances[key].alchemicalState;
                if (cellState != AlchemicalState.None)
                {
                    AlchemicalState newState = (AlchemicalState)Mathf.Clamp((int)cellState + (int)combinedHeatState.Value,0,2);
                    if (newState != cellState)
                    {
                        substances[newState] = substances[cellState].TransformState((int)newState);
                        substances[cellState] = new AlchemicalSubstance(AlchemicalState.None);
                        modifiedState = true;
                    }
                }
                if (characterState != AlchemicalState.None)
                {
                    AlchemicalState newState = (AlchemicalState)Mathf.Clamp((int)characterState + (int)combinedHeatState.Value, 0, 2);
                    if (newState != characterState)
                    {
                        characterSubstances[newState] = characterSubstances[characterState].TransformState((int)newState);
                        characterSubstances[characterState] = new AlchemicalSubstance(AlchemicalState.None);
                        modifiedState = true;

                        if (newState == AlchemicalState.solid)
                            substances[newState] = characterSubstances[newState];
                    }
                }
            }

            combinedHeatState.Value = modifiedState ? 0 : combinedHeatState.Value;
            return (substances, characterSubstances, statusEffects,combinedHeatState.Value, combinedHeatState.Value);
        }
        #endregion

        public Dictionary<AlchemicalState, AlchemicalSubstance> MergeSubstances(GridCell cell, CharacterStateManager character)
        {
            Dictionary<AlchemicalState, AlchemicalSubstance> characterSubstances = new Dictionary<AlchemicalState, AlchemicalSubstance>(character.characterSubstances);
            foreach (AlchemicalState key in cell.substances.Keys)
            {
                if (cell.substances[key].alchemicalState != AlchemicalState.None)
                {
                    AlchemicalSubstance characterSubstance = new AlchemicalSubstance(characterSubstances[key]);
                    characterSubstance.alchemicalState = key;
                    characterSubstances[key] = characterSubstance;
                }
            }
            return characterSubstances;
        }

        public (Dictionary<AlchemicalState, AlchemicalSubstance>, Dictionary<AlchemicalState, AlchemicalSubstance>, List<int>, HeatValue, HeatValue) 
            CharacterCellInteractions(GridCell targetCell, CharacterStateManager targetCharacter)
        {
            HeatState combinedHeat = targetCell.heatState + targetCharacter.heatState;
            if(combinedHeat.Value!=HeatValue.neutral)
                targetCharacter.characterSubstances = MergeSubstances(targetCell,targetCharacter);
            return ApplyAlchemicalTransfomation(targetCell, targetCharacter);
        }

        #region Apply Direct Substance
        public Dictionary<AlchemicalState, AlchemicalSubstance> ApplyDirectSubstance(AlchemicalSubstance newSubstance, 
            Dictionary<AlchemicalState,AlchemicalSubstance> _substances)
        {
            Dictionary<AlchemicalState, AlchemicalSubstance> substances = new Dictionary<AlchemicalState, AlchemicalSubstance>(_substances);
            
            if (substances[newSubstance.alchemicalState].auxiliaryStates.Contains((int)StatusEffect.Shocked))
                newSubstance.AddAuxState(StatusEffect.Shocked);

            substances[newSubstance.alchemicalState] = newSubstance;
            return substances;
        }

        public (Dictionary<AlchemicalState, AlchemicalSubstance>,List<int>, Dictionary<StatusEffect, int>) ApplyDirectSubstance(AlchemicalSubstance newSubstance,
            Dictionary<AlchemicalState, AlchemicalSubstance> _substances, List<int> _statusEffects, Dictionary<StatusEffect, int> _statusTurns)
        {
            Dictionary<AlchemicalState, AlchemicalSubstance> substances = new Dictionary<AlchemicalState, AlchemicalSubstance>(_substances);
            Dictionary<StatusEffect, int> statusTurns = new Dictionary<StatusEffect, int>(_statusTurns);
            substances[newSubstance.alchemicalState] = newSubstance;
            List<int> statusEffects = _statusEffects.Union(newSubstance.auxiliaryStates).ToList();
            foreach(StatusEffect effect in statusEffects.Except(_statusEffects))
            {
                statusTurns.Add(effect, statusTurnsDict[effect]);
            }
            return (substances,statusEffects,statusTurns);
        }
        #endregion

        #region Remove All VFX
        public void RemoveAllVFXCell(GridCell cell)
        {            
            foreach (GameObject vfx in cell.VFXDict.Values)
            {
                if (vfx !=null)
                    Destroy(vfx.gameObject);
            }
            cell.VFXDict=new Dictionary<AlchemicalState, GameObject>()
            {
                { AlchemicalState.solid, null },
                { AlchemicalState.liquid, null },
                { AlchemicalState.gas,null }
            };
        }

        public void RemoveAllVFXCharacter(CharacterStateManager character)
        {
            foreach (GameObject vfx in character.stateVFXDict.Values)
            {
                if (vfx!=null)
                    Destroy(vfx.gameObject);
            }
            character.stateVFXDict = new Dictionary<AlchemicalState, GameObject>()
            {
                { AlchemicalState.solid, null },
                { AlchemicalState.liquid, null },
            };
        }
        #endregion

        #region Apply State VFX
        public void ApplyStateVFX(GridCell cell)
        {
            switch (cell.heatState.Value)
            {
                case HeatValue.hot:
                    if (cell.isFlammable)
                        activateVFX.ActivateElementalEffect(StatusEffect.Burning, cell);
                    else
                        cell.heatState.Value = HeatValue.neutral;
                    break;

                case HeatValue.cold:
                    activateVFX.ActivateElementalEffect(StatusEffect.Chilled, cell);
                    break;

                default:
                    if (cell.substances[AlchemicalState.gas].auxiliaryStates.Contains((int)StatusEffect.Oiled))
                    {
                        RemoveAllVFXCell(cell);
                        activateVFX.ActivateElementalEffect(StatusEffect.Inferno, cell);
                        cell.heatState.Value = HeatValue.hot;
                        cell.substances[AlchemicalState.gas].auxiliaryStates.Add((int)StatusEffect.Inferno);
                        PropagateInferno(cell);                        
                    }
                    else
                        activateVFX.ActivateElementalEffect(cell);
                    break;
            }

            if (ActivateVFX.Instance.PoisonCheckCell != null)
            {
                PropagateEffect(ActivateVFX.Instance.PoisonCheckCell, StatusEffect.Poisoned);
                ActivateVFX.Instance.PoisonCheckCell = null;
            }
            if (ActivateVFX.Instance.ShockCheckCell != null)
            {
                PropagateEffect(ActivateVFX.Instance.ShockCheckCell, StatusEffect.Shocked);
                ActivateVFX.Instance.ShockCheckCell = null;
            }
        }

        public void ApplyStateVFX(CharacterStateManager character)
        {
            switch (character.heatState.Value)
            {
                case HeatValue.hot:
                    activateVFX.ActivateElementalEffect(StatusEffect.Burning, character);
                    break;

                case HeatValue.cold:
                    activateVFX.ActivateElementalEffect(StatusEffect.Chilled, character);
                    break;

                default:
                    if (character.statusVFXDict.ContainsKey(StatusEffect.Burning))
                    {
                        Destroy(character.statusVFXDict[StatusEffect.Burning]);
                        character.statusVFXDict.Remove(StatusEffect.Burning);
                    }
                    if (character.statusVFXDict.ContainsKey(StatusEffect.Inferno))
                    {
                        character.heatState.Value = HeatValue.hot;
                        return;
                    }
                    else if (character.characterSubstances[AlchemicalState.gas].auxiliaryStates.Contains((int)StatusEffect.Oiled))
                    {
                        RemoveAllVFXCharacter(character);
                        activateVFX.ActivateElementalEffect(StatusEffect.Inferno, character);
                        character.AddStatus(StatusEffect.Inferno);
                        character.heatState.Value = HeatValue.hot;
                    }
                    else
                        activateVFX.ActivateElementalEffect(character);
                    break;
            }
        }
        #endregion

        #region Effect Propagation
        public void PropagateEffect(GridCell sourceCell, StatusEffect effect)
        {
            IntVector2 currentCellIndex;
            List<IntVector2> que = new List<IntVector2>();
            List<IntVector2> visited = new List<IntVector2>();
            que.Add(sourceCell.index);

            while (que.Count != 0)
            {
                currentCellIndex = que[0];
                que.RemoveAt(0);

                visited.Add(currentCellIndex);               

                List<IntVector2> validMoves = new List<IntVector2>()
                {
                    new IntVector2(currentCellIndex.x+1,currentCellIndex.y),
                    new IntVector2(currentCellIndex.x-1,currentCellIndex.y),
                    new IntVector2(currentCellIndex.x,currentCellIndex.y+1),
                    new IntVector2(currentCellIndex.x,currentCellIndex.y-1)
                };
            
                foreach (IntVector2 move in validMoves)
                {
                    GridCell cell = GridManager.Instance.GetCellByIndex(move);

                    if (cell != null && !visited.Contains(move) && cell.substances[AlchemicalState.liquid].alchemicalState == AlchemicalState.liquid)
                    {
                        cell.substances[AlchemicalState.liquid].AddAuxState(effect);
                        switch (effect)
                        {                            
                            case StatusEffect.Shocked:
                                if (cell.VFXDict[AlchemicalState.liquid]!=null)
                                    cell.VFXDict[AlchemicalState.liquid].transform.Find("ShockEffect").gameObject.SetActive(true);
                                break;
                            case StatusEffect.Poisoned:
                                if (cell.VFXDict[AlchemicalState.liquid] != null)
                                {
                                    cell.VFXDict[AlchemicalState.liquid].transform.Find("PoisonEffect").gameObject.SetActive(true);
                                    cell.VFXDict[AlchemicalState.liquid].GetComponentInChildren<MeshRenderer>().material.SetFloat("PoisonedCheck", 1);
                                }
                                break;
                        }                            

                        if (!que.Contains(cell.index))
                        {
                            que.Add(cell.index);
                        }
                    }
                }
            }
        }

        public void PropagateInferno(GridCell sourceCell)
        {
            IntVector2 currentCellIndex;
            List<IntVector2> que = new List<IntVector2>();
            List<IntVector2> visited = new List<IntVector2>();
            que.Add(sourceCell.index);

            while (que.Count != 0)
            {
                currentCellIndex = que[0];
                que.RemoveAt(0);

                visited.Add(currentCellIndex);

                List<IntVector2> validMoves = new List<IntVector2>()
                {
                    new IntVector2(currentCellIndex.x+1,currentCellIndex.y),
                    new IntVector2(currentCellIndex.x-1,currentCellIndex.y),
                    new IntVector2(currentCellIndex.x,currentCellIndex.y+1),
                    new IntVector2(currentCellIndex.x,currentCellIndex.y-1)
                };

                foreach (IntVector2 move in validMoves)
                {
                    GridCell cell = GridManager.Instance.GetCellByIndex(move);

                    if (cell != null && !visited.Contains(move) && cell.substances[AlchemicalState.liquid].alchemicalState == AlchemicalState.liquid
                        && cell.substances[AlchemicalState.liquid].auxiliaryStates.Contains((int)StatusEffect.Oiled))
                    {
                        RemoveAllVFXCell(cell);
                        activateVFX.ActivateElementalEffect(StatusEffect.Inferno, cell);
                        cell.substances[AlchemicalState.gas].auxiliaryStates.Add((int)StatusEffect.Inferno);
                        cell.heatState.Value = HeatValue.hot;

                        if (!que.Contains(cell.index))
                            que.Add(cell.index);

                        if (cell.occupyingObject!=null)
                        {
                            CharacterStateManager character = cell.occupyingObject.GetComponent<CharacterStateManager>();
                            RemoveAllVFXCharacter(character);
                            activateVFX.ActivateElementalEffect(StatusEffect.Inferno, character);
                        }
                    }
                }
            }
        }
        #endregion
    }
}

