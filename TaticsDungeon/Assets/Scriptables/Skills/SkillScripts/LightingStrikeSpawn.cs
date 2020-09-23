using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class LightingStrikeSpawn : VFXSpawns
    {
        public GridCell targetCell;
        public SkillAbstract skillScript;

        public override void Initialize(List<GridCell> cells, SkillAbstract skill, IntVector2 origin)
        {
            lifeTime = 2f;
            targetCell = cells[0];
            skillScript = skill;
            GameObject ob = Instantiate(projectilePrefab, targetCell.transform.position+Vector3.up, transform.rotation);

            Destroy(ob, lifeTime);
            Destroy(gameObject, lifeTime);
        }

        public void OnDestroy()
        {
            foreach (var substance in targetCell.substances)
            {
                if (substance.Value.alchemicalState != AlchemicalState.None)
                    substance.Value.AddAuxState(StatusEffect.Shocked);
            }
            skillScript.Excute(Time.deltaTime, targetCell);
        }
    }
}