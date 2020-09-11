using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public enum SkillType
    {
        PyhsicalAttack,
        Move,
        Fire,
        Water,
        Poison,
        Shock,
        Curse,
        Bless,
        Castable,
        Chill
    }

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
        CastPhysical,
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
        public GameObject skillScriptObject;
        public GameObject effectPrefab;

        public CastableSettings castableSettings;

        public float damage;

        [Header("Attribute Scaling")]
        public AttributeType scaleType;
        public float _scaleValue;

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

        public void OnEnable()
        {
            CollectCharacterModifiers();
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
    }
}

