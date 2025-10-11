using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int Attack { get; private set; }
    public int WeaponId { get; private set; }
    public int Luck { get; private set; }
    
    public PlayerStats(UserData userData)
    {
        MaxHealth = userData.HP;
        CurrentHealth = userData.HP;
        Attack = userData.Attack;
        WeaponId = userData.WeaponEquiped;
        // Luck = userData.Luck; // Assuming this exists
    }
    
    public void TakeDamage(int damage)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
    }

    public int RealHeal(int amount)
    {
        return Mathf.Min(CurrentHealth + amount, MaxHealth) - CurrentHealth;
    }
    
    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
    }
    
    public void IncreaseAttack(int amount)
    {
        Attack += amount;
    }
}

public class Inventory
{
    private List<ItemType> items = new List<ItemType>();
    
    public void AddItem(ItemType item)
    {
        items.Add(item);
    }
    
    public bool HasItem(ItemType item)
    {
        return items.Contains(item);
    }
    
    public void RemoveItem(ItemType item)
    {
        items.Remove(item);
    }
}

public struct LevelData
{
    public List<Cell> cells;
    public List<Enemy> enemies;
    public List<ItemSpawn> items;
}

public struct ItemSpawn
{
    public ItemType type;
    public Vector2Int position;
    public int value;
}

public enum GameState
{
    Initializing,
    GeneratingLevel,
    Playing,
    GameOver
}