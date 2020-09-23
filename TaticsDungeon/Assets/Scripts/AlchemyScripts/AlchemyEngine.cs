using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PrototypeGame
{
    public enum AlchemicalState { solid, liquid, gas, None}
    public enum HeatValue { hot=1,cold=-1,neutral = 0};
    public enum AlchemicalEffect {solid, liquid, gas, fire, inferno, chill,shock, oil, poisoned};
    public enum StatusEffect
    {
        Burning, Inferno, Frozen, Shocked, Poisoned, Cursed, Blessed, Wet, Oiled, Electricuted,None
    }

    public struct AlchemicalSubstance
    {
        public AlchemicalState alchemicalState;
        public List<int> auxiliaryStates;

        public AlchemicalSubstance(AlchemicalState _alchemicalState)
        {
            alchemicalState = _alchemicalState;
            auxiliaryStates = new List<int>();
        }

        public void AddAuxState(StatusEffect _auxiliaryState)
        {            
            auxiliaryStates.Add((int)_auxiliaryState);
        }

        public AlchemicalSubstance TransformState(int _alchemicalState)
        {
            if (_alchemicalState > 2)
                _alchemicalState = 2;
            else if (_alchemicalState < 0)
                _alchemicalState = 0;
            alchemicalState = (AlchemicalState)_alchemicalState;
            return this;
        }
    }

    public class HeatState
    {
        public List<int> heatValues;
        public HeatValue Value = 0;

        public void AddHeat(HeatValue heatValue)
        {
            Value += (int)heatValue;
            if (Value < 0)
                Value= HeatValue.cold;
            else if (Value > 0)
                Value = HeatValue.hot;            
        }
    }

    public class AlchemyEngine : MonoBehaviour
    {
        public static AlchemyEngine instance = null;
        public ActivateVFX activateVFX;
        public List<int> transferableEffects = new List<int>();

        public void Awake()
        {
            if (instance == null)
                instance = this;
            activateVFX = GetComponent<ActivateVFX>();
            transferableEffects.Add((int)StatusEffect.Shocked);
        }

    public (Dictionary<AlchemicalState, AlchemicalSubstance>,HeatValue) ApplyAlchemicalTransfomation(Dictionary<AlchemicalState, 
            AlchemicalSubstance> _substances,GridCell cell)
        {
            HeatValue heatValue = cell.heatState.Value;
            Dictionary<AlchemicalState, AlchemicalSubstance> substances = new Dictionary<AlchemicalState, AlchemicalSubstance>(_substances); 

            if (heatValue == 0)
                return (substances, heatValue);

            List<AlchemicalSubstance> alchemicalSubstances = substances.Values.ToList();
            bool modifiedState = false;

            foreach (AlchemicalSubstance substance in alchemicalSubstances)
            {
                AlchemicalState state = substance.alchemicalState;
                if (state != AlchemicalState.None)
                {
                    AlchemicalState newState = state + (int)heatValue;
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

        public (Dictionary<AlchemicalState, AlchemicalSubstance>, HeatValue) ApplyDirectSubstance(AlchemicalSubstance newSubstance, 
            Dictionary<AlchemicalState,AlchemicalSubstance> _substances, GridCell cell)
        {
            Dictionary<AlchemicalState, AlchemicalSubstance> substances = new Dictionary<AlchemicalState, AlchemicalSubstance>(_substances);

            List<int> transferredEffects = substances[newSubstance.alchemicalState].auxiliaryStates.Intersect(transferableEffects).ToList();
            newSubstance.auxiliaryStates = transferredEffects.Union(newSubstance.auxiliaryStates).ToList();
            substances[newSubstance.alchemicalState] = newSubstance;
            return ApplyAlchemicalTransfomation(substances, cell);
        }

        public void RemoveAllVFXCell(GridCell cell)
        {            
            foreach (GameObject vfx in cell.cellVFXList)
            {
                Destroy(vfx.gameObject);
            }
            cell.VFXDict=new Dictionary<AlchemicalState, GameObject>()
            {
                { AlchemicalState.solid, null },
                { AlchemicalState.liquid, null },
                { AlchemicalState.gas,null }
            };
        }

        public void ApplyStateVFX(GridCell cell)
        {
            switch (cell.heatState.Value)
            {
                case HeatValue.hot: 
                    activateVFX.ActivateElementalEffect(AlchemicalEffect.fire, cell);                    
                    break;

                case HeatValue.cold:
                    activateVFX.ActivateElementalEffect(AlchemicalEffect.chill, cell);
                    break;

                default:
                    RemoveAllVFXCell(cell);

                    if (cell.substances[AlchemicalState.gas].auxiliaryStates.Contains((int)StatusEffect.Oiled))
                    {
                        activateVFX.ActivateElementalEffect(AlchemicalEffect.inferno, cell);
                        cell.substances[AlchemicalState.gas].auxiliaryStates.Add((int)StatusEffect.Inferno);
                        PropagateInferno(cell);                        
                    }
                    else
                        activateVFX.ActivateElementalEffect(cell);
                    break;
            }
        }

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
                        activateVFX.ActivateElementalEffect(AlchemicalEffect.inferno, cell);
                        cell.substances[AlchemicalState.gas].auxiliaryStates.Add((int)StatusEffect.Inferno);
                        cell.substances[AlchemicalState.gas].auxiliaryStates.Add((int)StatusEffect.Oiled);
                        if (!que.Contains(cell.index))
                            que.Add(cell.index);
                    }
                }
            }
        }
    }
}

