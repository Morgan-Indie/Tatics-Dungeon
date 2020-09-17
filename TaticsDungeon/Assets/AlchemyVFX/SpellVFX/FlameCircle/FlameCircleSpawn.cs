using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameCircleSpawn : VFXSpawns
    {
        public override void Initialize(List<GridCell> cells, SkillAbstract skill, IntVector2 origin)
        {
            skill.Excute(Time.deltaTime, cells[0]);
            Destroy(gameObject, lifeTime);
        }
    }
}