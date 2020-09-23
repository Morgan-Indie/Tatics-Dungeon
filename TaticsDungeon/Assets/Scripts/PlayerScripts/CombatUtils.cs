using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CombatUtils : MonoBehaviour
    {
        [Header("Not Required")]
        public CharacterStats characterStats;
        public EquipmentSlotManager equipmentSlotManager;

        public void Start()
        {
            characterStats = GetComponent<CharacterStats>();
        }

        public DamageStruct ComputeDamage(SkillAbstract skillScript)
        {        
            DamageStruct outputDamage = new DamageStruct(skillScript.peirceDamage.Value,
                skillScript.normalDamage.Value, skillScript.alchemicalDamage.Value, skillScript.alchemicalDamgeType);
            return outputDamage;
        }

        public void DealDamage(CharacterStats targetStats, SkillAbstract skillScript)
        {
            DamageStruct outputDamage = ComputeDamage(skillScript);

            int damageDeltNormal = (int)(outputDamage.normal * (1-targetStats.armor.Value/300f));
            //Debug.Log(characterStats.characterName + " damageDeltNormal: " + damageDeltNormal);

            int damageDeltPierce = outputDamage.pierce;
            //Debug.Log(characterStats.characterName + " damageDeltPierce: " + damageDeltPierce);
            
            int damageDeltAlchemical = (int)(outputDamage.alchemical*(1f - targetStats.playerResistanceStatDict[outputDamage.alchemicalType].Value / 200f));
            //Debug.Log(characterStats.characterName + " "+ outputDamage.alchemicalType+ ": " + damageDeltAlchemical);

            int totalDamage = damageDeltPierce + damageDeltAlchemical + damageDeltNormal;
            totalDamage = totalDamage>0 ? totalDamage: 0;
            targetStats.TakeDamage(totalDamage);
        }

        public void HandleAlchemicalSkillCharacter(CharacterStats targetStats, SkillAbstract skillScript)
        {
            DealDamage(targetStats, skillScript);
        }

        public void HandleAlchemicalSkillCell(GridCell targetCell, SkillAbstract skillScript)
        {
            targetCell.heatState.AddHeat(skillScript.heatValue);

            switch (skillScript.skill.type)
            {
                case SkillType.CastHeat:
                    (targetCell.substances,targetCell.heatState.Value) = AlchemyEngine.instance.ApplyAlchemicalTransfomation(
                        targetCell.substances, targetCell);
                    break;

                case SkillType.CastSubstance:
                    CastSubstance castSubstance = (CastSubstance)skillScript;
                    (targetCell.substances, targetCell.heatState.Value) =AlchemyEngine.instance.ApplyDirectSubstance(
                        castSubstance.substance,targetCell.substances, targetCell);
                    break;
            }

            AlchemyEngine.instance.ApplyStateVFX(targetCell);

            if (ActivateVFX.Instance.PoisonCheckCell != null)
            {
                AlchemyEngine.instance.PropagateEffect(ActivateVFX.Instance.PoisonCheckCell, StatusEffect.Poisoned);
                ActivateVFX.Instance.PoisonCheckCell = null;
            }
            if (ActivateVFX.Instance.ShockCheckCell != null)
            {
                AlchemyEngine.instance.PropagateEffect(ActivateVFX.Instance.ShockCheckCell, StatusEffect.Shocked);
                ActivateVFX.Instance.ShockCheckCell = null;
            }
        }
    }
}

