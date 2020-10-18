using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class EquipmentModifiersHandler : MonoBehaviour
    {
        public CharacterStats stats;
        public CharacterCombatStats combatStats;
        // Start is called before the first frame update

        void Awake()
        {
            stats = GetComponent<CharacterStats>();
            combatStats = GetComponent<CharacterCombatStats>();
        }

        public void ApplyCharacterModifiers(EquipableItem item)
        {
            foreach (var mod in item.statModDict)
                stats.attributeDict[mod.Key].AddModifier(mod.Value);
        }

        #region ApplyAttributeScaling
        public void ApplyDamageScalers(EquipableItem item)
        {
            foreach (var mod in item.scaleModDict)
            {
                StatModifier StatMod = new StatModifier(stats.attributeDict[mod.Key].Value * mod.Value, StatModType.PercentAdd, item);
                foreach (DamageStat stat in item.damageDict.Values)
                    stat.AddModifier(StatMod);
            }
        }

        public void ApplyDefenseScalers(EquipableItem item)
        {
            foreach (var mod in item.scaleModDict)
            {
                StatModifier StatMod = new StatModifier(stats.attributeDict[mod.Key].Value * mod.Value, StatModType.PercentAdd, item);
                foreach (DefenseStat stat in item.defenseDict.Values)
                    stat.AddModifier(StatMod);
            }
        }
        #endregion

        #region Apply equipment stats to characterCombatStats

        public void ApplyDamageStats(EquipableItem item)
        {
            foreach (DamageStat stat in item.damageDict.Values)
                combatStats.damageStatDict[stat.type].AddModifier(new StatModifier(stat.Value, StatModType.Flat, item));
        }

        public void ApplyDefenseStats(EquipableItem item)
        {
            foreach (DefenseStat stat in item.defenseDict.Values)
                combatStats.defenseStatDict[stat.type].AddModifier(new StatModifier(stat.Value, StatModType.Flat, item));
        }
        #endregion

        public void RemoveAllModifiers(EquipableItem item)
        {
            foreach (var mod in characterStats.playerAttributeDict)
                mod.Value.RemoveAllModifiersFromSource(item);
            foreach (var mod in characterStats.playerCombatStatDict)
                mod.Value.RemoveAllModifiersFromSource(item);
            characterStats.currentAlchemcialType = CombatStatType.normalDamage;
        }

        public void ApplyEquipmentModifiers(EquipableItem item)
        {
            ApplyCharacterModifiers(item);
            if (item.isWeapon)
            {
                ApplyDamageScalers(item);
                ApplyDamageStats(item);
            }
        }
    }
}

