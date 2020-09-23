using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class OilFlaskSpawn : VFXSpawns
    {
        public Transform playerHand;
        public GridCell targetCell;
        public SkillAbstract skillScript;

        public override void Initialize(List<GridCell> cells, SkillAbstract skill, IntVector2 origin)
        {
            targetCell = cells[0];
            skillScript = skill;
            playerHand = skill.gameObject.GetComponent<EquipmentSlotManager>().rightHandSlot.parentOverride;
            lifeTime = 1f;
            GameObject ob = Instantiate(projectilePrefab, playerHand.position, playerHand.rotation);

            Vector3 endControl = (ob.transform.position + targetCell.transform.position) / 2 + Vector3.up * 2f;

            LTBezierPath Path = new LTBezierPath(new Vector3[] { ob.transform.position, endControl, endControl, targetCell.transform.position });
            LeanTween.move(ob, Path, 1.0f).setOrientToPath(true);
            Destroy(ob, lifeTime);
            Destroy(gameObject, lifeTime);
        }

        public void OnDestroy()
        {
            skillScript.Excute(Time.deltaTime, targetCell);
        }
    }
}