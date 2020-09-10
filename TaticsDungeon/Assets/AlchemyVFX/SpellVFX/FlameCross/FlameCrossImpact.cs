using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameCrossImpact : MonoBehaviour
    {
        public GameObject smallProjectilePrefab;
        public float lifeTime = 1f;

        public void Initalize(List<GridCell> cells, SkillAbstract skill)
        {
            foreach (GridCell cell in cells)
            {
                GameObject ob = Instantiate(smallProjectilePrefab, transform.position, Quaternion.identity);
                ob.GetComponent<FlameCrossSmallProjectile>().Initalize(cell, skill);
            }
            Destroy(gameObject, lifeTime);
        }
    }
}