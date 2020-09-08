using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame {
    public class InstantCastSpawn : MonoBehaviour
    {
        public GameObject projectilePrefab;
        public float lifeTime = 1f;
        
        public void Initalize(List<GridCell> cells, SkillAbstract skill)
        {
            foreach (GridCell cell in cells)
            {
                transform.LookAt(cell.transform);
                GameObject ob = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                ob.GetComponent<InstantCastProjectile>().Initalize(cell, skill);
                Destroy(gameObject, lifeTime);
            }
        }
    }
}