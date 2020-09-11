﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CastShock 
    {
        public static void Activate(CharacterStats characterStats, AnimationHandler animationHandler, TaticalMovement taticalMovement, Skill skill, float delta)
        {
            IntVector2 index = taticalMovement.GetMouseIndex();
            GridManager.Instance.HighlightCastableRange(taticalMovement.currentIndex, index, skill);

            if (index.x >= 0 && characterStats.currentAP >= skill.APcost)
            {
                if (Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput)
                {
                    InputHandler.instance.tacticsXInput = false;

                    animationHandler.PlayTargetAnimation("Attack");
                    characterStats.UseAP(skill.APcost);
                    GridManager.Instance.RemoveAllHighlights();
                    List<GridCell> cells = CastableShapes.GetCastableCells(skill, index);
                    foreach (GridCell cell in cells)
                    {
                        AlchemyManager.Instance.ApplyShock(cell.alchemyState);
                    }
                }
            }
        }
    }
}