using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    [CreateAssetMenu(menuName = "Items/Equipable Item")]
    public class EquipableItem : Items
    {
        public bool equipped;
        public SlotType slotType;
        public bool isWeapon = false;
        public EquipmentType equipmentType;

        public List<meshLocation> itemMeshLocs;
        public List<Mesh> itemMeshesMale;
        public List<Mesh> itemMeshesFemale;

        [Header("Stat Scaling 1")]
        public float _scaleValue1;
        public AttributeType scaleType1;

        [Header("Stat Scaling 2")]
        public float _scaleValue2;
        public AttributeType scaleType2;

        [Header("Damage Types")]
        public float _normalDamage = 0f;
        public float _pierceDamage = 0f;
<<<<<<< Updated upstream
        public float _poisonDamage = 0f;
        public float _fireDamage = 0f;
        public float _waterDamage = 0f;
        public float _curseDamage = 0f;
        public float _shockDamage = 0f;
=======
        public DamageStatType alchemicalType = DamageStatType.None;
        public float _alchemicalDamage = 0f;
>>>>>>> Stashed changes

        [Header("Resistances")]
        public float _armor;
        public float _fireResistance;
        public float _waterResistance;
        public float _shockResistance;
        public float _poisonResistance;

        [Header("StatModifiers 1")]
        public float modValue1 = 0f;
        public StatModType modType1;
        public AttributeType modAttribute1;

        [Header("StatModifiers 2")]
        public float modValue2 = 0f;
        public StatModType modType2;
        public AttributeType modAttribute2;

        public ItemEffect itemEffect;
        public Dictionary<AttributeType, StatModifier> statModDict = new Dictionary<AttributeType, StatModifier>();
        public Dictionary<AttributeType, float> scaleModDict = new Dictionary<AttributeType, float>();
        public Dictionary<DefenseStatType, DefenseStat> defenseDict = new Dictionary<DefenseStatType, DefenseStat>();
        public Dictionary<DamageStatType, DamageStat> damageDict = new Dictionary<DamageStatType, DamageStat>();

        public void Init()
        {
            CollectCharacterModifiers();
            CollectScalingModifiers();
            if (!isWeapon)
                CollectDefenseStats();
        }

        public void CollectCharacterModifiers()
        {
            if (modValue1!=0)
                statModDict.Add(modAttribute1, new StatModifier(modValue1, modType1, this));
            if (modValue2!=0)
                statModDict.Add(modAttribute2, new StatModifier(modValue2, modType2, this));
        }

        public void CollectScalingModifiers()
        {
            scaleModDict.Add(scaleType1, _scaleValue1);

            if (_scaleValue2!=0)
                scaleModDict.Add(scaleType2, _scaleValue2);
        }

        public void CollectDefenseStats()
        {
            if (_armor != 0)
                defenseDict[DefenseStatType.armor] = new DefenseStat(_armor, DefenseStatType.armor);
            if (_fireResistance != 0)
                defenseDict[DefenseStatType.fireResistance] = new DefenseStat(_fireResistance, DefenseStatType.fireResistance);
            if (_waterResistance != 0)
                defenseDict[DefenseStatType.waterResistance] = new DefenseStat(_waterResistance, DefenseStatType.waterResistance);
            if (_shockResistance != 0)
                defenseDict[DefenseStatType.shockResistance] = new DefenseStat(_shockResistance, DefenseStatType.shockResistance);
            if (_poisonResistance != 0)
                defenseDict[DefenseStatType.poisonResistance] = new DefenseStat(_poisonResistance, DefenseStatType.poisonResistance);
        }

        public void CollectDamageStats()
        {
<<<<<<< Updated upstream
            if (_normalDamage != 0f)
                equipmentStatDict.Add(CombatStatType.normalDamage, new StatModifier(_normalDamage, StatModType.Flat, this));
            if (_fireDamage != 0f)
                equipmentStatDict.Add(CombatStatType.fireDamage, new StatModifier(_fireDamage, StatModType.Flat, this));
            if (_pierceDamage != 0f)
                equipmentStatDict.Add(CombatStatType.pierceDamage, new StatModifier(_pierceDamage, StatModType.Flat, this));
            if (_poisonDamage != 0f)
                equipmentStatDict.Add(CombatStatType.poisonDamage, new StatModifier(_poisonDamage, StatModType.Flat, this));
            if (_shockDamage != 0f)
                equipmentStatDict.Add(CombatStatType.shockDamage, new StatModifier(_shockDamage, StatModType.Flat, this));
            if (_waterDamage != 0f)
                equipmentStatDict.Add(CombatStatType.waterDamage, new StatModifier(_waterDamage, StatModType.Flat, this));
            if (_armor != 0f)
                equipmentStatDict.Add(CombatStatType.armor, new StatModifier(_armor, StatModType.Flat, this));
            if (_fireResistance != 0f)
                equipmentStatDict.Add(CombatStatType.fireResistance, new StatModifier(_fireResistance, StatModType.Flat, this));
            if (_poisonResistance != 0f)
                equipmentStatDict.Add(CombatStatType.poisonResistance, new StatModifier(_poisonResistance, StatModType.Flat, this));
            if (_waterResistance != 0f)
                equipmentStatDict.Add(CombatStatType.waterResistance, new StatModifier(_waterResistance, StatModType.Flat, this));
            if (_shockResistance != 0f)
                equipmentStatDict.Add(CombatStatType.shockResistance, new StatModifier(_shockResistance, StatModType.Flat, this));
=======
            damageDict[DamageStatType.normalDamage] = new DamageStat(_normalDamage, DamageStatType.normalDamage);
            if (alchemicalType!=DamageStatType.None)
                damageDict[alchemicalType] = new DamageStat(_alchemicalDamage, alchemicalType);
            if (_pierceDamage!=0)
                damageDict[DamageStatType.pierceDamage] = new DamageStat(_pierceDamage, DamageStatType.pierceDamage);
>>>>>>> Stashed changes
        }
    }
}

