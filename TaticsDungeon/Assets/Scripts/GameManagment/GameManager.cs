using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

namespace PrototypeGame
{
    public class GameManager : MonoBehaviour
    {
        public CharacterState playerState = CharacterState.Ready;
        public bool isPlayerTurn;

        [HideInInspector]
        public static GameManager instance = null;
        public Dictionary<string, PlayerManager> playersDict = new Dictionary<string, PlayerManager>();
        public Dictionary<string, EnemyManager> enemiesDict = new Dictionary<string, EnemyManager>();
        public PlayerManager[] playersList;
        public EnemyManager[] enemiesList;
        public CharacterStatusLayout characterStatusLayout;
        public Camera UIcam;
        public Light UIcamLight;
        public AlchemyManager alchemyManager;

        public PlayerManager currentCharacter;
        public EnemyManager currentEnemy;
        public int playerIndex=0;
        public int enemyIndex=0;
        protected int Turn = 1;

        [Header("Required")]
        public TurnPopUpFade popUpUI;
        public EndTurn endTurn;
        public GameObject playerStatusPrefab;
        public GameObject enemyStatusPrefab;
        public GameObject inventoryUIPrefab;        

        // Start is called before the first frame update
        void Awake()
        {
            if (instance == null)
                instance = this;

            characterStatusLayout = GetComponent<CharacterStatusLayout>();
            alchemyManager = GetComponent<AlchemyManager>();

            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                playersDict.Add(player.GetComponent<CharacterStats>().characterName, player.GetComponent<PlayerManager>());
                player.GetComponent<CharacterStats>().statusPanel = Instantiate(playerStatusPrefab);
            }

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                enemiesDict.Add(enemy.GetComponent<CharacterStats>().characterName, enemy.GetComponent<EnemyManager>());
                enemy.GetComponent<CharacterStats>().statusPanel = Instantiate(enemyStatusPrefab);
            }

            enemiesList = enemiesDict.Values.ToArray();
            playersList = playersDict.Values.ToArray();

            currentCharacter = playersList[0];
            currentCharacter.isCurrentPlayer = true;
            isPlayerTurn = true;

            currentEnemy = enemiesList[0];            
            currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 1);
            SetUICam();            
        }

        public void Start()
        {            
            foreach (PlayerManager player in playersList)
            {
                player.skillSlotsHandler.skillPanel.SetActive(false);
                player.taticalMovement.SetCurrentCell();
            }
            foreach (EnemyManager enemy in enemiesList)
                enemy.taticalMovement.SetCurrentCell();

            InitalizePlayerTurn();
            popUpUI.Activate();

            foreach (var player in playersDict)
                characterStatusLayout.AddPlayerStatusPanel(player.Value);
            foreach (var enemy in enemiesDict)
                characterStatusLayout.AddEnemyStatusPanel(enemy.Value);
        }

        public void Update()
        {
            float delta = Time.deltaTime;

            if (isPlayerTurn)
            {
                currentCharacter.PlayerUpdate(delta);
                if (currentCharacter.stateManager.characterState!=CharacterState.IsInteracting)
                    CharacterSwitch();
                if (playerState == CharacterState.InMenu)
                {
                    UIcam.enabled = true;
                    UIcamLight.enabled = true;
                    UIcam.GetComponent<UICam>().HandleUICam();
                }                
            }
                
            else
            {
                CheckEnemyEndTurn();
                currentEnemy.EnemyUpdate(delta);                
            }
        }

        public void SetUICam()
        {
            UIcam = Camera.main.GetComponentsInChildren<Camera>()[1];
            UIcamLight = UIcam.GetComponentInChildren<Light>();
            UIcam.enabled = false;
            UIcamLight.enabled = false;
        }

        public void InitalizePlayerTurn()
        {
            foreach (PlayerManager player in playersList)
            {
                // must recover AP first before setting navDict
                player.characterStats.currentAP= player.characterStats.maxAP;
                player.characterStats.apBar.RecoverAP();
            }
            currentEnemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            currentCharacter = playersList[0];
            currentCharacter.isCurrentPlayer = true;
            currentCharacter.skillSlotsHandler.skillPanel.SetActive(true);
            currentCharacter.taticalMovement.SetCurrentNavDict();
            currentCharacter.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            isPlayerTurn = true;

            Debug.Log("Player Turn Intialized");
        }

        public void InitalizeEnemyTurn()
        {
            foreach (EnemyManager enemy in enemiesList)
            {
                // must recover AP first before setting navDict
                enemy.characterStats.currentAP= enemy.characterStats.maxAP;
                enemy.characterStats.apBar.RecoverAP();
            }
            currentCharacter.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            currentEnemy = enemiesList[0];
            currentEnemy.isCurrentEnemy = true;
            currentEnemy.taticalMovement.SetCurrentNavDict();
            isPlayerTurn = false;
            currentEnemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            Debug.Log("Enemy Turn Intialized");
        }

        #region Player Selecting
        public void SetNextPlayer()
        {            
            currentCharacter.isCurrentPlayer = false;
            if (playerState == CharacterState.InMenu)
                currentCharacter.inventoryHandler.inventoryUI.SetActive(false);
            currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 0);
            currentCharacter.skillSlotsHandler.skillPanel.SetActive(false);
            currentCharacter.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            playerIndex++;

            if (playerIndex < 0)
                playerIndex = playersList.Length-1;
            else if (playerIndex >= playersList.Length)
                playerIndex = 0;

            currentCharacter = playersList[playerIndex];
            currentCharacter.isCurrentPlayer = true;
            currentCharacter.skillSlotsHandler.skillPanel.SetActive(true);
            currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 1);
            currentCharacter.taticalMovement.SetCurrentNavDict();

            currentCharacter.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            if (playerState == CharacterState.InMenu)
                currentCharacter.inventoryHandler.inventoryUI.SetActive(true);
        }

        public void SetPreviousPlayer()
        {
            currentCharacter.skillSlotsHandler.skillPanel.SetActive(false);
            currentCharacter.isCurrentPlayer = false;
            currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 0);
            currentCharacter.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            playerIndex--;

            if (playerIndex < 0)
                playerIndex = playersList.Length-1;
            else if (playerIndex >= playersList.Length)
                playerIndex = 0;

            currentCharacter = playersList[playerIndex];
            currentCharacter.isCurrentPlayer = true;
            currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 1);
            currentCharacter.taticalMovement.SetCurrentNavDict();
            currentCharacter.skillSlotsHandler.skillPanel.SetActive(true);
            currentCharacter.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            if (playerState == CharacterState.InMenu)
                currentCharacter.inventoryHandler.inventoryUI.SetActive(true);
        }

        #endregion

        #region Enemy Selection

        public void SetNextEnemy()
        {
            if (enemyIndex < 0)
                enemyIndex = enemiesList.Length - 1;
            else if (enemyIndex >= enemiesList.Length)
                enemyIndex = 0;
            currentEnemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            currentEnemy.isCurrentEnemy = false;
            currentEnemy = enemiesList[enemyIndex];
            currentEnemy.isCurrentEnemy = true;
            currentEnemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            enemyIndex++;
        }

        #endregion


        public void CheckEnemyEndTurn()
        {
            int totalAp = 0;
            foreach (EnemyManager enemy in enemiesList)
                totalAp += enemy.characterStats.currentAP;
            if (totalAp == 0 && currentEnemy.stateManager.characterState!=CharacterState.IsInteracting)
                SwitchTurns();
        }

        public void CharacterSwitch()
        {
            if (InputHandler.instance.characterSelectInputNext)
            {
                SetNextPlayer();
                InputHandler.instance.characterSelectInputNext = false;
            }
            else if (InputHandler.instance.characterSelectInputPrevious)
            {
                SetPreviousPlayer();
                InputHandler.instance.characterSelectInputPrevious = false;
            }
        }

        public void SwitchTurns()
        {
            popUpUI.Activate();
            if (isPlayerTurn)
            {
                foreach (PlayerManager player in playersList)
                {
                    player.isCurrentPlayer = false;
                }
                currentCharacter.skillSlotsHandler.skillPanel.SetActive(false);
                GridManager.Instance.RemoveAllHighlights();
                InitalizeEnemyTurn();                
            }
            else
            {
                foreach (EnemyManager enemy in enemiesList)
                    enemy.isCurrentEnemy = false;
                Turn++;
                InitalizePlayerTurn();
            }
        }

        public void SwitchSkill(Skill s)
        {
            if (currentCharacter.stateManager.characterState!= CharacterState.IsInteracting)
            {
                currentCharacter.selectedSkill = s;
                if (s == null || s.type == SkillType.Move)
                    GridManager.Instance.HighlightNavDict(currentCharacter.taticalMovement.currentNavDict);
                else
                    GridManager.Instance.RemoveAllHighlights();
            }
        }
    }
}

