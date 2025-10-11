using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatusEffectCalculator
{
    public static int GetStatusDamage(Enemy enemy)
    {
        int totalDamage = 0;
        
        foreach (var state in enemy.States)
        {
            switch (state)
            {
                case StateOfCharacter.Fired:
                    enemy.FireTime--;
                    totalDamage += 2;
                    break;
                case StateOfCharacter.Freezed:
                    enemy.FreezedAcomulation++;
                    totalDamage += 1;
                    break;
                case StateOfCharacter.Poisoned:
                    totalDamage += 1;
                    break;
                case StateOfCharacter.Blessing:
                    enemy.BlessTime--;
                    totalDamage += enemy.TypeOfEnemy == Specie.Demon ? 3 : 1;
                    break;
                case StateOfCharacter.Cursed:
                    enemy.CursedTime--;
                    totalDamage += enemy.TypeOfEnemy == Specie.Undead ? 1 : 2;
                    break;
                case StateOfCharacter.Bleeding:
                    enemy.BleedTime--;
                    totalDamage += 2;
                    break;
            }
        }

        enemy.TryToRemoveState();
        
        return totalDamage;
    }
}