﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CombatUtils : MonoBehaviour
    {
        [Header("Not Required")]
        public static CombatUtils Instance = null;

        public void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

<<<<<<< Updated upstream
        public DamageStruct ComputeDamage()
        {
            float totalNormalDamage = 0f;
            float totalPierceDamage = 0f;
            float totalFireDamge = 0f;
            float totalWaterDamge = 0f;
            float totalPoisonDamge = 0f;
            float totalShockDamge = 0f;

            foreach (var damageStat in characterStats.playerCombatStatDict)
                if (damageStat.Key == CombatStatType.normalDamage)
                    totalNormalDamage += damageStat.Value.Value;
                else if (damageStat.Key == CombatStatType.pierceDamage)
                    totalPierceDamage += damageStat.Value.Value;
                else if (damageStat.Key == CombatStatType.fireDamage)
                    totalFireDamge += damageStat.Value.Value;
                else if (damageStat.Key == CombatStatType.waterDamage)
                    totalWaterDamge += damageStat.Value.Value;
                else if (damageStat.Key == CombatStatType.shockDamage)
                    totalShockDamge += damageStat.Value.Value;
                else if (damageStat.Key == CombatStatType.poisonDamage)
                    totalPoisonDamge += damageStat.Value.Value;

            DamageStruct outputDamage = new DamageStruct(totalFireDamge,
                totalWaterDamge, totalPoisonDamge, totalShockDamge,
                totalPierceDamage, totalNormalDamage);
            return outputDamage;
        }

        public void PhyiscalAttack(GameObject targetCharacter)
        {
            CharacterStats targetStats = targetCharacter.GetComponent<CharacterStats>();
            DamageStruct outputDamage = ComputeDamage();
            int damageDeltNormal = outputDamage.normal - (int)targetStats.armor.Value;
            int damageDeltPierce = outputDamage.pierce;
            int damageDeltFire = outputDamage.fire - (int)targetStats.fireResistance.Value;
            int damageDeltWater = outputDamage.water - (int)targetStats.waterResistance.Value;
            int damageDeltShock = outputDamage.shock - (int)targetStats.shockResistance.Value;
            int damageDeltPoison = outputDamage.poison - (int)targetStats.poisonResistance.Value;

            int totalDamage = damageDeltPierce + damageDeltPoison + damageDeltNormal + damageDeltWater + damageDeltFire + damageDeltShock;
            totalDamage = totalDamage>0 ? totalDamage: 0;
            targetStats.TakeDamage(totalDamage);
        }

        public void SetFireInteractions(CharacterStats targetCharacterStats, object source, int burnDamage, bool fromCell = false)
        {
            if (targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Wet))
            {
                targetCharacterStats.stateManager.statusEffects.Remove(StatusEffect.Wet);
            }
            else if (targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Frozen))
            {
                targetCharacterStats.stateManager.statusEffects.Remove(StatusEffect.Wet);
            }

            else
            {
                if (fromCell)
                {
                    GridCell cell = (GridCell)source;
                    if (targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Oiled) || cell.alchemyState.fireState == FireState.Inferno)
                    {
                        targetCharacterStats.stateManager.statusEffects.Add(StatusEffect.Inferno);
                        ActivateVFX.Instance.ActivateElementalEffect(StatusEffect.Inferno, targetCharacterStats.gameObject);
                        targetCharacterStats.stateManager.statusEffects.Remove(StatusEffect.Oiled);
                        StatModifier burnDamageMod = new StatModifier(burnDamage * .5f, StatModType.Flat, cell.burnSource);
                        targetCharacterStats.stateManager.burnDamageOverTime.AddModifier(burnDamageMod);
                        targetCharacterStats.stateManager.DamageSourceTurns.Add(cell.burnSource, (CombatStatType.fireDamage, 3));
                    }
                    else
                    {
                        targetCharacterStats.stateManager.statusEffects.Add(StatusEffect.Burning);
                        ActivateVFX.Instance.ActivateElementalEffect(StatusEffect.Burning, targetCharacterStats.gameObject);
                        StatModifier burnDamageMod = new StatModifier(burnDamage * .25f, StatModType.Flat, cell.burnSource);
                        targetCharacterStats.stateManager.burnDamageOverTime.AddModifier(burnDamageMod);
                        targetCharacterStats.stateManager.DamageSourceTurns.Add(cell.burnSource, (CombatStatType.fireDamage, 3));
                    }
                }
                else
                {
                    if (targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Oiled))
                    {
                        targetCharacterStats.stateManager.statusEffects.Add(StatusEffect.Inferno);
                        ActivateVFX.Instance.ActivateElementalEffect(StatusEffect.Inferno, targetCharacterStats.gameObject);
                        targetCharacterStats.stateManager.statusEffects.Remove(StatusEffect.Oiled);
                        StatModifier burnDamageMod = new StatModifier(burnDamage * .5f, StatModType.Flat, source);
                        targetCharacterStats.stateManager.burnDamageOverTime.AddModifier(burnDamageMod);
                        targetCharacterStats.stateManager.DamageSourceTurns.Add(source, (CombatStatType.fireDamage, 3));
                    }

                    else if (Random.value >= .5f)
                    {
                        ActivateVFX.Instance.ActivateElementalEffect(StatusEffect.Burning, targetCharacterStats.gameObject);
                        targetCharacterStats.stateManager.statusEffects.Add(StatusEffect.Burning);
                        StatModifier burnDamageMod = new StatModifier(burnDamage * .25f, StatModType.Flat, source);
                        targetCharacterStats.stateManager.burnDamageOverTime.AddModifier(burnDamageMod);
                        if (targetCharacterStats.stateManager.DamageSourceTurns.ContainsKey(source))
                            targetCharacterStats.stateManager.DamageSourceTurns.Remove(source);
                        targetCharacterStats.stateManager.DamageSourceTurns.Add(source, (CombatStatType.fireDamage, 3));
                    }
                }
            }
        }

        public void SetWaterInteractions(CharacterStats targetCharacterStats)
        {
            if (!targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Wet))
            {
                targetCharacterStats.stateManager.statusEffects.Add(StatusEffect.Wet);
                ActivateVFX.Instance.ActivateElementalEffect(StatusEffect.Wet, targetCharacterStats.gameObject);
            }
        }

        public void SetChillInteractions(CharacterStats targetCharacterStats, GridCell cell, bool fromCell = false)
        {
            if (fromCell)
            {
                if (targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Wet))
                    targetCharacterStats.stateManager.statusEffects.Remove(StatusEffect.Wet);
                if (!targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Frozen))
                    targetCharacterStats.stateManager.statusEffects.Add(StatusEffect.Frozen);
            }

            else if (targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Wet))
            {
                targetCharacterStats.stateManager.statusEffects.Remove(StatusEffect.Wet);
                if (!targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Frozen))
                    targetCharacterStats.stateManager.statusEffects.Add(StatusEffect.Frozen);
                AlchemyManager.Instance.ApplySolid(cell.alchemyState, SolidPhaseState.Water);
            }
=======
        public DamageStruct ComputeDamage(SkillAbstract skillScript)
        {
            float pierceDamage = 0f;
            float alchemicalDamage = 0f;
            float normalDamage = 0f;
            DamageStatType alchemicalType = DamageStatType.None;

            foreach (DamageStat stat in skillScript.damageStatsList)
            {
                switch (stat.type)
                {
                    case DamageStatType.normalDamage:
                        normalDamage = stat.Value*skillScript.character.combatStats.baseDamage.Value;
                        break;

                    case DamageStatType.pierceDamage:
                        pierceDamage = stat.Value * skillScript.character.combatStats.baseDamage.Value;
                        break;

                    default:
                        alchemicalDamage = stat.Value * skillScript.character.combatStats.baseDamage.Value;
                        alchemicalType = stat.type;
                        break;
                }
            }

            DamageStruct outputDamage = new DamageStruct(pierceDamage, normalDamage, alchemicalDamage, alchemicalType);
            return outputDamage;
        }

        public void DealDamage(CharacterManager targetCharacter, DamageStruct damage)
        {
            int damageDeltNormal = (int)(damage.normal * (1 - targetCharacter.combatStats.armor.Value / 300f));

            int damageDeltPierce = damage.pierce;

            int damageDeltAlchemical = (int)(damage.alchemical * (1f - targetCharacter.combatStats.combatStatDict[damage.alchemicalType].Value / 200f));

            int totalDamage = damageDeltPierce + damageDeltAlchemical + damageDeltNormal;
            totalDamage = totalDamage > 0 ? totalDamage : 0;
            targetCharacter.health.TakeDamage(totalDamage);
        }

        public void DealDamage(CharacterManager targetCharacter, SkillAbstract skillScript)
        {
            DamageStruct outputDamage = ComputeDamage(skillScript);
            DealDamage(targetCharacter, outputDamage);
        }

        public void HandleAlchemicalSkillCharacter(CharacterManager targetCharacter, GridCell targetCell, SkillAbstract skillScript)
        {
            DealDamage(targetCharacter, skillScript);
            CharacterStateManager characterState = targetCharacter.stateManager;
            characterState.characterSubstances = AlchemyEngine.instance.MergeSubstances(targetCell, characterState);

            switch (skillScript.skill.type)
            {
                case SkillType.CastHeat:
                    (targetCell.substances, characterState.characterSubstances, characterState.statusEffects, characterState.statusTurns,
                        targetCell.heatState.Value, characterState.heatState.Value) = AlchemyEngine.instance.ApplyAlchemicalTransfomation(
                        targetCell, characterState, skillScript);
                    break;

                case SkillType.CastSubstance:
                    CastSubstance castSubstance = (CastSubstance)skillScript;
                    targetCell.substances = AlchemyEngine.instance.ApplyDirectSubstance(castSubstance.substance, targetCell.substances);

                    (characterState.characterSubstances, characterState.statusEffects, characterState.statusTurns) = AlchemyEngine.instance.ApplyDirectSubstance(
                        castSubstance.substance, characterState.characterSubstances, characterState.statusEffects, characterState.statusTurns);

                    (targetCell.substances, characterState.characterSubstances, characterState.statusEffects, characterState.statusTurns,
                        targetCell.heatState.Value, characterState.heatState.Value) = AlchemyEngine.instance.ApplyAlchemicalTransfomation(
                        targetCell, characterState, skillScript);
                    break;
            }

            AlchemyEngine.instance.ApplyStateVFX(targetCell);
            AlchemyEngine.instance.ApplyStateVFX(characterState);
>>>>>>> Stashed changes
        }

        public void SetShockInteractions(CharacterStats targetCharacterStats, bool fromCell = false)
        {
        }

        public void HandleAlchemicalSkill(CharacterStats targetStats, SkillAbstract skillScript)
        {
            switch (skillScript.skill.type)
            {
                case SkillType.Fire:
                    int fireDamage = (int)skillScript.alchemicalDamage.Value - (int)targetStats.fireResistance.Value;
                    targetStats.TakeDamage(fireDamage);
                    break;
                case SkillType.Water:
                    int WaterDamage = (int)skillScript.alchemicalDamage.Value - (int)targetStats.waterResistance.Value;
                    targetStats.TakeDamage(WaterDamage);
                    break;
                case SkillType.Chill:
                    int ChillDamage = (int)skillScript.alchemicalDamage.Value - (int)targetStats.waterResistance.Value;
                    targetStats.TakeDamage(ChillDamage);
                    break;
            }
        }
    }
}

