using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatSystem
{
    public static CombatResult ProcessCombat(PlayerStats player, Enemy enemy, bool Boost)
    {
        var result = new CombatResult();

        // Calculate player damage to enemy
        int playerDamage = CalculatePlayerDamage(player, enemy, Boost);
        //enemy.Health = Mathf.Max(0, enemy.Health - playerDamage);
        result.enemyDamageRecived = playerDamage;
        result.enemyDefeated = (enemy.Health - playerDamage) <= 0;

        result.playerBoostForNextAttack = WeaponEffectCalculator.OnKillEffect(player.WeaponId, player, enemy, result);

        // Calculate enemy damage to player (or 0)
        result.playerDamageRecived = CalculateEnemyDamage(enemy, player, result);

        return result;
    }

    private static int CalculatePlayerDamage(PlayerStats player, Enemy enemy, bool Boost, float mutliplier = 1f)
    {
        int baseDamage = Mathf.RoundToInt(player.Attack * mutliplier);

        // Apply weapon effects
        float weaponMultiplier = WeaponEffectCalculator.GetDamageMultiplier(player.WeaponId, enemy, Boost);

        // Apply status effects
        int statusDamage = StatusEffectCalculator.GetStatusDamage(enemy);

        // Apply armor reduction
        int armorReduction = enemy.HasArmor() ? Mathf.FloorToInt(enemy.Health * 0.1f) : 0;

        WeaponEffectCalculator.ApplyStatusEffects(player.WeaponId, enemy);

        return Mathf.Max(1, Mathf.FloorToInt(baseDamage * weaponMultiplier) + statusDamage - armorReduction);
    }

    private static int CalculateEnemyDamage(Enemy enemy, PlayerStats player, CombatResult result)
    {
        int baseDamage = enemy.Attack;
        bool CantAttack = !enemy.WasFreezed && enemy.FreezedAcomulation >= 3;
        if (CantAttack)
        {
            enemy.WasFreezed = true;
            return 0;
        }

        // Apply player weapon defensive effects
            return WeaponEffectCalculator.ApplyDefensiveEffects(player.WeaponId, baseDamage, enemy, result);
    }

    public static CombatResult CalculateStatusDamage(Enemy enemy)
    {

        var result = new CombatResult();
        //Aqui tambien meter el estado del jugador como daño extra al abrir una casilla
        int playerStatusDamage = 0;
        int enemyStatusDamage = StatusEffectCalculator.GetStatusDamage(enemy);
        result.playerDamageRecived = playerStatusDamage;
        result.enemyDamageRecived = enemyStatusDamage;
        result.enemyDefeated = (enemy.Health - enemyStatusDamage) <= 0;

        return result;

    }

    public static CombatResult ProcessSkillCombat(PlayerStats player, Enemy enemy, bool Boost, float mutliplier = 1f)
    {
        var result = new CombatResult();

        // Calculate player damage to enemy
        int playerDamage = CalculatePlayerDamage(player, enemy, Boost, mutliplier);
        //enemy.Health = Mathf.Max(0, enemy.Health - playerDamage);
        result.enemyDamageRecived = playerDamage;
        result.enemyDefeated = (enemy.Health - playerDamage) <= 0;

        result.playerBoostForNextAttack = WeaponEffectCalculator.OnKillEffect(player.WeaponId, player, enemy, result);

        // Calculate enemy damage to player (or 0)
        result.playerDamageRecived = 0;

        return result;
    }
}

public struct CombatResult
{
    public int playerDamageRecived;
    public int enemyDamageRecived;
    public bool enemyDefeated;
    public bool playerBoostForNextAttack;
}