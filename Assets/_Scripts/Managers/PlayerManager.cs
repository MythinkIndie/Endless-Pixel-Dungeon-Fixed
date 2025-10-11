using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerStats playerStats;
    private bool isKeyInInventory = false;
    private bool isChestInInventory = false;
    public System.Action<int> OnHealthChanged;
    public System.Action<int> OnAttackChanged;
    public System.Action<ItemType, bool> OnPickupKeyChest;
    public System.Action OnPlayerDeath;
    public bool AttackBoost = false;

    public void Initialize(UserData userData)
    {
        playerStats = new PlayerStats(userData);
    }

    public void TakeDamage(int damage)
    {
        playerStats.TakeDamage(damage);
        OnHealthChanged?.Invoke(playerStats.CurrentHealth);

        if (playerStats.CurrentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        playerStats.Heal(amount);
        OnHealthChanged?.Invoke(playerStats.CurrentHealth);
    }

    public void IncreaseAttack(int amount)
    {

        playerStats.IncreaseAttack(amount);
        OnAttackChanged?.Invoke(playerStats.Attack);
        
    }

    public bool HasKey()
    {
        return isKeyInInventory;
    }

    public void AddItem(ItemType item)
    {

        if (ItemType.Key == item)
        {
            isKeyInInventory = true;
            OnPickupKeyChest?.Invoke(item, true);
        }
        if (ItemType.Chest == item)
        {
            isChestInInventory = true;
            OnPickupKeyChest?.Invoke(item, true);
        }
    }

    public void RemoveKey()
    {
        isKeyInInventory = false;
        OnPickupKeyChest?.Invoke(ItemType.Key, false);
    }

    public PlayerStats GetPlayer() => playerStats;
}