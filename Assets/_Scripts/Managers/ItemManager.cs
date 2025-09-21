using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Item Effects")]
    public BuffBenefits buffHealthPrefab;
    public BuffBenefits buffAttackPrefab;
    public BuffBenefits buffCoinsPrefab;

    private List<ItemSpawn> activeItems = new List<ItemSpawn>();

    public Action<ItemType, Vector2Int> OnItemCollected;
    public Action<ItemType, bool> OnPickupKeyChest;

    public void Initialize()
    {

    }

    public void SpawnItems(List<ItemSpawn> items)
    {
        activeItems.Clear();
        activeItems.AddRange(items);
    }

    public void CollectItem(Cell cell)
    {
        var player = DungeonGameManager.Instance.playerManager;
        bool pickUpItem = true;
        var originalCell = cell.Item;

        switch (cell.Item)
        {
            case ItemType.Key:
                player.AddItem(ItemType.Key);
                OnPickupKeyChest?.Invoke(ItemType.Key, true);
                //ShowBuffEffect(buffAttackPrefab, "Key collected!");
                break;

            case ItemType.Potion:
                if (player.GetPlayer().CurrentHealth < player.GetPlayer().MaxHealth)
                {
                    int healAmount = CalculateHealAmount(7, 5);
                    player.Heal(healAmount);
                    ShowBuffEffect(buffHealthPrefab, healAmount);
                }
                else
                {
                    pickUpItem = false;
                }
                break;

            case ItemType.BigPotion:
                if (player.GetPlayer().CurrentHealth < player.GetPlayer().MaxHealth)
                {
                    int bigHealAmount = CalculateHealAmount(16, 8);
                    player.Heal(bigHealAmount);
                    ShowBuffEffect(buffHealthPrefab, bigHealAmount);
                }
                else
                {
                    pickUpItem = false;
                }
                break;

            case ItemType.Sword:
                int attackBonus = UnityEngine.Random.Range(0, 80) > 70 ? 2 : 1; // + playerData.Luck
                player.IncreaseAttack(attackBonus);
                ShowBuffEffect(buffAttackPrefab, attackBonus);
                break;

            case ItemType.Coin:
                int goldAmount = CalculateGoldAmount();
                DungeonGameManager.Instance.PickUpGold(goldAmount);
                ShowBuffEffect(buffCoinsPrefab, goldAmount);
                break;

            case ItemType.Chest:
                player.AddItem(ItemType.Chest);
                OnPickupKeyChest?.Invoke(ItemType.Chest, true);
                //HandleChest();
                break;

            case ItemType.Exit:
                if (!player.HasKey()) pickUpItem = false;
                break;
        }

        // Clear item from cell
        cell.Item = pickUpItem? ItemType.None : cell.Item;
        OnItemCollected?.Invoke(originalCell, cell.Pos);
    }

    private int CalculateHealAmount(int base_, int variance)
    {
        return base_ + UnityEngine.Random.Range(0, variance); // + playerData.Luck calculations
    }

    private int CalculateGoldAmount()
    {
        int floor = DungeonGameManager.Instance.currentFloor;
        return UnityEngine.Random.Range(1, 3) + floor + UnityEngine.Random.Range(0, 1); // + luck bonus
    }

    private void HandleChest()
    {
        var playerData = DungeonGameManager.Instance.playerData;

        if (playerData.CanGetItemFromGame())
        {
            playerData.GetItemFromChest();
        }
        else
        {
            playerData.Gold += Mathf.Min(2, 5);
            ShowBuffEffect(buffCoinsPrefab, 100);
        }
    }

    private void ShowBuffEffect(BuffBenefits buffPrefab, int message)
    {
        buffPrefab.SetData(message);
    }
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