using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class DangerTriangleScript : VFXSpawns
    {
        public List<GridCell> rangeCells;
        public LeanTweenType rotateType;
        public LeanTweenType collapseType;
        public float hoverHeight = 1.5f;
        public float startTimeDelay = 2f;
        public float delayDivisor = 1.5f;
        SkillAbstract skill;

        public override void Initialize(List<GridCell> cells, SkillAbstract _skill, IntVector2 origin)
        {
            rangeCells = new List<GridCell>();
            skill = _skill;
            GridCell[] holdCells = cells.ToArray();
            transform.position = new Vector3(cells[0].transform.position.x, cells[0].transform.position.y, cells[0].transform.position.z);
            LeanTween.moveY(gameObject, hoverHeight, 0.5f).setOnComplete(ShootProjectile);
            for (int i = 0; i < holdCells.Length / 2; i++)
            {
                int index = Mathf.FloorToInt(Random.Range(0, holdCells.Length));
                if (rangeCells.Contains(holdCells[index]))
                    i--;
                else
                    rangeCells.Add(holdCells[index]);
            }
        }

        void ShootProjectile()
        {
            int index = Mathf.FloorToInt(Random.Range(0, rangeCells.Count));
            GameObject ob = Instantiate(projectilePrefab, transform.position, transform.rotation);
            ob.GetComponent<FireStormProjectile>().Initalize(rangeCells[index], skill);
            rangeCells.Remove(rangeCells[index]);
            startTimeDelay /= delayDivisor;
            if (rangeCells.Count > 0)
                LeanTween.rotateAround(gameObject, new Vector3(0, 1, 0), 360f, startTimeDelay).setOnComplete(ShootProjectile).setEase(rotateType);
            else
                SpellComplete();
        }

        void SpellComplete()
        {
            LeanTween.scale(gameObject, Vector3.zero, 1f).setOnComplete(Destroy).setEase(collapseType);
        }

        void Destroy()
        {
            Destroy(gameObject);
        }


    }
}
