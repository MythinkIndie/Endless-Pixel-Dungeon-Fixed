using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy UI")]
    public AttackDefenseView enemyUIPrefab;
    public EnemyPrefabListItem enemyListItemPrefab;
    public Transform enemyListParent;

    private List<Enemy> activeEnemies = new List<Enemy>();
    private List<AttackDefenseView> enemyUIList = new List<AttackDefenseView>();
    private List<EnemyPrefabListItem> enemyListItems = new List<EnemyPrefabListItem>();

    public System.Action<Enemy> OnEnemyDefeated;
    public System.Action<CombatResult, Enemy> OnStatusKillEnemy;

    public void Initialize()
    {
        // Setup enemy management system
    }

    public void SpawnEnemies(List<Enemy> enemies)
    {
        ClearEnemies();
        activeEnemies.AddRange(enemies);

        foreach (var enemy in enemies)
        {
            CreateEnemyUI(enemy);
        }
    }

    private void CreateEnemyUI(Enemy enemy)
    {
        // Create world UI for enemy
        var enemyUI = GetAvailableEnemyUI();
        enemyUI.gameObject.SetActive(true);
        enemyUI.transform.position = new Vector3(enemy.Position.x - 0.075f, enemy.Position.y, 0);
        enemyUI.BoardPos = enemy.Position;
        enemyUI.SetData(enemy);


    }

    private AttackDefenseView GetAvailableEnemyUI()
    {
        // Find available UI element
        var available = enemyUIList.Find(ui => ui.BoardPos.x == -1 && ui.BoardPos.y == -1);

        if (available != null)
        {
            return available;
        }

        // Create new one if none available
        var newUI = Instantiate(enemyUIPrefab);
        enemyUIList.Add(newUI);
        return newUI;
    }

    public void ShowEnemyOnList(Enemy enemy)
    {
        // Create list item
        var listItem = Instantiate(enemyListItemPrefab, enemyListParent);
        listItem.SetData(enemyUIList.Find(ui => ui.BoardPos == enemy.Position)._enemy, -1);
        enemyListItems.Add(listItem);

    }

    public void UpdateEnemyUI(Enemy enemy, int damage)
    {
        var enemyUI = enemyUIList.Find(ui => ui.BoardPos == enemy.Position);
        if (enemyUI != null)
        {
            enemy.Health = Mathf.Max(0, enemy.Health - damage);
            enemyUI.SetData(enemy, true);
        }

        var listItem = enemyListItems.Find(item => item.x == enemy.Position.x && item.y == enemy.Position.y);
        if (listItem != null)
        {
            int stateId = enemy.States.Count > 0 ? (int)enemy.States[0] : -1;
            listItem.SetData(enemyUI._enemy, stateId);
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        activeEnemies.Remove(enemy);

        var enemyUI = enemyUIList.Find(ui => ui.BoardPos == enemy.Position);
        if (enemyUI != null)
        {
            enemyUI.BoardPos = new Vector2Int(-1, -1);
            enemyUI.gameObject.SetActive(false);
        }

        var listItem = enemyListItems.Find(item => item.x == enemy.Position.x && item.y == enemy.Position.y);
        if (listItem != null)
        {
            enemyListItems.Remove(listItem);
            Destroy(listItem.gameObject);
        }

        OnEnemyDefeated?.Invoke(enemy);
    }

    private void ClearEnemies()
    {
        activeEnemies.Clear();

        foreach (var ui in enemyUIList)
        {
            ui.BoardPos = new Vector2Int(-1, -1);
            ui.gameObject.SetActive(false);
        }

        foreach (var item in enemyListItems)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        enemyListItems.Clear();
    }

    public void ApplyStatusEffects()
    {
        //Lista de todos los enemigos que tengan efecto de estado y sacar el combat result
        foreach (Enemy e in activeEnemies.FindAll(_e => _e.States.Count > 0))
        {
            var result = CombatSystem.CalculateStatusDamage(e);
            OnStatusKillEnemy?.Invoke(result, e);
        }

    }
}

public enum Specie {

    Undead,
    Demon,
    Beast

}

public enum StateOfCharacter {

    Fired, // No afecta a no muertos ni demonios
    Freezed, // Pierde el ataque una única vez
    Poisoned, // No afecta a demonios
    Blessing, // Bendición no afecta a animales
    Cursed, //Maldicion No afecta a no muertos
    Bleeding //Sangrado No afecta a no muertos

}