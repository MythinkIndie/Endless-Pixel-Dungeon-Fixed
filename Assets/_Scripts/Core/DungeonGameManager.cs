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
    
    // Game State
    public GameState currentState { get; private set; }
    public int currentFloor { get; private set; }

    public UserData playerData;
    
    // Events
    public System.Action<GameState> OnGameStateChanged;
    public System.Action<int> OnFloorChanged;
    public System.Action OnGameOver;
    
    private void Awake()
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

        EnemyFactory.Initialize(enemySprites);
        
        // Initialize all managers
        playerManager.Initialize(playerData);
        boardManager.Initialize();
        enemyManager.Initialize();
        itemManager.Initialize();
        skillManager.Initialize(playerData.WeaponEquiped);
        uiManager.Initialize();
        
        // Subscribe to events
        SubscribeToEvents();
        
        ChangeState(GameState.GeneratingLevel);
        StartNewLevel();
    }
    
    private void SubscribeToEvents()
    {
        boardManager.OnCellRevealed += HandleCellRevealed;
        boardManager.NewEnemyRevelated += HandleEnemyAppears;
        playerManager.OnPlayerDeath += HandleGameOver;
        itemManager.OnItemCollected += HandleItemCollected;
        enemyManager.OnEnemyDefeated += HandleEnemyDefeated;
    }
    
    public void StartNewLevel(Vector2Int startPosition = default)
    {
        currentFloor++;
        OnFloorChanged?.Invoke(currentFloor);

        playerManager.RemoveKey();
        
        // Generate new level
        LevelData levelData = LevelGenerator.GenerateLevel(currentFloor, playerData, startPosition);
             
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
            CombatResult result = CombatSystem.ProcessCombat(playerManager.GetPlayer(), cell.Enemy);
            HandleCombatResult(result, cell);
        }
        else if (cell.Item != ItemType.None)
        {
            // Handle item pickup
            itemManager.CollectItem(cell);
        }
    }
    
    private void HandleCombatResult(CombatResult result, Cell cell) {

        playerManager.TakeDamage(result.playerDamageRecived);

        if (result.enemyDefeated)
        {
            enemyManager.RemoveEnemy(cell.Enemy);
            cell.Enemy = null;
        }
        else
        {
            enemyManager.UpdateEnemyUI(cell, result.enemyDamageRecived);
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
    }

    private void HandleEnemyAppears(Enemy enemy)
    {
        enemyManager.ShowEnemyOnList(enemy);
    }
    
    private void HandleGameOver()
    {
        ChangeState(GameState.GameOver);
        OnGameOver?.Invoke();
    }

    public void PickUpGold(int amount)
    {
        playerData.Gold += amount;
        uiManager.UpdateGoldUI(playerData.Gold);
    }
    
    public void ChangeState(GameState newState)
    {
        currentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }
}
