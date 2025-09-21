using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillFactory
{
    public static ISkill CreateSkill(int skillId, int weaponId) {
        switch (skillId) {
            case 0: return new SolarBeamSkill(weaponId);
            // case 1: return new ChainAttackSkill(weaponId);
            // case 2: return new PurifySkill(weaponId);
            // case 3: return new PowerStrikeSkill(weaponId);
            // case 4: return new BurnRoomSkill(weaponId);
            // case 5: return new RockFallSkill(weaponId);
            // case 6: return new SuperStrikeSkill(weaponId);
            // case 7: return new RandomEffectSkill(weaponId);
            default: return new NoSkill();
        }
    }
}

public interface ISkill
{
    int SkillId { get; }
    int WeaponId { get; }
    bool CanExecute();
    void Execute();
}

public class SolarBeamSkill : ISkill {
    public int SkillId => 0;
    public int WeaponId { get; private set; }
    
    private int cooldown = 0;
    private int maxCooldown = 5;
    
    public SolarBeamSkill(int weaponId)
    {
        WeaponId = weaponId;
    }
    
    public bool CanExecute()
    {
        return cooldown <= 0;
    }
    
    public void Execute() {
        
        if (!CanExecute()) return;
        
        cooldown = maxCooldown;
        
        // Get all visible enemies
        var boardManager = DungeonGameManager.Instance.boardManager;
        var playerManager = DungeonGameManager.Instance.playerManager;
        var allCells = new List<Cell>(); // Would get from board manager
        
        var visibleEnemies = allCells.FindAll(c => c.Enemy != null && c.isRevelated);
        
        foreach (var cell in visibleEnemies)
        {
            int damage = Mathf.FloorToInt(playerManager.GetPlayer().Attack * 0.75f);
            cell.Enemy.Health = Mathf.Max(0, cell.Enemy.Health - damage);
            
            DungeonGameManager.Instance.enemyManager.UpdateEnemyUI(cell, damage);
            
            if (cell.Enemy.Health <= 0)
            {
                DungeonGameManager.Instance.enemyManager.RemoveEnemy(cell.Enemy);
                cell.Enemy = null;
            }
        }
        
        Debug.Log("Solar Beam executed!");
    }
}

public class NoSkill : ISkill
{
    public int SkillId => -1;
    public int WeaponId => -1;
    public bool CanExecute() => false;
    public void Execute() { }
}