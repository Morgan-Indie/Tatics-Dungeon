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
        public GameObject OilVFXCharacter;
        public GameObject InfernoVFXCharacter;
        public GameObject ShockVFXCharacter;
        public GameObject PoisonVFXCharacter;
        public GameObject FrozenVFXCharacter;

        public GameObject FireVFXCell;
        public GameObject WetVFXCell;
        public GameObject OilVFXCell;
        public GameObject InfernoVFXCell;
        public GameObject ShockVFXCell;
        public GameObject FrozenVFXCell;
        public GameObject ChilledVFXCell;
        public GameObject GasVFXCell;

        public List<GameObject> BloodVFXs;
        public GameObject HealingVFX;
        public Dictionary<AlchemicalEffect, GameObject> characterVFX_dict = new Dictionary<AlchemicalEffect, GameObject>();
        public Dictionary<AlchemicalEffect, GameObject> cellVFX_dict = new Dictionary<AlchemicalEffect, GameObject>();

        public GridCell ShockCheckCell = null;
        public GridCell PoisonCheckCell = null;

        public void Awake()
        {
            if (Instance == null)
                Instance = this;

            characterVFX_dict.Add(AlchemicalEffect.fire, FireVFXCharacter);
            characterVFX_dict.Add(AlchemicalEffect.inferno, InfernoVFXCharacter);
            characterVFX_dict.Add(AlchemicalEffect.liquid, WetVFXCharacter);
            characterVFX_dict.Add(AlchemicalEffect.poisoned, PoisonVFXCharacter);
            characterVFX_dict.Add(AlchemicalEffect.oil, PoisonVFXCharacter);
            characterVFX_dict.Add(AlchemicalEffect.solid, FrozenVFXCharacter);

            cellVFX_dict.Add(AlchemicalEffect.fire, FireVFXCell);
            cellVFX_dict.Add(AlchemicalEffect.inferno, InfernoVFXCell);
            cellVFX_dict.Add(AlchemicalEffect.liquid, WetVFXCell);
            cellVFX_dict.Add(AlchemicalEffect.oil, OilVFXCell);
            cellVFX_dict.Add(AlchemicalEffect.solid, FrozenVFXCell);
            cellVFX_dict.Add(AlchemicalEffect.chill, ChilledVFXCell);
            cellVFX_dict.Add(AlchemicalEffect.gas, GasVFXCell);
        }

        public void ActivateElementalEffect(GridCell target)
        {            
            foreach (AlchemicalSubstance substance in target.substances.Values.ToList())
            {
                if (substance.alchemicalState != AlchemicalState.None)
                {
                    GameObject vfx = Instantiate(cellVFX_dict[(AlchemicalEffect)substance.alchemicalState]);     

                    if (substance.alchemicalState==AlchemicalState.liquid)
                    {
                        IntVector2 currentCellIndex = target.index;
                        List<IntVector2> validMoves = new List<IntVector2>()
                        {
                            new IntVector2(currentCellIndex.x+1,currentCellIndex.y),
                            new IntVector2(currentCellIndex.x-1,currentCellIndex.y),
                            new IntVector2(currentCellIndex.x,currentCellIndex.y+1),
                            new IntVector2(currentCellIndex.x,currentCellIndex.y-1)
                        };

                        foreach(IntVector2 move in validMoves)
                        {
                            GridCell cell = GridManager.Instance.GetCellByIndex(move);

                            if (cell!=null)
                            {
                                if (cell.substances[AlchemicalState.gas].auxiliaryStates.Contains((int)StatusEffect.Inferno) &&
                                        substance.auxiliaryStates.Contains((int)StatusEffect.Oiled))
                                {
                                    AlchemyEngine.instance.PropagateInferno(cell);
                                    break;
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

                    foreach (StatusEffect statusEffect in substance.auxiliaryStates)
                    {
                        switch (statusEffect)
                        {
                            case StatusEffect.Poisoned:
                                if (!vfx.transform.Find("PoisonEffect").gameObject.activeSelf)
                                {
                                    vfx.transform.Find("PoisonEffect").gameObject.SetActive(true);
                                    if (substance.alchemicalState == AlchemicalState.liquid)
                                    {
                                        vfx.GetComponentInChildren<MeshRenderer>().material.SetFloat("PoisonedCheck", 1);
                                        PoisonCheckCell = target;
                                    }
                                    else if (substance.alchemicalState == AlchemicalState.gas)
                                    {
                                        vfx.GetComponentInChildren<VisualEffect>().SetBool("Poisoned",true);
                                    }
                                    else
                                        vfx.GetComponentInChildren<MeshRenderer>().material.SetFloat("PoisonedCheck", 1);
                                }
                                break;

                            case StatusEffect.Shocked:
                                if (!vfx.transform.Find("ShockEffect").gameObject.activeSelf)
                                {
                                    vfx.transform.Find("ShockEffect").gameObject.SetActive(true);
                                    if (substance.alchemicalState==AlchemicalState.liquid)
                                        ShockCheckCell = target;
                                }
                                break;

                            case StatusEffect.Oiled: vfx.GetComponentInChildren<MeshRenderer>().material.SetFloat("OilCheck", 1); break;
                        }
                    }
                    vfx.transform.position = target.transform.position;
                    vfx.transform.SetParent(target.transform);
                    target.cellVFXList.Add(vfx);
                    target.VFXDict[substance.alchemicalState]=vfx;
                }
            }            
        }

        public void ActivateElementalEffect(AlchemicalEffect state, GridCell target)
        {
            cellVFX_dict[state].transform.position = target.transform.position;
            GameObject vfx = Instantiate(cellVFX_dict[state]);
            vfx.transform.position = target.transform.position;
            vfx.transform.SetParent(target.transform);
            target.cellVFXList.Add(vfx);
        }

        public void ActivateHealingEffect(GameObject target)
        {
            GameObject healEffect = Instantiate(HealingVFX) as GameObject;
            healEffect.transform.position = target.transform.position + Vector3.up * .5f;
            healEffect.transform.SetParent(target.transform);
            Destroy(healEffect, 2f);
        }
    }
}

