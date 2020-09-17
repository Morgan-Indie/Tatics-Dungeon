using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameCrossSpawn : VFXSpawns
    {
        public override void Initialize(List<GridCell> cells, SkillAbstract skill, IntVector2 origin)
        {
            GameObject ob = Instantiate(projectilePrefab, transform.position, transform.rotation);
            ob.GetComponent<FlameCrossProjectile>().Initalize(cells, skill);
            Destroy(gameObject, lifeTime);
        }
    }
}