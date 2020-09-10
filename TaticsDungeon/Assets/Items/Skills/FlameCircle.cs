using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameCircle : SkillAbstract
    {
        public bool animationCompleted;
        public GameObject target;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler, TaticalMovement _taticalMovement, Skill _skill)
        {
            FlameCircle flameCircle = _characterStats.gameObject.AddComponent<FlameCircle>();
            flameCircle.characterStats = _characterStats;
            flameCircle.animationHandler = _animationHandler;
            flameCircle.taticalMovement = _taticalMovement;
            flameCircle.skill = _skill;
            return flameCircle;
        }
        public override void Activate(float delta)
        {
            IntVector2 index = taticalMovement.GetMouseIndex();
            GridManager.Instance.HighlightCastableRange(taticalMovement.currentIndex, index, skill);
            int distance = taticalMovement.currentIndex.GetDistance(index);

            if (index.x >= 0 && characterStats.currentAP >= skill.APcost && distance <= skill.castableSettings.range)
            {
                if (Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput &&
                    characterStats.stateManager.characterState != CharacterState.IsInteracting)
                {
                    GridManager.Instance.RemoveAllHighlights();
                    InputHandler.instance.tacticsXInput = false;
                    List<GridCell> cells = CastableShapes.GetCastableCells(skill, index);
                    GameObject effect = Instantiate(skill.effectPrefab, cells[0].transform.position + Vector3.up * 0.25f, Quaternion.identity);
                    effect.GetComponent<FlameCircleSpawn>().Initalize(cells[0], this);
                }
            }
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            AlchemyManager.Instance.ApplyHeat(targetCell.alchemyState);
        }
    }
}