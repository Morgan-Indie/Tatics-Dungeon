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

            int damageDeltNormal = (int)(outputDamage.normal * (1-targetStats.armor.Value/300f));
            Debug.Log(characterStats.characterName + " damageDeltNormal: " + damageDeltNormal);

            int damageDeltPierce = outputDamage.pierce;
            Debug.Log(characterStats.characterName + " damageDeltPierce: " + damageDeltPierce);

            int damageDeltFire = (int)(outputDamage.fire * (1 - targetStats.fireResistance.Value/200f));
            Debug.Log(characterStats.characterName + " damageDeltFire: " + damageDeltFire);

            int damageDeltWater = (int)(outputDamage.water * (1 - targetStats.waterResistance.Value / 200f));
            Debug.Log(characterStats.characterName + " damageDeltWater: " + damageDeltWater);

            int damageDeltShock = (int)(outputDamage.shock * (1 - targetStats.shockResistance.Value / 200f));
            Debug.Log(characterStats.characterName + " damageDeltShock: " + damageDeltShock);

            int damageDeltPoison = (int)(outputDamage.poison * (1 - targetStats.poisonResistance.Value / 200f));
            Debug.Log(characterStats.characterName + " damageDeltPoison: " + damageDeltPoison);

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
        }

        public void SetShockInteractions(CharacterStats targetCharacterStats, bool fromCell = false)
        {
        }

        public void HandleAlchemicalSkill(CharacterStats targetStats, SkillAbstract skillScript)
        {
            switch (skillScript.skill.type)
            {
                case SkillType.Fire:
                    int fireDamage = (int)(skillScript.alchemicalDamage.Value* (1- targetStats.fireResistance.Value/200f));
                    targetStats.TakeDamage(fireDamage);
                    break;
                case SkillType.Water:
                    int WaterDamage = (int)(skillScript.alchemicalDamage.Value* (1 - targetStats.waterResistance.Value));
                    targetStats.TakeDamage(WaterDamage);
                    break;
                case SkillType.Chill:
                    int ChillDamage = (int)(skillScript.alchemicalDamage.Value * (1-targetStats.waterResistance.Value));
                    targetStats.TakeDamage(ChillDamage);
                    break;
            }
        }
    }
}

