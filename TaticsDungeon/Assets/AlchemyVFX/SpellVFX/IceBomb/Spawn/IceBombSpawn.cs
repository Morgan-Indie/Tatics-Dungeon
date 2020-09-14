using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class IceBombSpawn : VFXSpawns
    {
        public List<GridCell> rangeCells;
        public LeanTweenType expandType;

        public override void Initialize(List<GridCell> cells, SkillAbstract skill) { }

        public void Initialize(List<GridCell> cells, IntVector2 origin)
        {
            rangeCells = new List<GridCell>();
            GridCell[] holdCells = cells.ToArray();
            for (int i = 0; i < holdCells.Length / 2; i++)
            {
                int index = Mathf.FloorToInt(Random.Range(0, holdCells.Length));
                if (rangeCells.Contains(holdCells[index]))
                    i--;
                else
                    rangeCells.Add(holdCells[index]);
            }
            LeanTween.scale(gameObject, Vector3.one * 10f, 1f).setOnComplete(ShootProjectile).setEase(expandType);
        }

        void ShootProjectile()
        {
            LeanTween.scale(gameObject, Vector3.one * 0.1f, 0.5f).setOnComplete(Destroy);
            foreach (GridCell cell in rangeCells)
            {
                GameObject ob = Instantiate(projectilePrefab, transform.position, transform.rotation);
                ob.transform.LookAt(Vector3.up + transform.position);
                ob.GetComponent<IceBombProjectile>().Initalize(cell);
            }
        }

        void Destroy()
        {
            Destroy(gameObject);
        }
    }
}