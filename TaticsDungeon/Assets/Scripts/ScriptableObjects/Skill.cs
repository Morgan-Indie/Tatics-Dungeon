using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public enum CharacterClass
    {
        Warrior,
        Paladin,
        Tinkerer,
        Mage,
        Ranger,
        Rougue,
        None
    }

    public enum CastableSpell
    {
        CastFire,
        CastChill,
        CastWater,
        CastOil,
        CastPoison,
        CastShock,
        FireStorm,
        IceBomb,
        EnergyBall,
    };

    public enum CastableShape
    {
        Single,
        Circular,
        Area,
        Line, 
        Cross,
        Checker,
    }

    [System.Serializable]
    public class CastableSettings
    {
        public CastableSpell castableSpell;
        public int range = 3;
        public CastableShape shape = CastableShape.Single;
        public bool inclusive = true;
        public int radius = 1;
        [ConditionalHide("shape", (int)CastableShape.Line)]
        public int lineOrientation = 0;
        [ConditionalHide("shape", (int)CastableShape.Cross)]
        public int crossOrientation = 0;
    }

    [CreateAssetMenu(fileName ="Skill")]
    [System.Serializable]
    public class Skill : ScriptableObject
    {
        public string skillName;
        public CharacterClass skillClass;
        public Sprite icon;
        public int LevelRequired;
        public int APcost;
        public SkillType type;
        public int coolDown;
        public GameObject effectPrefab;

        [ConditionalHide("type", (int)SkillType.Castable)]
        public CastableSettings castableSettings;

        [Header("Attribute Scaling")]
        public StatModType strScaleType;
        public float _scaleValueStrength;
        public StatModType vitScaleType;
        public float _scaleValueVitality;
        public StatModType dexScaleType;
        public float _scaleValueDexterity;
        public StatModType lucScaleType;
        public float _scaleValueLuck;
        public StatModType intScaleType;
        public float _scaleValueIntelligence;
        public StatModType staScaleType;
        public float _scaleValueStamina;
        public StatModType levScaleType;
        public float _scaleValueLevel;

        [Header("Combat Stat Scaling")]
        public StatModType normalDamageScaleType;
        public float _scaleValueNormalDamage;
        public StatModType armorScaleType;
        public float _scaleValueArmor;
        public StatModType resistanceScaleType;
        public float _scaleValueResistance;

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

        public Dictionary<AttributeType, StatModifier> statModDict = new Dictionary<AttributeType, StatModifier>();
        public Dictionary<AttributeType, StatScalingStruct> attributeScaleModDict = new Dictionary<AttributeType, StatScalingStruct>();
        public Dictionary<CombatStatType, StatScalingStruct> combatStatScaleDict = new Dictionary<CombatStatType, StatScalingStruct>();

        public void OnEnable()
        {
            CollectCharacterModifiers();
            CollectAttributeScalingModifiers();
            CollectCombatStatScalingModifiers();
        }        

        public void CollectCharacterModifiers()
        {
            if (strModValue != 0f)
                statModDict.Add(AttributeType.strength, new StatModifier(strModValue, strModType, this));
            if (dexModValue != 0f)
                statModDict.Add(AttributeType.dexterity, new StatModifier(dexModValue, dexModType, this));
            if (lucModValue != 0f)
                statModDict.Add(AttributeType.luck, new StatModifier(lucModValue, lucModType, this));
            if (vitModValue != 0f)
                statModDict.Add(AttributeType.vitality, new StatModifier(vitModValue, vitModType, this));
            if (staModValue != 0f)
                statModDict.Add(AttributeType.stamina, new StatModifier(staModValue, staModType, this));
            if (intModValue != 0f)
                statModDict.Add(AttributeType.intelligence, new StatModifier(intModValue, intModType, this));
        }

        public void CollectAttributeScalingModifiers()
        {
            if (_scaleValueStrength != 0f)
                attributeScaleModDict.Add(AttributeType.strength, new StatScalingStruct(strScaleType,_scaleValueStrength));
            if (_scaleValueDexterity != 0f)
                attributeScaleModDict.Add(AttributeType.dexterity, new StatScalingStruct(strScaleType, _scaleValueDexterity));
            if (_scaleValueLuck != 0f)
                attributeScaleModDict.Add(AttributeType.luck, new StatScalingStruct(strScaleType, _scaleValueLuck));
            if (_scaleValueVitality != 0f)
                attributeScaleModDict.Add(AttributeType.vitality, new StatScalingStruct(strScaleType, _scaleValueVitality));
            if (_scaleValueStamina != 0f)
                attributeScaleModDict.Add(AttributeType.stamina, new StatScalingStruct(strScaleType, _scaleValueStamina));
            if (_scaleValueIntelligence != 0f)
                attributeScaleModDict.Add(AttributeType.intelligence, new StatScalingStruct(strScaleType, _scaleValueIntelligence));
            if (_scaleValueIntelligence != 0f)
                attributeScaleModDict.Add(AttributeType.intelligence, new StatScalingStruct(strScaleType, _scaleValueIntelligence));
            if (_scaleValueLevel != 0f)
                attributeScaleModDict.Add(AttributeType.level, new StatScalingStruct(levScaleType, _scaleValueLevel));
        }

        public void CollectCombatStatScalingModifiers()
        {
            if (_scaleValueNormalDamage != 0f)
                combatStatScaleDict.Add(CombatStatType.normalDamage, new StatScalingStruct(normalDamageScaleType, _scaleValueNormalDamage));
            if (_scaleValueArmor != 0f)
                combatStatScaleDict.Add(CombatStatType.armor, new StatScalingStruct(armorScaleType, _scaleValueArmor));
            if (_scaleValueResistance != 0f)
                combatStatScaleDict.Add(CombatStatType.resistance, new StatScalingStruct(resistanceScaleType, _scaleValueResistance));
        }
    }
}

