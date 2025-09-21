using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelGenerator
{
    public static LevelData GenerateLevel(int floorLevel, UserData playerData, Vector2Int startPosition)
    {
        var levelData = new LevelData
        {
            cells = GenerateCells(floorLevel, startPosition),
            enemies = new List<Enemy>(),
            items = new List<ItemSpawn>()
        };

        // Generate walls
        GenerateWalls(levelData.cells, Random.Range(0, 5));

        // Generate items
        GenerateItems(levelData, floorLevel, playerData);

        // Generate enemies
        GenerateEnemies(levelData, floorLevel);

        return levelData;
    }

    private static List<Cell> GenerateCells(int floorLevel, Vector2Int startPosition)
    {
        var cells = new List<Cell>();
        int boardWidth = 9;
        int boardHeight = 7;

        for (int y = 0; y < boardHeight; y++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                var cell = new Cell(x, y + 2);
                cells.Add(cell);

                // Set entrance at (0, 2) for new levels, or previous exit position
                if (x == startPosition.x && y == startPosition.y)
                {
                    cell.Type = CellType.Entrance;
                }
            }
        }

        return cells;
    }

    private static void GenerateWalls(List<Cell> cells, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var available = cells.FindAll(c => c.Type == CellType.Empty && c.Item == ItemType.None);
            if (available.Count > 0)
            {
                available[Random.Range(0, available.Count)].Type = CellType.Wall;
            }
        }
    }

    private static void GenerateItems(LevelData levelData, int floorLevel, UserData playerData)
    {

        var cells = levelData.cells;

        // Always generate exit and key
        PlaceRandomItem(cells, ItemType.Exit, 1);
        PlaceRandomItem(cells, ItemType.Key, 1);

        // Chest (rare, based on floor and luck)
        int chestChance = 97 - floorLevel; // - (playerData.Luck * 0.2);
        if (Random.Range(0, 100) > chestChance)
        {
            PlaceRandomItem(cells, ItemType.Chest, 1);
        }

        // Potions
        if (Random.Range(0, 100) > 80) // - (playerData.Luck * 2)
        {
            PlaceRandomItem(cells, ItemType.BigPotion, 1);
            PlaceRandomItem(cells, ItemType.Potion, 1);
        }
        else
        {
            PlaceRandomItem(cells, ItemType.Potion, 2);
        }

        // Coins
        PlaceRandomItem(cells, ItemType.Coin, Random.Range(1, 4));

        // Swords (based on luck)
        int swordCount = Random.Range(0, 100) > 100 ? 2 : 1; // - (Mathf.Pow(playerData.Luck, 2f) * 2)
        PlaceRandomItem(cells, ItemType.Sword, swordCount);
    }

    private static void GenerateEnemies(LevelData levelData, int floorLevel)
    {
        var cells = levelData.cells;
        int enemyCount = Random.Range(3, 8);
        
        for (int i = 0; i < enemyCount; i++)
        {
            var available = cells.FindAll(c => c.Type == CellType.Empty && c.Item != ItemType.Exit);
            if (available.Count > 0)
            {
                var cell = available[Random.Range(0, available.Count)];
                
                // Crear enemigo usando factory
                var enemy = EnemyFactory.CreateRandomEnemy(floorLevel);
                enemy.Position = cell.Pos;
                
                cell.Enemy = enemy;
                levelData.enemies.Add(enemy);
            }
        }
    }

    private static void PlaceRandomItem(List<Cell> cells, ItemType itemType, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var available = cells.FindAll(c => c.Type == CellType.Empty && c.Item == ItemType.None);
            if (available.Count > 0)
            {
                var rnd = Random.Range(0, available.Count);
                available[rnd].Item = itemType;
                if (itemType == ItemType.Exit) available[rnd].Type = CellType.Exit;
            }
        }
    }
    
}