using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CharacterCombatStats : MonoBehaviour
    {
        #region DamageStats
        public DamageStat baseDamage;
        public DamageStat pierceDamage;
        public DamageStat fireDamage;
        public DamageStat waterDamage;
        public DamageStat shockDamage;
        public DamageStat poisonDamage;
        #endregion

        #region DefenseStats
        public DefenseStat armor;
        public DefenseStat fireResistance;
        public DefenseStat waterResistance;
        public DefenseStat shockResistance;
        public DefenseStat poisonResistance;
        #endregion

        public Dictionary<DamageStatType, DamageStat> damageStatDict = new Dictionary<DamageStatType, DamageStat>();
        public Dictionary<DefenseStatType, DefenseStat> defenseStatDict = new Dictionary<DefenseStatType, DefenseStat>();
        public Dictionary<DamageStatType, DefenseStat> combatStatDict = new Dictionary<DamageStatType, DefenseStat>();

        void Awake()
        {
            InitializeDefenseStats();
            InitializeDamageStats();
            CollectDefenseStats();
            CollectCombatStats();
            CollectDamageStats();
        }

        void InitializeDefenseStats()
        {
            armor = new DefenseStat(0, DefenseStatType.armor);
            fireResistance = new DefenseStat(0, DefenseStatType.fireResistance);
            waterResistance = new DefenseStat(0, DefenseStatType.waterResistance);
            shockResistance = new DefenseStat(0, DefenseStatType.shockResistance);
            poisonResistance = new DefenseStat(0, DefenseStatType.poisonResistance);
        }

        void InitializeDamageStats()
        {
            baseDamage = new DamageStat(0, DamageStatType.normalDamage);
            fireDamage = new DamageStat(0, DamageStatType.fireDamage);
            waterDamage = new DamageStat(0, DamageStatType.waterDamage);
            shockDamage = new DamageStat(0, DamageStatType.shockDamage);
            poisonDamage = new DamageStat(0, DamageStatType.poisonDamage);
        }

        void CollectDefenseStats()
        {
            defenseStatDict.Add(DefenseStatType.armor, armor);
            defenseStatDict.Add(DefenseStatType.fireResistance, fireResistance);
            defenseStatDict.Add(DefenseStatType.waterResistance, waterResistance);
            defenseStatDict.Add(DefenseStatType.shockResistance, shockResistance);
            defenseStatDict.Add(DefenseStatType.poisonResistance, poisonResistance);
        }

        void CollectCombatStats()
        {
            combatStatDict.Add(DamageStatType.waterDamage, waterResistance);
            combatStatDict.Add(DamageStatType.shockDamage, shockResistance);
            combatStatDict.Add(DamageStatType.fireDamage, fireResistance);
            combatStatDict.Add(DamageStatType.poisonDamage, poisonResistance);
            combatStatDict.Add(DamageStatType.normalDamage, armor);
        }

        void CollectDamageStats()
        {
            damageStatDict.Add(DamageStatType.normalDamage, baseDamage);
            damageStatDict.Add(DamageStatType.fireDamage, fireDamage);
            damageStatDict.Add(DamageStatType.waterDamage, waterDamage);
            damageStatDict.Add(DamageStatType.shockDamage, shockDamage);
            damageStatDict.Add(DamageStatType.poisonDamage, poisonDamage);
        }
    }
}

