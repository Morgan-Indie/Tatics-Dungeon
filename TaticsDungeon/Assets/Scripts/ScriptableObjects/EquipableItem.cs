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
        public EquipmentType equipmentType;
        public int range=1;

        public List<meshLocation> itemMeshLocs;
        public List<Mesh> itemMeshesMale;
        public List<Mesh> itemMeshesFemale;

        [Header("Stat Scaling")]
        public float _scaleValueStrength;
        public float _scaleValueVitality;
        public float _scaleValueDexterity;
        public float _scaleValueLuck;
        public float _scaleValueIntelligence;
        public float _scaleValueStamina;

        [Header("Damage Types")]
        public float _normalDamage = 0f;
        public float _pierceDamage = 0f;
        public float _poisonDamage = 0f;
        public float _fireDamage = 0f;
        public float _waterDamage = 0f;
        public float _curseDamage = 0f;
        public float _shockDamage = 0f;

        [Header("Resistances")]
        public float _armor;
        public float _fireResistance;
        public float _waterResistance;
        public float _shockResistance;
        public float _poisonResistance;

        [Header("StatModifiers")]
        public float strModValue = 0f;
        public StatModType strModType;
        public float dexModValue = 0f;
        public StatModType dexModType;
        public float lucModValue = 0f;
        public StatModType lucModType;
        public float intModValue = 0f;
        public StatModType intModType;
        public float staModValue = 0f;
        public StatModType staModType;
        public float vitModValue = 0f;
        public StatModType vitModType;

        public ItemEffect itemEffect;
        public Dictionary<AttributeType, StatModifier> statModDict = new Dictionary<AttributeType, StatModifier>();
        public Dictionary<AttributeType, float> scaleModDict = new Dictionary<AttributeType, float>();
        public Dictionary<CombatStatType, StatModifier> equipmentStatDict = new Dictionary<CombatStatType, StatModifier>();

        public void Init()
        {
            if (statModDict.Count ==0)
                CollectCharacterModifiers();
            if (scaleModDict.Count==0)
                CollectScalingModifiers();
            if (equipmentStatDict.Count==0)
                CollectEquipmentStats();
        }

        public void CollectCharacterModifiers()
        {
            if (strModValue!=0f)
                statModDict.Add(AttributeType.strength, new StatModifier(strModValue, strModType, this));
            if (dexModValue!=0f)
                statModDict.Add(AttributeType.dexterity, new StatModifier(dexModValue, dexModType, this));
            if (lucModValue!=0f)
                statModDict.Add(AttributeType.luck, new StatModifier(lucModValue, lucModType, this));
            if (vitModValue!=0f)
                statModDict.Add(AttributeType.vitality, new StatModifier(vitModValue, vitModType, this));
            if (staModValue!=0f)
                statModDict.Add(AttributeType.stamina, new StatModifier(staModValue, staModType, this));
            if (intModValue!=0f)
                statModDict.Add(AttributeType.intelligence, new StatModifier(intModValue, intModType, this));
        }

        public void CollectScalingModifiers()
        {
            if (_scaleValueStrength != 0f)
                scaleModDict.Add(AttributeType.strength, _scaleValueStrength);
            if (_scaleValueDexterity != 0f)
                scaleModDict.Add(AttributeType.dexterity, _scaleValueDexterity);
            if (_scaleValueLuck != 0f)
                scaleModDict.Add(AttributeType.luck, _scaleValueLuck);
            if (_scaleValueVitality != 0f)
                scaleModDict.Add(AttributeType.vitality, _scaleValueVitality);
            if (_scaleValueStamina != 0f)
                scaleModDict.Add(AttributeType.stamina, _scaleValueStamina);
            if (_scaleValueIntelligence != 0f)
                scaleModDict.Add(AttributeType.intelligence, _scaleValueIntelligence);
        }

        public void CollectEquipmentStats()
        {
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
        }
    }
}

