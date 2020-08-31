using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

namespace PrototypeGame
{
    public class GameManager : MonoBehaviour
    {
        public string playerState = "ready";
        public bool CombatMode = false;
        public bool isPlayerTurn;

        [HideInInspector]
        public static GameManager instance = null;
        public Dictionary<string, PlayerManager> playersDict = new Dictionary<string, PlayerManager>();
        public Dictionary<string, EnemyManager> enemiesDict = new Dictionary<string, EnemyManager>();
        public PlayerManager[] playersList;
        public EnemyManager[] enemiesList;
        public CharacterStatusLayout characterStatusLayout;
        public CameraModeSwitch cameraModeSwitch;
        public Camera UIcam;
        public Light UIcamLight;
        public AlchemyManager alchemyManager;

        public PlayerManager currentCharacter;
        public EnemyManager currentEnemy;
        public int playerIndex=0;
        public int enemyIndex=0;

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
            cameraModeSwitch = GetComponent<CameraModeSwitch>();
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
            
            currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 1);
            CameraHandler.instance.playerTransform = currentCharacter.transform;
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
        }

        public void LateUpdate()
        {
            if (isPlayerTurn)
            {
                CharacterSwitch();
                if (playerState == "inMenu")
                {
                    UIcam.enabled = true;
                    UIcamLight.enabled = true;
                    UIcam.GetComponent<UICam>().HandleUICam();
                }
            }
            else
            {
                CheckEnemyEndTurn();
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

            currentCharacter = playersList[0];
            currentCharacter.isCurrentPlayer = true;
            currentCharacter.skillSlotsHandler.skillPanel.SetActive(true);
            currentCharacter.taticalMovement.SetCurrentNavDict();
            currentCharacter.playerMovement.characterRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
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

            currentEnemy = enemiesList[0];
            currentEnemy.isCurrentEnemy = true;
            currentEnemy.taticalMovement.SetCurrentNavDict();
            isPlayerTurn = false;

            Debug.Log("Enemy Turn Intialized");
        }

        #region Player Selecting
        public void SetNextPlayer()
        {            
            currentCharacter.isCurrentPlayer = false;
            if (playerState == "inMenu")
                currentCharacter.inventoryHandler.inventoryUI.SetActive(false);
            currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 0);
            currentCharacter.skillSlotsHandler.skillPanel.SetActive(false);
            currentCharacter.playerMovement.characterRigidbody.constraints = RigidbodyConstraints.FreezeAll;
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

            currentCharacter.playerMovement.characterRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            if (playerState == "inMenu")
                currentCharacter.inventoryHandler.inventoryUI.SetActive(true);
        }

        public void SetPreviousPlayer()
        {
            currentCharacter.skillSlotsHandler.skillPanel.SetActive(false);
            currentCharacter.isCurrentPlayer = false;
            currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 0);
            currentCharacter.playerMovement.characterRigidbody.constraints = RigidbodyConstraints.FreezeAll;

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
            currentCharacter.playerMovement.characterRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            if (playerState == "inMenu")
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
            currentEnemy.playerMovement.characterRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            currentEnemy.isCurrentEnemy = false;
            currentEnemy = enemiesList[enemyIndex];
            currentEnemy.isCurrentEnemy = true;
            currentEnemy.playerMovement.characterRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            enemyIndex++;
        }

        #endregion


        public void EnterCombatMode()
        {
            cameraModeSwitch.isometricCamera.GetComponent<Camera>().enabled = true;
            CombatMode = true;
            popUpUI.Activate();

            foreach (var player in playersDict)
                characterStatusLayout.AddPlayerStatusPanel(player.Value);
            foreach (var enemy in enemiesDict)
                characterStatusLayout.AddEnemyStatusPanel(enemy.Value);
            Debug.Log("Entered Tactics Mode");
          }

        public void ExitCombatMode()
        {
            CombatMode = false;
            cameraModeSwitch.isometricCamera.GetComponent<Camera>().enabled = false;
            Debug.Log("Exited Tactics Mode");
        }

        public void CheckEnemyEndTurn()
        {
            int totalAp = 0;
            foreach (EnemyManager enemy in enemiesList)
                totalAp += enemy.characterStats.currentAP;
            if (totalAp == 0 && currentEnemy.stateManager.characterState!="isInteracting")
                SwitchTurns();
        }

        public void CharacterSwitch()
        {
            if (InputHandler.instance.characterSelectInputNext)
            {
                SetNextPlayer();
                InputHandler.instance.characterSelectInputNext = false;
                IsometricCamera.instance.FocusOnCurrentPlayer();
            }
            else if (InputHandler.instance.characterSelectInputPrevious)
            {
                SetPreviousPlayer();
                InputHandler.instance.characterSelectInputPrevious = false;
                IsometricCamera.instance.FocusOnCurrentPlayer();
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
                //IsometricCamera.instance.FocusOnCurrentEnemy();                
            }
            else
            {
                foreach (EnemyManager enemy in enemiesList)
                    enemy.isCurrentEnemy = false;
                IsometricCamera.instance.FocusOnCurrentPlayer();                
                InitalizePlayerTurn();
            }
        }

        public void SwitchSkill(Skill s)
        {
            if (currentCharacter.stateManager.characterState!="isInteracting")
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

