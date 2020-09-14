using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

namespace PrototypeGame
{
    public enum GameState
    {
        Ready,InMenu,ResolvingInteraction
    }
    public class GameManager : MonoBehaviour
    {
        public GameState gameState;
        public bool isPlayerTurn;

        [HideInInspector]
        public static GameManager instance = null;
        public Dictionary<string, PlayerManager> playersDict = new Dictionary<string, PlayerManager>();
        public Dictionary<string, EnemyManager> enemiesDict = new Dictionary<string, EnemyManager>();
        public CharacterStatusLayout characterStatusLayout;
        public Camera UIcam;
        public Light UIcamLight;
        public AlchemyManager alchemyManager;

        public PlayerManager currentCharacter;
        public EnemyManager currentEnemy;
        public int playerIndex=0;
        public int enemyIndex=0;
        public int Turn = 1;

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

            currentCharacter = playersDict.Values.ToArray()[0];
            currentCharacter.isCurrentPlayer = true;
            isPlayerTurn = true;

            currentEnemy = enemiesDict.Values.ToArray()[0];            
            currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 1);
            SetUICam();            
        }

        public void Start()
        {            
            foreach (PlayerManager player in playersDict.Values.ToArray())
            {
                player.skillSlotsHandler.skillPanel.SetActive(false);
                player.taticalMovement.SetCurrentCell();
            }
            foreach (EnemyManager enemy in enemiesDict.Values.ToArray())
                enemy.taticalMovement.SetCurrentCell();

            InitalizePlayerTurn();
            popUpUI.Activate();

            foreach (var player in playersDict)
                characterStatusLayout.AddPlayerStatusPanel(player.Value);
            foreach (var enemy in enemiesDict)
                characterStatusLayout.AddEnemyStatusPanel(enemy.Value);
            gameState = GameState.Ready;
        }

        public void Update()
        {
            float delta = Time.deltaTime;
            CheckGameState();
            InputHandler.instance.TickInput(delta);
            CameraHandler.instance.HandleCamera(delta);

            if (isPlayerTurn)
            {
                if (gameState == GameState.InMenu)
                {
                    UIcam.enabled = true;
                    UIcamLight.enabled = true;
                    UIcam.GetComponent<UICam>().HandleUICam();
                }


                if (gameState != GameState.ResolvingInteraction)
                    CharacterSwitch();
                currentCharacter.PlayerUpdate(delta);
            }
                
            else
            {
                if (CheckEnemyEndTurn())
                    SwitchTurns();
                else
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
            foreach (PlayerManager player in playersDict.Values.ToArray())
            {
                // must recover AP first before setting navDict
                player.characterStats.currentAP= player.characterStats.maxAP;
                player.characterStats.apBar.RecoverAPUI();
            }
            currentCharacter = playersDict.Values.ToArray()[0];
            currentCharacter.isCurrentPlayer = true;
            currentCharacter.skillSlotsHandler.skillPanel.SetActive(true);
            currentCharacter.taticalMovement.SetCurrentNavDict();
            isPlayerTurn = true;

            //Debug.Log("Player Turn Intialized");
        }

        public void InitalizeEnemyTurn()
        {
            foreach (EnemyManager enemy in enemiesDict.Values.ToArray())
            {
                // must recover AP first before setting navDict
                enemy.characterStats.currentAP= enemy.characterStats.maxAP;
                enemy.characterStats.apBar.RecoverAPUI();
            }
            currentEnemy = enemiesDict.Values.ToArray()[0];
            currentEnemy.isCurrentEnemy = true;
            currentEnemy.taticalMovement.SetCurrentNavDict();
            isPlayerTurn = false;
            //Debug.Log("Enemy Turn Intialized");
        }

        #region Player Selecting
        public void SetNextPlayer()
        {            
            currentCharacter.isCurrentPlayer = false;
            if (gameState == GameState.InMenu)
            {
                currentCharacter.inventoryHandler.inventoryUI.SetActive(false);
                currentCharacter.stateManager.characterState = CharacterState.Ready;
            }
            currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 0);
            currentCharacter.skillSlotsHandler.skillPanel.SetActive(false);
            playerIndex++;

            if (playerIndex < 0)
                playerIndex = playersDict.Values.ToArray().Length-1;
            else if (playerIndex >= playersDict.Values.ToArray().Length)
                playerIndex = 0;

            currentCharacter = playersDict.Values.ToArray()[playerIndex];
            currentCharacter.isCurrentPlayer = true;
            currentCharacter.skillSlotsHandler.skillPanel.SetActive(true);
            currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 1);
            currentCharacter.taticalMovement.SetCurrentNavDict();

            if (gameState == GameState.InMenu)
            {
                currentCharacter.inventoryHandler.inventoryUI.SetActive(true);
                currentCharacter.stateManager.characterState = CharacterState.InMenu;
                currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 0);
            }
        }

        public void SetPreviousPlayer()
        {
            currentCharacter.skillSlotsHandler.skillPanel.SetActive(false);
            currentCharacter.isCurrentPlayer = false;
            if (gameState == GameState.InMenu)
            {
                currentCharacter.inventoryHandler.inventoryUI.SetActive(false);
                currentCharacter.stateManager.characterState = CharacterState.Ready;
            }
            currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 0);

            playerIndex--;

            if (playerIndex < 0)
                playerIndex = playersDict.Values.ToArray().Length-1;
            else if (playerIndex >= playersDict.Values.ToArray().Length)
                playerIndex = 0;

            currentCharacter = playersDict.Values.ToArray()[playerIndex];
            currentCharacter.isCurrentPlayer = true;
            currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 1);
            currentCharacter.taticalMovement.SetCurrentNavDict();
            currentCharacter.skillSlotsHandler.skillPanel.SetActive(true);
            if (gameState == GameState.InMenu)
            {
                currentCharacter.inventoryHandler.inventoryUI.SetActive(true);
                currentCharacter.stateManager.characterState = CharacterState.InMenu;
                currentCharacter.playerModel.GetComponent<Renderer>().material.SetFloat("OnOff", 0);
            }
        }

        #endregion

        #region Enemy Selection

        public void SetNextEnemy()
        {            
            if (enemyIndex < 0)
                enemyIndex = enemiesDict.Values.ToArray().Length - 1;
            else if (enemyIndex >= enemiesDict.Values.ToArray().Length)
                enemyIndex = 0;
            currentEnemy.isCurrentEnemy = false;
            currentEnemy = enemiesDict.Values.ToArray()[enemyIndex];
            currentEnemy.isCurrentEnemy = true;
            currentEnemy.taticalMovement.SetCurrentNavDict();
            enemyIndex++;
        }

        #endregion


        public bool CheckEnemyEndTurn()
        {
            int totalAp = 0;
            foreach (EnemyManager enemy in enemiesDict.Values.ToArray())
                totalAp += enemy.characterStats.currentAP;
            if (totalAp == 0 && gameState == GameState.Ready)
                return true;
            return false;
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
                foreach (PlayerManager player in playersDict.Values.ToArray())
                {
                    player.isCurrentPlayer = false;
                    player.stateManager.UpdateTurns();
                }
                currentCharacter.skillSlotsHandler.skillPanel.SetActive(false);
                GridManager.Instance.RemoveAllHighlights();
                InitalizeEnemyTurn();                
            }
            else
            {
                foreach (EnemyManager enemy in enemiesDict.Values.ToArray())
                {
                    enemy.isCurrentEnemy = false;
                    enemy.stateManager.UpdateTurns();
                }

                Turn++;
                InitalizePlayerTurn();
            }
        }

        public void SwitchSkill(SkillAbstract skillScript)
        {
            if (currentCharacter.stateManager.characterState!= CharacterState.IsInteracting)
            {
                currentCharacter.selectedSkill = skillScript;
                if (skillScript == null||skillScript.skill.type == SkillType.Move)
                    GridManager.Instance.HighlightNavDict(currentCharacter.taticalMovement.currentNavDict);
                else
                    GridManager.Instance.RemoveAllHighlights();
            }
        }

        public void CheckGameState()
        {
            foreach (PlayerManager player in playersDict.Values.ToArray())
            {
                if (player.stateManager.characterState == CharacterState.InMenu)
                {
                    gameState = GameState.InMenu;
                    return;
                }

                if (player.stateManager.characterState==CharacterState.IsInteracting)
                {
                    gameState = GameState.ResolvingInteraction;
                    return;
                }
            }

            foreach (EnemyManager enemy in enemiesDict.Values.ToArray())
            {
                if (enemy.stateManager.characterState != CharacterState.Ready)
                {
                    gameState = GameState.ResolvingInteraction;
                    return;
                }
            }
            
            gameState = GameState.Ready;
        }
    }
}

