using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypeGame
{
    public class CharacterStats : MonoBehaviour
    {
        [Header("Name")]
        public string characterName;

        [Header("Player Stats")]
        public float _strength=10;
        public float _vitality=10;
        public float _dexterity=10;
        public float _intelligence=10;
        public float _luck=10;
        public float _stamina = 10;

        [Header("For View Only")]
        public int maxHealth;
        public int currentHealth;
        public int maxAP;
        public int currentAP;

        public Stat Strength;
        public Stat Vitality;
        public Stat Dexterity;
        public Stat Luck;
        public Stat Intelligence;
        public Stat Stamina;

        public CombatStat normalDamage;
        public CombatStat pierceDamage;
        public CombatStat poisonDamage;
        public CombatStat fireDamage;
        public CombatStat waterDamage;
        public CombatStat curseDamage;
        public CombatStat shockDamage;
        public CombatStat armor;
        public CombatStat fireResistance;
        public CombatStat waterResistance;
        public CombatStat shockResistance;
        public CombatStat poisonResistance;

        public CombatStatType currentAlchemcialType = CombatStatType.normalDamage;

        [Header("Not Required")]
        public HealthBar healthBar;
        public APFill apBar;
        public GameObject statusPanel;
        public Text panelName;

        [HideInInspector]
        public Dictionary<AttributeType, Stat> playerAttributeDict= new Dictionary<AttributeType, Stat>();
        public Dictionary<CombatStatType, Stat> playerCombatStatDict = new Dictionary<CombatStatType, Stat>();
        public Dictionary<CombatStatType, Stat> playerResistanceStatDict = new Dictionary<CombatStatType, Stat>();

        [HideInInspector]
        public CharacterStateManager stateManager;
        public AnimationHandler animationHandler;
        public TaticalMovement taticalMovement;

        private void Awake()
        {
            CollectPlayerAttributes();
            CollectPlayerCombatStats();
        }

        // Start is called before the first frame update
        void Start()
        {
            healthBar = statusPanel.GetComponentInChildren<HealthBar>();
            apBar = statusPanel.GetComponentInChildren<APFill>();
            panelName = statusPanel.GetComponentInChildren<Text>();
            panelName.text = characterName;

            stateManager = GetComponent<CharacterStateManager>();
            animationHandler = GetComponent<AnimationHandler>();
            taticalMovement = GetComponent<TaticalMovement>();
        }

        public void CollectPlayerAttributes()
        {
            Strength = new Stat(_strength);
            Vitality = new Stat(_vitality);
            Stamina = new Stat(_stamina);
            Luck = new Stat(_luck);
            Intelligence = new Stat(_intelligence);
            Dexterity = new Stat(_dexterity);

            playerAttributeDict.Add(AttributeType.strength, Strength);
            playerAttributeDict.Add(AttributeType.dexterity, Dexterity);
            playerAttributeDict.Add(AttributeType.luck, Luck);
            playerAttributeDict.Add(AttributeType.vitality, Vitality);
            playerAttributeDict.Add(AttributeType.stamina, Stamina);
            playerAttributeDict.Add(AttributeType.intelligence, Intelligence);
        }

        public void CollectPlayerCombatStats()
        {
            normalDamage = new CombatStat(0, CombatStatType.normalDamage);
            pierceDamage = new CombatStat(0, CombatStatType.pierceDamage);
            poisonDamage = new CombatStat(0, CombatStatType.poisonDamage);
            fireDamage = new CombatStat(0, CombatStatType.fireDamage);
            waterDamage = new CombatStat(0, CombatStatType.waterDamage);
            curseDamage = new CombatStat(0, CombatStatType.curseDamage);
            shockDamage = new CombatStat(0, CombatStatType.shockDamage);
            armor = new CombatStat(0, CombatStatType.armor);
            fireResistance = new CombatStat(0, CombatStatType.fireResistance);
            waterResistance = new CombatStat(0, CombatStatType.waterResistance);
            shockResistance = new CombatStat(0, CombatStatType.shockResistance);
            poisonResistance = new CombatStat(0, CombatStatType.poisonResistance);

            playerCombatStatDict.Add(CombatStatType.normalDamage, normalDamage);
            playerCombatStatDict.Add(CombatStatType.poisonDamage, poisonDamage);
            playerCombatStatDict.Add(CombatStatType.pierceDamage, pierceDamage);
            playerCombatStatDict.Add(CombatStatType.waterDamage, waterDamage);
            playerCombatStatDict.Add(CombatStatType.shockDamage, shockDamage);
            playerCombatStatDict.Add(CombatStatType.fireDamage, fireDamage);

            playerCombatStatDict.Add(CombatStatType.armor, armor);
            playerCombatStatDict.Add(CombatStatType.fireResistance, fireResistance);
            playerCombatStatDict.Add(CombatStatType.waterResistance, waterResistance);
            playerCombatStatDict.Add(CombatStatType.shockResistance, shockResistance);
            playerCombatStatDict.Add(CombatStatType.poisonResistance, poisonResistance);
            playerCombatStatDict.Add(CombatStatType.curseDamage, curseDamage);

            playerResistanceStatDict.Add(CombatStatType.waterDamage, waterResistance);
            playerResistanceStatDict.Add(CombatStatType.shockDamage, shockResistance);
            playerResistanceStatDict.Add(CombatStatType.fireDamage, fireResistance);
            playerResistanceStatDict.Add(CombatStatType.poisonDamage, poisonResistance);
        }
    }
}

