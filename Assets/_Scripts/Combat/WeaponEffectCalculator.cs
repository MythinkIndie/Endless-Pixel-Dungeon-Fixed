using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponEffectCalculator
{
    public static float GetDamageMultiplier(int weaponId, Enemy enemy, bool Boost)
    {
        switch (weaponId)
        {
            case 6: // Cursed Bane
                return enemy.TypeOfEnemy == Specie.Undead ? 1.4f : 1f;
            case 8: // Blessed weapon
                return enemy.TypeOfEnemy == Specie.Demon ? 1.4f : 1f;
            case 10: // Enhanced weapon
                return 1.25f;
            case 11:
                return Boost ? 2f : 1f;
            case 12:
                return Boost ? 1.2f : 1f;
            case 15: // Beast hunter
                return enemy.TypeOfEnemy == Specie.Beast ? 1.4f : 1f;
            case 16: // Power weapon
                return 1.75f;
            case 17: // Versatile weapon
                return 1.1f;
            case 19:
                return 1.5f;
            default:
                return 1f;
        }
    }

    public static void ApplyStatusEffects(int weaponId, Enemy enemy)
    {

        //Poner aqui un porcentaje para aplicarlo como probabilidad

        switch (weaponId) {

            case 1: // Fire weapon
                enemy.TryToAddState(StateOfCharacter.Fired);
                break;
            case 2: // Ice weapon
                enemy.TryToAddState(StateOfCharacter.Freezed);
                break;
            case 3: // Poison weapon
                enemy.TryToAddState(StateOfCharacter.Poisoned);
                break;
            case 7: // Random effect weapon
                if (enemy.States.Count == 0)
                {
                    enemy.TryToAddState((StateOfCharacter)Random.Range(0, 5));
                }
                break;
            case 8: // Blessed weapon
                enemy.TryToAddState(StateOfCharacter.Blessing);
                break;
            case 11: // Cursed weapon
                enemy.TryToAddState(StateOfCharacter.Cursed);
                break;
            case 12: // Rose Katana
                enemy.TryToAddState(StateOfCharacter.Bleeding);
                break;
            case 13: // Dual element weapon
                enemy.TryToAddState(StateOfCharacter.Fired);
                enemy.TryToAddState(StateOfCharacter.Freezed);
                break;
            case 18: // Pochita Chainsaw
                enemy.TryToAddState(StateOfCharacter.Bleeding);
                break;
        }
    }

    public static int ApplyDefensiveEffects(int weaponId, int incomingDamage, Enemy enemy, CombatResult result)
    {
        switch (weaponId)
        {
            case 4: // Dueltist Sword
                bool isEnemyDead = result.enemyDefeated;
                bool iEvadedAttack = Random.Range(0, 100) < 10;
                if (iEvadedAttack) DungeonGameManager.Instance.HandleBlockSound();

                return (iEvadedAttack || isEnemyDead) ? 0 : incomingDamage;
            case 16: // Glass cannon
                return Mathf.FloorToInt(incomingDamage * 1.5f);
            case 19: // Defensive weapon
                return Mathf.Clamp(incomingDamage - 2, 1, 9999);
            default:
                return incomingDamage;
        }
    }

    public static bool OnKillEffect(int weaponId, PlayerStats player, Enemy enemy, CombatResult result)
    {

        bool boostForNext = false;

        switch (weaponId)
        {
            case 6: // Cursed Bane
                if (result.enemyDefeated && enemy.TypeOfEnemy == Specie.Undead)
                {
                    player.Heal(Mathf.FloorToInt(player.CurrentHealth * 0.1f));
                }
                break;
            case 11:
                boostForNext = enemy.TypeOfEnemy == Specie.Demon;
                break;
            case 16:
                if (result.enemyDefeated) player.Heal(Mathf.FloorToInt(player.CurrentHealth * 0.1f));
                break;
        }

        return boostForNext;

    }
}
