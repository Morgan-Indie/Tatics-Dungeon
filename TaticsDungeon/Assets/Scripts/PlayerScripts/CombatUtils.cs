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

        public void HandleAlchemicalSkillCharacter(CharacterStateManager targetCharacter, GridCell targetCell, SkillAbstract skillScript)
        {
            DealDamage(targetCharacter.characterStats, skillScript);
            targetCharacter.characterSubstances = AlchemyEngine.instance.MergeSubstances(targetCell, targetCharacter);

            switch (skillScript.skill.type)
            {
                case SkillType.CastHeat:
                    (targetCell.substances, targetCharacter.characterSubstances,targetCharacter.statusEffects, targetCharacter.statusTurns,
                        targetCell.heatState.Value,targetCharacter.heatState.Value) = AlchemyEngine.instance.ApplyAlchemicalTransfomation(
                        targetCell,targetCharacter,skillScript);
                    break;

                case SkillType.CastSubstance:
                    CastSubstance castSubstance = (CastSubstance)skillScript;
                    targetCell.substances = AlchemyEngine.instance.ApplyDirectSubstance(castSubstance.substance, targetCell.substances);

                    (targetCharacter.characterSubstances,targetCharacter.statusEffects,targetCharacter.statusTurns) = AlchemyEngine.instance.ApplyDirectSubstance(
                        castSubstance.substance, targetCharacter.characterSubstances,targetCharacter.statusEffects, targetCharacter.statusTurns);

                    (targetCell.substances, targetCharacter.characterSubstances, targetCharacter.statusEffects, targetCharacter.statusTurns,
                        targetCell.heatState.Value, targetCharacter.heatState.Value) = AlchemyEngine.instance.ApplyAlchemicalTransfomation(
                        targetCell, targetCharacter, skillScript);
                    break;
            }

            AlchemyEngine.instance.ApplyStateVFX(targetCell);
            AlchemyEngine.instance.ApplyStateVFX(targetCharacter);
        }

        public void HandleAlchemicalSkillCell(GridCell targetCell, SkillAbstract skillScript)
        {
            targetCell.heatState= targetCell.heatState + skillScript.heatState;

            switch (skillScript.skill.type)
            {
                case SkillType.CastHeat:
                    (targetCell.substances,targetCell.heatState.Value) = AlchemyEngine.instance.ApplyAlchemicalTransfomation(targetCell);
                    break;

                case SkillType.CastSubstance:
                    CastSubstance castSubstance = (CastSubstance)skillScript;
                    targetCell.substances = AlchemyEngine.instance.ApplyDirectSubstance(castSubstance.substance,targetCell.substances);
                    (targetCell.substances, targetCell.heatState.Value) = AlchemyEngine.instance.ApplyAlchemicalTransfomation(targetCell);
                    break;
            }

            AlchemyEngine.instance.ApplyStateVFX(targetCell);
        }
    }
}

