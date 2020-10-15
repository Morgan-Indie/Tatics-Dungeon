using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class EquipmentModifiersHandler : MonoBehaviour
    {
        public CharacterStats characterStats;
        // Start is called before the first frame update

        void Awake()
        {
            characterStats = GetComponent<CharacterStats>();
        }

        #region Handle Stat Modifiers

        // add modifiers for the character's attributes, strength, intellegence etc... from the equipment
        private void ApplyCharacterModifiers(EquipableItem item)
        {
            foreach (var mod in item.statModDict)
                characterStats.playerAttributeDict[mod.Key].AddModifier(mod.Value);
        }

        // add base damage and defense etc.. from the item
        private void ApplyEquipmentStats(EquipableItem item)
        {
            foreach (var mod in item.equipmentStatDict)
            {
                characterStats.playerCombatStatDict[mod.Key].AddModifier(mod.Value);
            }
        }

        private void ApplyEquipmentScalers(EquipableItem item)
        {
            foreach (var mod in item.scaleModDict)
            {
                float scaleFactor = characterStats.playerAttributeDict[mod.Key].Value * mod.Value;                
                if (item._pierceDamage != 0)
                    characterStats.pierceDamage.AddModifier(new StatModifier(scaleFactor, StatModType.Flat, item));
                if (item._alchemicalDamage != 0)
                {
                    characterStats.playerCombatStatDict[item.alchemicalType].AddModifier(new StatModifier(scaleFactor, 
                        StatModType.Flat, item));
                    characterStats.currentAlchemcialType = item.alchemicalType;
                }
                if (item._normalDamage != 0)
                    characterStats.normalDamage.AddModifier(new StatModifier(scaleFactor, StatModType.Flat, item));
                if (item._armor != 0)
                    characterStats.armor.AddModifier(new StatModifier(scaleFactor, StatModType.Flat, item));
                if (item._fireResistance != 0)
                    characterStats.fireResistance.AddModifier(new StatModifier(scaleFactor, StatModType.Flat, item));
                if (item._waterResistance != 0)
                    characterStats.waterResistance.AddModifier(new StatModifier(scaleFactor, StatModType.Flat, item));
                if (item._shockResistance != 0)
                    characterStats.shockResistance.AddModifier(new StatModifier(scaleFactor, StatModType.Flat, item));
                if (item._poisonResistance != 0)
                    characterStats.poisonResistance.AddModifier(new StatModifier(scaleFactor, StatModType.Flat, item));
            }
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
            ApplyEquipmentStats(item);
            ApplyEquipmentScalers(item);
        }
    }
}

