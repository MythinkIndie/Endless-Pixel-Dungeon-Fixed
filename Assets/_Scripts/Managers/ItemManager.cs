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

    [SerializeField] private AudioClip KeySFX;
    [SerializeField] private AudioClip PickChestSFX;
    [SerializeField] private AudioClip DrinkPotionSFX;
    [SerializeField] private AudioClip CantPickPotionSFX;
    [SerializeField] private List<AudioClip> CoinPickSFX;
    [SerializeField] private List<AudioClip> SwordPickSFX;

    public void Initialize()
    {

    }

    public void SpawnItems(List<ItemSpawn> items)
    {
        activeItems.Clear();
        activeItems.AddRange(items);
    }

    public bool CollectItem(Cell cell)
    {
        var player = DungeonGameManager.Instance.playerManager;
        bool pickUpItem = true;
        var originalCell = cell.Item;

        switch (cell.Item)
        {
            case ItemType.Key:
                player.AddItem(ItemType.Key);
                OnPickupKeyChest?.Invoke(ItemType.Key, true);
                AudioManager.SharedInstance.PlaySound(KeySFX);
                break;

            case ItemType.Potion:
                if (player.GetPlayer().CurrentHealth < player.GetPlayer().MaxHealth)
                {
                    AudioManager.SharedInstance.PlaySound(DrinkPotionSFX);
                    int healAmount = CalculateHealAmount(7, 5);
                    ShowBuffEffect(buffHealthPrefab, player.GetPlayer().RealHeal(healAmount));
                    player.Heal(healAmount);
                }
                else
                {
                    AudioManager.SharedInstance.PlaySound(CantPickPotionSFX);
                    pickUpItem = false;
                }
                break;

            case ItemType.BigPotion:
                if (player.GetPlayer().CurrentHealth < player.GetPlayer().MaxHealth)
                {
                    AudioManager.SharedInstance.PlaySound(DrinkPotionSFX);
                    int bigHealAmount = CalculateHealAmount(16, 8);
                    ShowBuffEffect(buffHealthPrefab, player.GetPlayer().RealHeal(bigHealAmount));
                    player.Heal(bigHealAmount);
                    
                }
                else
                {
                    AudioManager.SharedInstance.PlaySound(CantPickPotionSFX);
                    pickUpItem = false;
                }
                break;

            case ItemType.Sword:
                int attackBonus = UnityEngine.Random.Range(0, 80) > 70 ? 2 : 1; // + playerData.Luck
                player.IncreaseAttack(attackBonus);
                AudioManager.SharedInstance.PlaySound(SwordPickSFX[UnityEngine.Random.Range(0, SwordPickSFX.Count)]);
                ShowBuffEffect(buffAttackPrefab, attackBonus);
                break;

            case ItemType.Coin:
                int goldAmount = CalculateGoldAmount();
                DungeonGameManager.Instance.PickUpGold(goldAmount);
                AudioManager.SharedInstance.PlaySound(CoinPickSFX[UnityEngine.Random.Range(0, CoinPickSFX.Count)]);
                ShowBuffEffect(buffCoinsPrefab, goldAmount);
                break;

            case ItemType.Chest:
                player.AddItem(ItemType.Chest);
                AudioManager.SharedInstance.PlaySound(PickChestSFX);
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
        return pickUpItem;
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

        if (UserData.SharedInstance.CanGetItemFromGame())
        {
            UserData.SharedInstance.GetItemFromChest();
        }
        else
        {
            UserData.SharedInstance.Gold += Mathf.Min(2, 5);
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