using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatSystem
{
    public static CombatResult ProcessCombat(PlayerStats player, Enemy enemy)
    {
        var result = new CombatResult();
        
        // Calculate player damage to enemy
        int playerDamage = CalculatePlayerDamage(player, enemy);
        //enemy.Health = Mathf.Max(0, enemy.Health - playerDamage);
        result.enemyDamageRecived = playerDamage;
        result.enemyDefeated = (enemy.Health - playerDamage) <= 0;

        WeaponEffectCalculator.OnKillEffect(player.WeaponId, player, enemy, result);

        // Calculate enemy damage to player (or 0)
        result.playerDamageRecived = CalculateEnemyDamage(enemy, player);
        
        return result;
    }
    
    private static int CalculatePlayerDamage(PlayerStats player, Enemy enemy)
    {
        int baseDamage = player.Attack;
        
        // Apply weapon effects
        float weaponMultiplier = WeaponEffectCalculator.GetDamageMultiplier(player.WeaponId, enemy);
        
        // Apply status effects
        int statusDamage = StatusEffectCalculator.GetStatusDamage(enemy);
        
        // Apply armor reduction
        int armorReduction = enemy.HasArmor() ? Mathf.FloorToInt(enemy.Health * 0.1f) : 0;
        
        return Mathf.Max(1, Mathf.FloorToInt(baseDamage * weaponMultiplier) + statusDamage - armorReduction);
    }
    
    private static int CalculateEnemyDamage(Enemy enemy, PlayerStats player)
    {
        int baseDamage = enemy.Attack;
        
        // Apply player weapon defensive effects
        return WeaponEffectCalculator.ApplyDefensiveEffects(player.WeaponId, baseDamage, enemy);
    }
}

public struct CombatResult
{
    public int playerDamageRecived;
    public int enemyDamageRecived;
    public bool enemyDefeated;
}