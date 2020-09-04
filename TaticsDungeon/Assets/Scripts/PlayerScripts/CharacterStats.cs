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
        public CombatStat resistance;

        [Header("Not Required")]
        public HealthBar healthBar;
        public APFill apBar;
        public GameObject statusPanel;
        public Text panelName;

        [HideInInspector]
        public Dictionary<AttributeType, Stat> playerAttributeDict= new Dictionary<AttributeType, Stat>();
        public Dictionary<CombatStatType, Stat> playerCombatStatDict = new Dictionary<CombatStatType, Stat>();

        [HideInInspector]
        public CharacterStateManager stateManager;
        public AnimationHandler animationHandler;

        private void Awake()
        {
            CollectPlayerAttributes();
            CollectPlayerCombatStats();
        }

        // Start is called before the first frame update
        void Start()
        {
            //statusPanel = Instantiate(GameManager.instance.playerStatusPrefab);
            healthBar = statusPanel.GetComponentInChildren<HealthBar>();
            apBar = statusPanel.GetComponentInChildren<APFill>();
            panelName = statusPanel.GetComponentInChildren<Text>();
            panelName.text = characterName;

            maxHealth = SetMaxHealthFromVitality();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            maxAP = SetMaxAPFromStamina();
            currentAP = maxAP;
            apBar.SetMaxAP(maxAP);

            stateManager = GetComponent<CharacterStateManager>();
            animationHandler = GetComponent<AnimationHandler>();
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
            playerAttributeDict.Add(AttributeType.dexterity, Vitality);
            playerAttributeDict.Add(AttributeType.luck, Stamina);
            playerAttributeDict.Add(AttributeType.vitality, Luck);
            playerAttributeDict.Add(AttributeType.stamina, Intelligence);
            playerAttributeDict.Add(AttributeType.intelligence, Dexterity);
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
            resistance = new CombatStat(0, CombatStatType.resistance);

            playerCombatStatDict.Add(CombatStatType.normalDamage, normalDamage);
            playerCombatStatDict.Add(CombatStatType.poisonDamage, poisonDamage);
            playerCombatStatDict.Add(CombatStatType.pierceDamage, pierceDamage);
            playerCombatStatDict.Add(CombatStatType.waterDamage, waterDamage);
            playerCombatStatDict.Add(CombatStatType.shockDamage, shockDamage);
            playerCombatStatDict.Add(CombatStatType.fireDamage, fireDamage);
            playerCombatStatDict.Add(CombatStatType.armor, armor);
            playerCombatStatDict.Add(CombatStatType.resistance, resistance);
            playerCombatStatDict.Add(CombatStatType.curseDamage, curseDamage);
        }

        private int SetMaxHealthFromVitality()
        {
            maxHealth = (int)Vitality.Value * 10;
            return maxHealth;
        }

        public int SetMaxAPFromStamina()
        {
            maxAP = Mathf.FloorToInt(Stamina.Value);
            return maxAP;
        }

        public void TakeDamage(int damange)
        {
            currentHealth -= damange;
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                stateManager.characterState = CharacterState.Dead;
<<<<<<< Updated upstream
=======
                taticalMovement.currentCell.occupyingObject = null;
                taticalMovement.currentCell.state = CellState.open;
>>>>>>> Stashed changes
                animationHandler.PlayTargetAnimation("Death");
            }

            else if (stateManager.characterAction == CharacterAction.LyingDown)
                animationHandler.PlayTargetAnimation("LyingHitReaction");
            else
                animationHandler.PlayTargetAnimation("MinorHitReaction");
        }

        public void UseAP(int AP)
        {
            currentAP -= AP;
            apBar.SetCurrentAP(currentAP);
        }
    }
}

