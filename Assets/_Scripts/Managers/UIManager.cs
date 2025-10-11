using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TMPro.TMP_Text playerAttackText;
    public TMPro.TMP_Text playerHealthText;
    public TMPro.TMP_Text playerMaxHealthText;
    public TMPro.TMP_Text playerGoldText;
    public TMPro.TMP_Text playerDepthText;

    [Header("Popups")]
    public GameObject gameOverPopup;
    public GameObject personalRecordPopup;
    public GameObject chestFoundPopup;
    public GameObject exitPopup;

    [Header("Inventory")]
    public GameObject inventoryBox;

    [Header("Effects")]
    public UnityEngine.UI.Image bloodScreen;
    public UnityEngine.UI.Image bloodScreenBounds;

    private bool isBloodAnimating = false;

    public void Initialize()
    {
        // Subscribe to player events
        var playerManager = DungeonGameManager.Instance.playerManager;
        playerManager.OnHealthChanged += UpdateHealthUI;
        playerManager.OnAttackChanged += UpdateAttackUI;
        playerManager.OnPickupKeyChest += UpdateInventoryUI;

        DungeonGameManager.Instance.OnFloorChanged += UpdateDepthUI;
        DungeonGameManager.Instance.OnGameOver += ShowGameOver;

        UpdateUI();
        InitializeUpdateUI();
    }

    public void UpdateUI()
    {
        var playerManager = DungeonGameManager.Instance.playerManager;

        if (playerManager != null)
        {
            var player = playerManager.GetPlayer();
            playerAttackText.text = player.Attack.ToString();
            playerHealthText.text = player.CurrentHealth.ToString();
            playerMaxHealthText.text = "/ " + player.MaxHealth.ToString();
        }

        if (UserData.SharedInstance != null)
        {
            playerGoldText.text = UserData.SharedInstance.Gold.ToString();
        }

        playerDepthText.text = DungeonGameManager.Instance.currentFloor.ToString();

    }

    private void InitializeUpdateUI()
    {
        UpdateInventoryUI(ItemType.Key, false);
        UpdateInventoryUI(ItemType.Chest, false);
    }

    private void UpdateHealthUI(int newHealth)
    {
        playerHealthText.text = newHealth.ToString();
        UpdateBloodEffect();
    }

    private void UpdateAttackUI(int newAttack)
    {
        playerAttackText.text = newAttack.ToString();
    }

    private void UpdateDepthUI(int newDepth)
    {
        playerDepthText.text = newDepth.ToString();
    }

    public void UpdateGoldUI(int newGold)
    {
        playerGoldText.text = newGold.ToString();
    }

    private void UpdateBloodEffect()
    {
        if (!isBloodAnimating)
        {
            var player = DungeonGameManager.Instance.playerManager.GetPlayer();
            float healthPercent = (float)player.CurrentHealth / player.MaxHealth;
            float bloodIntensity = Mathf.Abs(healthPercent - 1f) / 20;

            StartCoroutine(BloodEffectCoroutine(bloodIntensity));
        }
    }

    private System.Collections.IEnumerator BloodEffectCoroutine(float intensity)
    {
        isBloodAnimating = true;

        Color bloodColor = bloodScreen.color;
        Color boundsColor = bloodScreenBounds.color;

        // Breathing effect
        while (true)
        {
            // Fade in
            bloodColor.a = intensity;
            boundsColor.a = intensity * 1.8f;
            bloodScreen.color = bloodColor;
            bloodScreenBounds.color = boundsColor;

            yield return new WaitForSeconds(0.8f);

            // Fade out
            bloodColor.a = intensity - 0.01f;
            boundsColor.a = (intensity - 0.01f) * 1.8f;
            bloodScreen.color = bloodColor;
            bloodScreenBounds.color = boundsColor;

            yield return new WaitForSeconds(0.8f);
        }
    }

    public void ShowGameOver()
    {
        gameOverPopup.SetActive(true);
    }

    public void RetryGameButton()
    {

    }

    public void OpenExitMenuButton()
    {
        exitPopup.SetActive(true);
    }
    public void CloseExitMenuButton()
    {
        exitPopup.SetActive(false);
    }

    public void UpdateInventoryUI(ItemType item, bool hasItem)
    {
        if (item == ItemType.Key)
        {
            var keyImage = inventoryBox.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
            keyImage.color = hasItem ? Color.white : new Color(0.5f, 0.5f, 0.5f, 0.3f);
        }
        else if (item == ItemType.Chest)
        {
            var chestImage = inventoryBox.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>();
            chestImage.color = hasItem ? Color.white : new Color(0.5f, 0.5f, 0.5f, 0.3f);
        }
    }

    public void ShowEnemyInterface(Enemy enemy)
    { 
        
    }
}