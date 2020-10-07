using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.VFX;
using UnityEditor.VFX;

namespace PrototypeGame
{
    public class ActivateVFX : MonoBehaviour
    {
        public static ActivateVFX Instance = null;
        public GameObject FireVFXCharacter;
        public GameObject WetVFXCharacter;
        public GameObject InfernoVFXCharacter;
        public GameObject ShockVFXCharacter;
        public GameObject PoisonVFXCharacter;
        public GameObject FrozenVFXCharacter;

        public GameObject FireVFXCell;
        public GameObject WetVFXCell;
        public GameObject InfernoVFXCell;
        public GameObject FrozenVFXCell;
        public GameObject ChilledVFXCell;
        public GameObject GasVFXCell;

        public List<GameObject> BloodVFXs;
        public GameObject HealingVFX;
        public Dictionary<StatusEffect, GameObject> characterStatusVFX_dict = new Dictionary<StatusEffect, GameObject>();
        public Dictionary<StatusEffect, GameObject> cellStatusVFX_dict = new Dictionary<StatusEffect, GameObject>();
        public Dictionary<AlchemicalState, GameObject> characterStateVFX_dict = new Dictionary<AlchemicalState, GameObject>();
        public Dictionary<AlchemicalState, GameObject> cellStateVFX_dict = new Dictionary<AlchemicalState, GameObject>();

        public GridCell ShockCheckCell = null;
        public GridCell PoisonCheckCell = null;

        public void Awake()
        {
            if (Instance == null)
                Instance = this;

            characterStatusVFX_dict.Add(StatusEffect.Burning, FireVFXCharacter);
            characterStatusVFX_dict.Add(StatusEffect.Inferno, InfernoVFXCharacter);
            characterStatusVFX_dict.Add(StatusEffect.Poisoned, PoisonVFXCharacter);
            characterStatusVFX_dict.Add(StatusEffect.Shocked, ShockVFXCharacter);

            characterStateVFX_dict.Add(AlchemicalState.liquid, WetVFXCharacter);
            characterStateVFX_dict.Add(AlchemicalState.solid, FrozenVFXCharacter);

            cellStatusVFX_dict.Add(StatusEffect.Burning, FireVFXCell);
            cellStatusVFX_dict.Add(StatusEffect.Inferno, InfernoVFXCell);
            cellStatusVFX_dict.Add(StatusEffect.Chilled, ChilledVFXCell);

            cellStateVFX_dict.Add(AlchemicalState.liquid, WetVFXCell);
            cellStateVFX_dict.Add(AlchemicalState.gas, GasVFXCell);
            cellStateVFX_dict.Add(AlchemicalState.solid, FrozenVFXCell);

        }

        public void DestroyStateEffect(GridCell target, AlchemicalState key)
        {
            Destroy(target.VFXDict[key]);
            target.VFXDict[key] = null;
        }

        public void DestroyStateEffect(CharacterStateManager target, AlchemicalState key)
        {
            Destroy(target.stateVFXDict[key]);
            target.stateVFXDict[key] = null;
        }

        public void DestroyStatusEffect(CharacterStateManager target, StatusEffect key)
        {
            Destroy(target.statusVFXDict[key]);
            target.statusVFXDict[key] = null;
        }

        #region activate elemental effect
        public void ActivateElementalEffect(GridCell target)
        {            
            foreach (AlchemicalState key in target.substances.Keys.ToList())
            {
                if (target.substances[key].alchemicalState != AlchemicalState.None)
                {
                    if (target.VFXDict[key] == null)
                    {
                        InstantiateSubstanceVFX(target, key);

                        if (key == AlchemicalState.liquid)
                            StatusRepropagation(target);
                    }
                    else
                        ActivateStateStatusEffects(target.VFXDict[key], target, key);
                }

                else if (target.VFXDict[key] != null)
                    DestroyStateEffect(target, key);
            }            
        }

        public void ActivateElementalEffect(CharacterStateManager target)
        {
            foreach (AlchemicalState key in target.stateVFXDict.Keys.ToList())
            {
                if (target.characterSubstances[key].alchemicalState != AlchemicalState.None)
                {
                    if (target.stateVFXDict[key] == null)
                        InstantiateSubstanceVFX(target, key);
                    else
                        ActivateStateStatusEffects(target.stateVFXDict[key], target);
                }

                else if (target.stateVFXDict.ContainsKey(key) && target.stateVFXDict[key] != null)
                    DestroyStateEffect(target, key);
            }

            foreach (StatusEffect status in target.statusEffects)
            {
                if (!target.statusVFXDict.ContainsKey(status) && status!=StatusEffect.Oiled)
                {
                    ActivateElementalEffect(status, target);                    
                }
            }
        }
        #endregion

        public void StatusRepropagation(GridCell target)
        {
            IntVector2 currentCellIndex = target.index;
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

                if (cell != null)
                {
                    if (cell.substances[AlchemicalState.gas].auxiliaryStates.Contains((int)StatusEffect.Inferno) &&
                            target.substances[AlchemicalState.liquid].auxiliaryStates.Contains((int)StatusEffect.Oiled))
                    {                        
                        AlchemyEngine.instance.PropagateInferno(cell);
                        return;
                    }

                    if (cell.substances[AlchemicalState.liquid].alchemicalState == AlchemicalState.liquid &&
                        cell.substances[AlchemicalState.liquid].auxiliaryStates.Contains((int)StatusEffect.Shocked))
                    {
                        ShockCheckCell = cell;
                        break;
                    }
                }
            }
        }

        public void InstantiateSubstanceVFX(GridCell target,AlchemicalState key)
        {
            if (target.VFXDict[key] != null)
                Destroy(target.VFXDict[key]);

            GameObject vfx = Instantiate(cellStateVFX_dict[key]);

            ActivateStateStatusEffects(vfx, target, key);
            vfx.transform.position = target.transform.position;
            vfx.transform.SetParent(target.transform);            
            target.VFXDict[key] = vfx;
        }

        public void InstantiateSubstanceVFX(CharacterStateManager target, AlchemicalState key)
        {
            GameObject vfx = Instantiate(characterStateVFX_dict[key]);
            ActivateStateStatusEffects(vfx, target);
            vfx.transform.position = target.transform.position;
            vfx.transform.SetParent(target.transform);
            target.stateVFXDict[key] = vfx;
        }

        #region Activate Status Effects
        public void ActivateStateStatusEffects(GameObject vfx, GridCell target, AlchemicalState key)
        {
            foreach (StatusEffect statusEffect in target.substances[key].auxiliaryStates)
            {
                switch (statusEffect)
                {
                    case StatusEffect.Poisoned:
                        if (!vfx.transform.Find("PoisonEffect").gameObject.activeSelf)
                        {
                            vfx.transform.Find("PoisonEffect").gameObject.SetActive(true);
                            if (key == AlchemicalState.liquid)
                            {
                                vfx.GetComponentInChildren<MeshRenderer>().material.SetFloat("PoisonedCheck", 1);
                                PoisonCheckCell = target;
                            }
                            else if (key == AlchemicalState.gas)
                            {
                                vfx.GetComponentInChildren<VisualEffect>().SetBool("Poisoned", true);
                            }
                            else
                                vfx.GetComponentInChildren<MeshRenderer>().material.SetFloat("PoisonedCheck", 1);
                        }
                        break;

                    case StatusEffect.Shocked:
                        if (!vfx.transform.Find("ShockEffect").gameObject.activeSelf)
                        {
                            vfx.transform.Find("ShockEffect").gameObject.SetActive(true);
                            if (key == AlchemicalState.liquid)
                                ShockCheckCell = target;
                        }
                        break;

                    case StatusEffect.Oiled: vfx.GetComponentInChildren<MeshRenderer>().material.SetFloat("OilCheck", 1); break;
                }
            }
        }

        public void ActivateStateStatusEffects(GameObject vfx, CharacterStateManager target)
        {
            if (target.characterSubstances[AlchemicalState.liquid].auxiliaryStates.Contains((int)StatusEffect.Poisoned))
                vfx.GetComponentInChildren<VisualEffect>().SetBool("Poisoned", true);
            if (target.characterSubstances[AlchemicalState.liquid].auxiliaryStates.Contains((int)StatusEffect.Oiled))
                vfx.GetComponentInChildren<VisualEffect>().SetBool("Oiled", true);
        }
        #endregion

        #region direct effect activations
        public void ActivateElementalEffect(StatusEffect status, GridCell target)
        {
            cellStatusVFX_dict[status].transform.position = target.transform.position;
            GameObject vfx = Instantiate(cellStatusVFX_dict[status]);
            vfx.transform.position = target.transform.position;
            vfx.transform.SetParent(target.transform);
            target.statusVFXDict[status]=vfx;
        }

        public void ActivateElementalEffect(StatusEffect status, CharacterStateManager target)
        {
            characterStatusVFX_dict[status].transform.position = target.transform.position;
            GameObject vfx = Instantiate(characterStatusVFX_dict[status]);
            vfx.transform.position = target.transform.position+Vector3.up*.5f;
            vfx.transform.SetParent(target.transform);
            target.statusVFXDict[status]=vfx;
        }

        public void ActivateHealingEffect(GameObject target)
        {
            GameObject healEffect = Instantiate(HealingVFX) as GameObject;
            healEffect.transform.position = target.transform.position + Vector3.up * .5f;
            healEffect.transform.SetParent(target.transform);
            Destroy(healEffect, 2f);
        }
        #endregion
    }
}

