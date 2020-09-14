using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame {
    public class InstantCastSpawn : VFXSpawns
    {        
        public override void Initialize(List<GridCell> cells, SkillAbstract skill)
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