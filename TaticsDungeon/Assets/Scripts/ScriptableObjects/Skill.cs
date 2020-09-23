using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public enum SkillType
    {
        CastPyhsical,
        CastHeat,
        CastHealing,
        CastSubstance,
        Move,
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

    public enum CastType
    {
        Free,
        Pinned,
    }

    [System.Serializable]
    public class CastableSettings
    {
        public int range = 3;
        public CastableShape shape = CastableShape.Single;
        public bool inclusive = true;
        public int radius = 1;
        [ConditionalHide("shape", (int)CastableShape.Line)]
        public int lineOrientation = 0;
        [ConditionalHide("shape", (int)CastableShape.Cross)]
        public int crossOrientation = 0;
    }

    [System.Serializable]
    public class PinnedSettings
    {
        public PinnedShape shape = PinnedShape.Single;
        public int radius = 1;
        public bool inclusive = false;
        [ConditionalHide("shape", (int)PinnedShape.Cone)]
        public PinnedConeSettings coneSettings;
    }

    [System.Serializable]
    public class PinnedConeSettings
    {
        [Header("Cone Expansion Rate")]
        public int expansion = 1;
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
        public CastType castType;
        public int coolDown;

        public GameObject skillScriptObject;
        public GameObject effectPrefab;

        [ConditionalHide("castType", (int)CastType.Free)]
        public CastableSettings castableSettings;
        [ConditionalHide("castType", (int)CastType.Pinned)]
        public PinnedSettings pinnedSettings;

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

