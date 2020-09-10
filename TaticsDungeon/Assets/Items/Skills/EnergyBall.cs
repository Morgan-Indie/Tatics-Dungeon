using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class EnergyBall : SkillAbstract
    {
        public bool animationCompleted;
        public GameObject target;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler, TaticalMovement _taticalMovement, Skill _skill)
        {
            EnergyBall energyBall = _characterStats.gameObject.AddComponent<EnergyBall>();
            energyBall.characterStats = _characterStats;
            energyBall.animationHandler = _animationHandler;
            energyBall.taticalMovement = _taticalMovement;
            energyBall.skill = _skill;
            return energyBall;
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
                    GameObject effect = Instantiate(skill.effectPrefab, taticalMovement.transform.position + Vector3.up * 1.5f + taticalMovement.transform.forward * 1f, Quaternion.identity);
                    effect.GetComponent<InstantCastSpawn>().Initalize(cells, this);
                }
            }
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            AlchemyManager.Instance.ApplyHeat(targetCell.alchemyState);
        }
    }
}