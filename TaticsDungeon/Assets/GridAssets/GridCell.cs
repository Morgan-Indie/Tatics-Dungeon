using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public enum CellState { open, obstacle, occupiedParty, occupiedEnemy , interactable};

    public class GridCell : MonoBehaviour
    {
        public Color color;
        public int height;
        public CellTemplate cellTemplate;
        bool highlighted = false;
        Color highlightColor = Color.magenta;
        public GameObject occupyingObject=null;
        public CellState state;
        GameObject highlightEffect;
        public CellHighlightType highlightType = CellHighlightType.None;
        public CellAlchemyState alchemyState;
        public bool isStairs=false;
        public int BurnDamage;
        public int PoisonDamage;

        public bool HasAdjacentStair = false;
        //If cell is a stair cell, sets this tuple to its Entrance and Exit respectively 
        public (IntVector2, IntVector2) stairExits;

        //index within its parent mesh
        public IntVector2 index;
        //index inside of the entire grid
        public IntVector2 gridIndex;

        public void Start()
        {
            alchemyState = GetComponent<CellAlchemyState>();
            transform.position = new Vector3(transform.position.x, transform.position.y + height * GridMetrics.heightIncrement, transform.position.z);
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

        public void SetOccupyingObject(GameObject o) { occupyingObject = o;}
        public GameObject GetOccupyingObject() { return occupyingObject; }

        public void SetCellState()
        {
            if (cellTemplate.isARiver || cellTemplate.obstacleMode != 0)
                state = CellState.obstacle;

            else if (cellTemplate.stairMode != CellTemplate.StairMode.None)
                isStairs = true;

            else if (occupyingObject != null)
            {
                switch(occupyingObject.tag)
                {
                    case "Player":
                        state = CellState.occupiedParty;
                        break;
                    case "Enemy":
                        state = CellState.occupiedEnemy;
                        break;
                    case "Interactable":
                        state = CellState.interactable;
                        break;
                }
            }

            else
                state = CellState.open;
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
            return Mathf.Abs(this.x - index.x) + Mathf.Abs(this.y - index.y);
        }

        public bool IsOrtho(IntVector2 index)
        {
            return this.x==index.x || this.y == index.y;
        }

        public bool IsValid(GridMapAdapter gridMapAdapter)
        {
            return !(this.x < 0 || this.y < 0 || 
                this.x >= gridMapAdapter.gridMap.width || this.y >= gridMapAdapter.gridMap.height);
        }

        public IntVector2 Add(IntVector2 index)
        {
            return new IntVector2(this.x + index.x, this.y + index.y);
        }

        public IntVector2 Subtract(IntVector2 index)
        {
            return new IntVector2(this.x - index.x, this.y - index.y);
        }
    }
}