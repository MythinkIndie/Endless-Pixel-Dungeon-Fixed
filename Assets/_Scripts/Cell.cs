using System;
using UnityEngine;

[Serializable]
public class Enemy {

    public int Attack;
    public int Health;
    public Sprite EnemySprite;
    public int SpecificEnemy;

    public Enemy(int attack, int health, Sprite tile, int specificenemy) {

        Attack = attack;
        Health = health;
        EnemySprite = tile;
        SpecificEnemy = specificenemy;

    }

}

public enum CellType {

    Empty,
    Wall,
    Entrance

}

public enum ItemType {

    None,
    Key,
    Potion,
    BigPotion,
    Coin,
    Sword,
    Chest,
    Exit

}

[Serializable]
public class Cell {
    
    public Vector2Int Pos = Vector2Int.zero;
    public bool isRevelated = false;
    public int Locks = 0;
    public CellType Type = CellType.Empty;
    public ItemType Item = ItemType.None;
    public Enemy Enemy; 

    public Cell () {}

    public Cell (int x, int y) {

        Pos.x = x;
        Pos.y = y;

    }
}
