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
                    totalDamage += 2;
                    break;
                case StateOfCharacter.Freezed:
                    totalDamage += 1;
                    break;
                case StateOfCharacter.Poisoned:
                    totalDamage += 2;
                    break;
                case StateOfCharacter.Blessing:
                    totalDamage += 3;
                    break;
                case StateOfCharacter.Cursed:
                    totalDamage += 2;
                    break;
                case StateOfCharacter.Bleeding:
                    totalDamage += 2;
                    break;
            }
        }
        
        return totalDamage;
    }
}