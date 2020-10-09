using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PrototypeGame
{
    public enum CellState { open, obstacle,occupiedParty, occupiedEnemy , interactable};

    public class GridCell : MonoBehaviour
    {
        public Color color;
        public int height;
        public CellTemplate cellTemplate;
        bool highlighted = false;
        Color highlightColor = Color.magenta;        
        public CellState baseState;
        GameObject highlightEffect;
        public CellHighlightType highlightType = CellHighlightType.None;
        public bool isStairs=false;
        public bool isFlammable = false;
        public HeatState heatState = new HeatState();
        public LayerMask occupingObjectMask;
        public int InfernoTurns = 0;
        public int ChilledTurns = 0;

        public Dictionary<AlchemicalState, AlchemicalSubstance> substances;

        public GameObject occupyingObject
        {
            get
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position,Vector3.up,out hit, 1f ,occupingObjectMask))
                {
                    return hit.transform.gameObject;
                }
                return null;
            }
        }

        public CellState state
        {
            get
            {
                if (occupyingObject!=null)
                {
                    switch (occupyingObject.tag)
                    {
                        case "Player": return CellState.occupiedParty;
                        case "Enemy": return CellState.occupiedEnemy;
                        case "Interactable": return CellState.interactable;
                    }
                }
                return baseState;
            }
        }

        public Dictionary<AlchemicalState, GameObject> VFXDict = new Dictionary<AlchemicalState, GameObject>()
        {
            { AlchemicalState.solid, null },
            { AlchemicalState.liquid, null },
            { AlchemicalState.gas,null },
            { AlchemicalState.None,null }
        };

        public Dictionary<StatusEffect, GameObject> statusVFXDict = new Dictionary<StatusEffect, GameObject>()
        {
            {StatusEffect.Inferno,null },
            {StatusEffect.Chilled,null }
        };

        public List<int> cellStatusEffects
        {
            get
            {
                List<int> _cellStatusEffects = new List<int>();
                foreach (AlchemicalSubstance substance in substances.Values)
                {
                    if (substance.alchemicalState!=AlchemicalState.None)
                    {
                        foreach (int status in substance.auxiliaryStates.Distinct())
                            _cellStatusEffects.Add(status);
                    }
                }
                return _cellStatusEffects;
            }
        }

        public bool HasAdjacentStair = false;
        //If cell is a stair cell, sets this tuple to its Entrance and Exit respectively 
        public (IntVector2, IntVector2) stairExits;

        //index within its parent mesh
        public IntVector2 index;
        //index inside of the entire grid
        public IntVector2 gridIndex;

        public void Start()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + height * GridMetrics.heightIncrement, transform.position.z);
            substances = new Dictionary<AlchemicalState, AlchemicalSubstance>()
            {
                { AlchemicalState.solid,new AlchemicalSubstance(AlchemicalState.None) },
                { AlchemicalState.liquid,new AlchemicalSubstance(AlchemicalState.None) },
                { AlchemicalState.gas,new AlchemicalSubstance(AlchemicalState.None) }
            };
        }

        public Color GetColor()
        {
            if (!highlighted)
                return color;
            else
                return highlightColor;
        }

        public void Highlight()
        {
            highlighted = true;
        }

        public void ApplyHighlight(GameObject effect, CellHighlightType type)
        {
            if (type == highlightType)
                return;
            highlightType = type;
            highlighted = true;
            if (highlightEffect != null)
                Destroy(highlightEffect);
            highlightEffect = Instantiate(effect, transform.position, transform.rotation);
            highlightEffect.transform.SetParent(transform);
        }

        public void RemoveHighlight()
        {
            highlighted = false;
            if (highlightEffect != null)
                Destroy(highlightEffect);
            highlightType = CellHighlightType.None;
        }

        public bool IsHighlighted() { return highlighted; }

        public void SetIndices(int x, int y, int mx, int my, int maxMeshSize)
        {
            index = new IntVector2(x, y);
            gridIndex = new IntVector2(x + mx * maxMeshSize, y + my * maxMeshSize);
        }

        public void SetCellState()
        {
            if (cellTemplate.isARiver || cellTemplate.obstacleMode != 0)
                baseState = CellState.obstacle;
            else
                baseState = CellState.open;

            if (cellTemplate.stairMode != CellTemplate.StairMode.None)
                isStairs = true;
        }

        public CellState GetCellState() { return state; }        

        public void ApplyHighlight(GameObject effect)
        {
            highlighted = true;
            if (highlightEffect != null)
                Destroy(highlightEffect);
            highlightEffect = Instantiate(effect, transform.position, transform.rotation);
            highlightEffect.transform.SetParent(transform);
        }

        public void UpdateAlchemicalStates()
        {
            bool toRemove = true;
            foreach(AlchemicalState state in substances.Keys.ToList())
            {
                if (substances[state].alchemicalState!=AlchemicalState.None)
                {
                    toRemove = false;
                    substances[state].turnsLeft -= 1;
                    if (substances[state].turnsLeft <= 0)
                    {
                        substances[state].Reset();
                        if (VFXDict[state] != null)
                        {
                            Destroy(VFXDict[state]);
                            VFXDict[state] = null;
                        }
                    }

                    else
                    {
                        foreach (StatusEffect status in substances[state].auxiliaryStates.ToList())
                        {
                            substances[state].statusTurns[(int)status] -= 1;
                            if (substances[state].statusTurns[(int)status]<=0)
                            {
                                substances[state].auxiliaryStates.Remove((int)status);
                                substances[state].statusTurns.Remove((int)status);
                                if (status == StatusEffect.Shocked)
                                {
                                    foreach (GameObject vfx in VFXDict.Values)
                                    {
                                        if (vfx != null)
                                            vfx.transform.Find("ShockEffect").gameObject.SetActive(false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            if (ChilledTurns>0)
            {
                toRemove = false;
                ChilledTurns -= 1;
                if (ChilledTurns <=0)
                {
                    Destroy(statusVFXDict[StatusEffect.Chilled]);
                    statusVFXDict.Remove(StatusEffect.Chilled);
                    heatState.Value = HeatValue.neutral;
                }
            }

            if (InfernoTurns > 0)
            {
                toRemove = false;
                InfernoTurns -= 1;
                if (InfernoTurns <= 0)
                {
                    Destroy(statusVFXDict[StatusEffect.Inferno]);
                    statusVFXDict[StatusEffect.Inferno] = null;
                    heatState.Value = HeatValue.neutral;
                }
            }

            if (toRemove)
                GameManager.instance.GridCellsToUpdate.Remove(this);
        }
    }

    [System.Serializable]
    public struct IntVector2
    {
        public int x;
        public int y;
        public IntVector2(int xs, int ys)
        {
            x = xs;
            y = ys;
        }
        public void Print() { Debug.Log(x + ":" + y); }
        public string Tostring() { return x + ":" + y; }
        public bool Equals(IntVector2 index)
        {
            return x == index.x && y == index.y;
        }

        public void SetValues(int newX, int newY)
        {
            x = newX;
            y = newY;
        }

        public bool IsIn(List<IntVector2> indices)
        {
            foreach (IntVector2 index in indices)
            {
                if (this.Equals(index)) { return true; }
            }
            return false;
        }
        public int IndexInList(List<IntVector2> indices)
        {
            for (int i = 0; i < indices.Count; i++)
            {
                if (this.Equals(indices[i])) { return i; }
            }
            Debug.Log("IntVector2.IndexInList: values not contained within list");
            return -1;
        }

        public int GetDistance(IntVector2 index)
        {
            return Mathf.Abs(x - index.x) + Mathf.Abs(y - index.y);
        }

        public bool IsOrtho(IntVector2 index)
        {
            return x==index.x || y == index.y;
        }

        public bool IsValid(GridMapAdapter gridMapAdapter)
        {
            return !(x < 0 ||y < 0 || 
                x >= gridMapAdapter.gridMap.width || y >= gridMapAdapter.gridMap.height);
        }

        public IntVector2 Add(IntVector2 index)
        {
            return new IntVector2(this.x + index.x, this.y + index.y);
        }

        public IntVector2 Subtract(IntVector2 index)
        {
            return new IntVector2(this.x - index.x, this.y - index.y);
        }

        public static IntVector2 operator +(IntVector2 a, IntVector2 b)
        {
            return new IntVector2(a.x + b.x, a.y + b.y);
        }

        public static IntVector2 operator -(IntVector2 a, IntVector2 b)
        {
            return new IntVector2(a.x - b.x, a.y - b.y);
        }

        public int GetSection(IntVector2 point,int sections)
        {
            float sectionSize = 360f / (float)sections;
            float Vx = point.x - x;
            float Vy = point.y - y;

            float radians = Mathf.Atan2(Vy, Vx); 
            if (radians < 0) { radians += 2 * Mathf.PI; }

            float degrees = 360f - radians * Mathf.Rad2Deg;
            degrees += sectionSize / 2;
            if (degrees >= 360) { degrees -= 360; }
            int result = Mathf.FloorToInt(degrees / sectionSize);
            return result;
        }
    }
}