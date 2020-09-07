using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class EnergyBall 
    {
        public static void Activate(CharacterStats characterStats, AnimationHandler animationHandler, TaticalMovement taticalMovement, Skill skill, float delta)
        {
            IntVector2 index = taticalMovement.GetMouseIndex();
            GridManager.Instance.HighlightCastableRange(taticalMovement.currentIndex, index, skill);
            int distance = taticalMovement.GetMouseDistance(index);

            if (index.x >= 0 && characterStats.currentAP >= skill.APcost)
            {
                if (Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput)
                {
                    InputHandler.instance.tacticsXInput = false;

                    animationHandler.PlayTargetAnimation("Attack");
                    characterStats.UseAP(skill.APcost);
                    GridManager.Instance.RemoveAllHighlights();
                    List<GridCell> cells = CastableShapes.GetCastableCells(skill, index);
                    GameObject effect = SpellManager.Instance.BuildSpellPrefab(skill.effectPrefab, taticalMovement.transform.position + Vector3.up * 1.5f + taticalMovement.transform.forward * 1f);
                    effect.GetComponent<InstantCastSpawn>().Initalize(cells);
                }
            }
        }
    }
}