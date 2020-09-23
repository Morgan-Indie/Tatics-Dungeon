using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class IceSpikeSpawn : VFXSpawns
    {
        public GridCell targetCell;
        public SkillAbstract skillScript;

        public override void Initialize(List<GridCell> cells, SkillAbstract skill, IntVector2 origin)
        {
            lifeTime = 2f;
            targetCell = cells[0];
            skillScript = skill;
            GameObject ob = Instantiate(projectilePrefab, cells[0].transform.position, transform.rotation);

            Destroy(ob, lifeTime);
            Destroy(gameObject, lifeTime);
        }

        public void OnDestroy()
        {
            skillScript.Excute(Time.deltaTime, targetCell);
        }
    }
}