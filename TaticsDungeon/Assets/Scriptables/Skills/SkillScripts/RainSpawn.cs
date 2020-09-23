using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class RainSpawn : VFXSpawns
    {
        public override void Initialize(List<GridCell> cells, SkillAbstract skill, IntVector2 origin)
        {
            lifeTime = 2f;
            GameObject ob = Instantiate(projectilePrefab, cells[2].transform.position, transform.rotation);

            foreach (GridCell cell in cells)
            {                
                skill.Excute(Time.deltaTime, cell);
            }

            Destroy(ob, lifeTime);
            Destroy(gameObject, lifeTime);            
        }
    }
}