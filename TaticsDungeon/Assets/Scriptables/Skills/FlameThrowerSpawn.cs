using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameThrowerSpawn : VFXSpawns
    {
        public Transform playerHand;

        public override void Initialize(List<GridCell> cells, SkillAbstract skill, IntVector2 origin)
        {
            lifeTime = 2f;
            playerHand = skill.gameObject.GetComponent<EquipmentSlotManager>().leftHandSlot.parentOverride;

            GameObject ob = Instantiate(projectilePrefab, playerHand.transform.position, transform.rotation);
            ob.transform.LookAt(cells[0].transform);
            Debug.Log(cells.Count);

            foreach (GridCell cell in cells)
            {
                skill.Excute(Time.deltaTime, cell);
            }

            Destroy(ob, lifeTime);
            Destroy(gameObject, lifeTime);
        }
    }
}