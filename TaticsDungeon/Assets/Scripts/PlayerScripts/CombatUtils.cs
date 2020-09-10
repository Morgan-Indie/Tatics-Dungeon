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
            int damageDeltNormal = outputDamage.normal -(int)targetStats.armor.Value;
            int damageDeltPierce = outputDamage.pierce;
            int damageDeltFire = outputDamage.fire -(int)targetStats.resistance.Value;
            int damageDeltWater = outputDamage.water - (int)targetStats.resistance.Value;
            int damageDeltShock = outputDamage.shock - (int)targetStats.resistance.Value;
            int damageDeltPoison = outputDamage.poison;

            int totalDamage = damageDeltPierce + damageDeltPoison + damageDeltNormal+ damageDeltWater + damageDeltFire+ damageDeltShock;
            targetStats.TakeDamage(totalDamage);
        }

        public void SetFireInteractions(CharacterStats targetCharacterStats, Object source)
        {
            if (targetCharacterStats.stateManager.statusEffects.Contains (StatusEffect.Wet))
            {
                targetCharacterStats.stateManager.statusEffects.Remove(StatusEffect.Wet);
            }
            else if (targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Frozen))
            {
                targetCharacterStats.stateManager.statusEffects.Remove(StatusEffect.Wet);
            }

            else
            {
                if (typeof(GridCell)== source.GetType())
                {
                    GridCell cell = (GridCell)source;
                    targetCharacterStats.stateManager.statusEffects.Add(StatusEffect.Burning);
                    ElementalVFX.Instance.ActivateEffect(StatusEffect.Burning, targetCharacterStats.gameObject);
                    if (targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Oiled))
                    {
                        targetCharacterStats.stateManager.statusEffects.Remove(StatusEffect.Oiled);
                        StatModifier burnDamageMod = new StatModifier(cell.BurnDamage * .5f, StatModType.Flat, cell);
                        targetCharacterStats.stateManager.burnDamageOverTime.AddModifier(burnDamageMod);
                        targetCharacterStats.stateManager.DamageSourceTurns.Add(cell.alchemyState, (CombatStatType.fireDamage, 3));
                    }
                    else
                    {
                        StatModifier burnDamageMod = new StatModifier(cell.BurnDamage * .25f, StatModType.Flat, cell);
                        targetCharacterStats.stateManager.burnDamageOverTime.AddModifier(burnDamageMod);
                        targetCharacterStats.stateManager.DamageSourceTurns.Add(cell.alchemyState, (CombatStatType.fireDamage, 3));
                    }
                }
                else
                {
                    SkillAbstract skill = (SkillAbstract)source;
                    if (targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Oiled))
                    {
                        targetCharacterStats.stateManager.statusEffects.Add(StatusEffect.Burning);
                        ElementalVFX.Instance.ActivateEffect(StatusEffect.Burning, targetCharacterStats.gameObject);
                        targetCharacterStats.stateManager.statusEffects.Remove(StatusEffect.Oiled);
                        StatModifier burnDamageMod = new StatModifier(skill.alchemicalDamage.Value * .5f, StatModType.Flat, skill);
                        targetCharacterStats.stateManager.burnDamageOverTime.AddModifier(burnDamageMod);
                        targetCharacterStats.stateManager.DamageSourceTurns.Add(skill, (CombatStatType.fireDamage, 3));
                    }
                    
                    else if (Random.value>=.5f)
                    {
                        ElementalVFX.Instance.ActivateEffect(StatusEffect.Burning, targetCharacterStats.gameObject);
                        targetCharacterStats.stateManager.statusEffects.Add(StatusEffect.Burning);
                        StatModifier burnDamageMod = new StatModifier(skill.alchemicalDamage.Value * .25f, StatModType.Flat, skill);
                        targetCharacterStats.stateManager.burnDamageOverTime.AddModifier(burnDamageMod);
                        if (targetCharacterStats.stateManager.DamageSourceTurns.ContainsKey(skill))
                            targetCharacterStats.stateManager.DamageSourceTurns.Remove(skill);
                        targetCharacterStats.stateManager.DamageSourceTurns.Add(skill, (CombatStatType.fireDamage, 3));
                    }
                }
            }
        }

        public void SetWaterInteractions(CharacterStats targetCharacterStats)
        {
            if (!targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Wet))
            {
                targetCharacterStats.stateManager.statusEffects.Add(StatusEffect.Wet);
                ElementalVFX.Instance.ActivateEffect(StatusEffect.Wet, targetCharacterStats.gameObject);
            }
        }

        public void SetChillInteractions(CharacterStats targetCharacterStats)
        {
            if (targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Wet))
            {
                targetCharacterStats.stateManager.statusEffects.Remove(StatusEffect.Wet);
                targetCharacterStats.stateManager.statusEffects.Add(StatusEffect.Frozen);
            }
            else if (targetCharacterStats.stateManager.statusEffects.Contains(StatusEffect.Shocked))
            {
                targetCharacterStats.stateManager.statusEffects.Remove(StatusEffect.Shocked);
                targetCharacterStats.stateManager.statusEffects.Add(StatusEffect.Electricuted);
            }
        }

        public void OffensiveSpell(GameObject targetCharacter, SkillAbstract skillScript)
        {
            CharacterStats targetStats = targetCharacter.GetComponent<CharacterStats>();

            switch (skillScript.skill.type)
            {
                case SkillType.Fire:
                    int fireDamage = (int)skillScript.alchemicalDamage.Value - (int)targetStats.resistance.Value;
                    targetStats.TakeDamage(fireDamage);
                    SetFireInteractions(targetStats, skillScript);
                    break;
                case SkillType.Water:
                    int WaterDamage = (int)skillScript.alchemicalDamage.Value - (int)targetStats.resistance.Value;
                    targetStats.TakeDamage(WaterDamage);
                    SetWaterInteractions(targetStats);
                    break;
                case SkillType.Chill:
                    int ChillDamage = (int)skillScript.alchemicalDamage.Value - (int)targetStats.resistance.Value;
                    targetStats.TakeDamage(ChillDamage);
                    SetChillInteractions(targetStats);
                    break;
            }
        }
    }
}

