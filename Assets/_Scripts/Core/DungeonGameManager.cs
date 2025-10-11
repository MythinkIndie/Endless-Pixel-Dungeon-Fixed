using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGameManager : MonoBehaviour
{
    public static DungeonGameManager Instance { get; private set; }

    [Header("Managers")]
    public BoardManager boardManager;
    public PlayerManager playerManager;
    public EnemyManager enemyManager;
    public ItemManager itemManager;
    public UIManager uiManager;
    public SkillManager skillManager;

    [Header("Enemy Sprites")]
    public List<Sprite> enemySprites;
    public List<int> EnemiesWithArmor;

    // Game State
    public GameState currentState { get; private set; }
    public int currentFloor { get; private set; }

    // Events
    public System.Action<GameState> OnGameStateChanged;
    public System.Action<int> OnFloorChanged;
    public System.Action OnGameOver;

    [SerializeField] private AudioClip DungeonMusic;
    [SerializeField] private List<AudioClip> AtkSFX;
    [SerializeField] private List<AudioClip> PlayerDieSFX;
    [SerializeField] private AudioClip BlockSFX;

    //Poner to Awake para no tener fallos
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        currentState = GameState.Initializing;
        currentFloor = 0;

        //El audio manager SI que debe de inicializar siempre, dado que se crea en las primeras pantallas
        AudioManager.SharedInstance.PlayMusic(DungeonMusic, true);

        EnemyFactory.Initialize(enemySprites);

        // Initialize all managers
        playerManager.Initialize(UserData.SharedInstance);
        boardManager.Initialize();
        enemyManager.Initialize();
        itemManager.Initialize();
        skillManager.Initialize(UserData.SharedInstance.WeaponEquiped);
        uiManager.Initialize();

        // Subscribe to events
        SubscribeToEvents();

        ChangeState(GameState.GeneratingLevel);
        StartNewLevel();
    }

    private void SubscribeToEvents()
    {
        boardManager.OnCellInteraction += HandleCellRevealed;
        boardManager.OnCellRevealed += HandleScenaryEffects;
        boardManager.NewEnemyRevelated += HandleEnemyAppears;
        playerManager.OnPlayerDeath += HandleGameOver;
        itemManager.OnItemCollected += HandleItemCollected;
        enemyManager.OnEnemyDefeated += HandleEnemyDefeated;
        enemyManager.OnStatusKillEnemy += HandleCombatResult;
    }

    public void StartNewLevel(Vector2Int startPosition = default)
    {
        currentFloor++;
        OnFloorChanged?.Invoke(currentFloor);

        playerManager.RemoveKey();

        // Generate new level
        LevelData levelData = LevelGenerator.GenerateLevel(currentFloor, UserData.SharedInstance, startPosition);

        // Setup enemies
        enemyManager.SpawnEnemies(levelData.enemies);

        // Setup items
        itemManager.SpawnItems(levelData.items);

        // Setup board
        boardManager.LoadLevel(levelData);

        ChangeState(GameState.Playing);
    }

    private void HandleCellRevealed(Cell cell)
    {

        if (cell.Enemy != null)
        {
            // Handle enemy encounter
            //Sistema de combate
            CombatResult result = CombatSystem.ProcessCombat(playerManager.GetPlayer(), cell.Enemy, playerManager.AttackBoost);
            HandleCombatResult(result, cell.Enemy);
        }
        else if (cell.Item != ItemType.None)
        {
            // Handle item pickup
            itemManager.CollectItem(cell);
        }

        HandleScenaryEffects(cell);

        
    }

    private void HandleScenaryEffects(Cell cell)
    {

        //Aqui se pueden actualizar las particulas para clicar
        enemyManager.ApplyStatusEffects();

    }

    private void HandleCombatResult(CombatResult result, Enemy enemy)
    {

        playerManager.TakeDamage(result.playerDamageRecived);
        playerManager.AttackBoost = result.playerBoostForNextAttack;
        AudioManager.SharedInstance.PlaySound(AtkSFX[UnityEngine.Random.Range(0, AtkSFX.Count)]);

        if (result.enemyDefeated)
        {
            enemyManager.RemoveEnemy(enemy);
            enemy = null;
        }
        else
        {
            enemyManager.UpdateEnemyUI(enemy, result.enemyDamageRecived);
        }

        uiManager.UpdateUI();
    }

    private void HandleItemCollected(ItemType itemType, Vector2Int exitPos)
    {
        switch (itemType)
        {
            case ItemType.Key:
                boardManager.UnlockExit();
                break;
            case ItemType.Exit:
                if (playerManager.HasKey())
                {
                    exitPos.y -= 2;
                    StartNewLevel(exitPos);
                }
                break;
        }
    }

    private void HandleEnemyDefeated(Enemy enemy)
    {
        boardManager.UnlockAdjacentCells(enemy.Position);
        boardManager.RemoveEnemyFrom(enemy.Position);
    }

    private void HandleEnemyAppears(Enemy enemy)
    {
        enemyManager.ShowEnemyOnList(enemy);
    }

    private void HandleGameOver()
    {
        AudioManager.SharedInstance.PlaySound(PlayerDieSFX[UnityEngine.Random.Range(0, PlayerDieSFX.Count)]);
        ChangeState(GameState.GameOver);
        OnGameOver?.Invoke();
    }

    public void HandleBlockSound()
    {
        AudioManager.SharedInstance.PlaySound(BlockSFX);
    }

    public void PickUpGold(int amount)
    {
        UserData.SharedInstance.Gold += amount;
        uiManager.UpdateGoldUI(UserData.SharedInstance.Gold);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }
        
}
