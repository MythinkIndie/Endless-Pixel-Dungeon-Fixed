using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory
{
    // Referencia al array de sprites del BoardController original
    private static List<Sprite> enemySprites;
    
    public static void Initialize(List<Sprite> sprites)
    {
        enemySprites = sprites;
    }
    
    public static Enemy CreateEnemy(int floorLevel, int specificEnemyId, int baseAttackMin, int baseAttackMax, int baseHealthMin, int baseHealthMax)
    {
        // Usar la lógica original de scaling
        int attackMin = baseAttackMin + (int)(floorLevel * 1.5f);
        int attackMax = baseAttackMax + (int)(floorLevel * 1.5f);
        int healthMin = baseHealthMin + (int)(floorLevel * 2.5f);
        int healthMax = baseHealthMax + (int)(floorLevel * 3f);
        
        // Generar stats aleatorios como en el original
        int finalAttack = Random.Range(attackMin, attackMax);
        int finalHealth = Random.Range(healthMin, healthMax);
        
        // Obtener sprite
        Sprite enemySprite = null;

        if (enemySprites != null && specificEnemyId < enemySprites.Count)
        {
            enemySprite = enemySprites[specificEnemyId];
        }
        
        // Crear enemigo usando el constructor original
        var enemy = new Enemy(finalAttack, finalHealth, enemySprite, specificEnemyId);
        
        return enemy;
    }
    
    // Método simplificado para generar enemigo aleatorio
    public static Enemy CreateRandomEnemy(int floorLevel)
    {
        // Generar ID de enemigo excluyendo los problemáticos (como en tu código original)
        int specificEnemyId = Random.Range(0, 33);
        
        // Usar stats base como en tu código original
        return CreateEnemy(floorLevel, specificEnemyId, 1, 3, 14, 22);
    }
}

public struct EnemyTemplate
{
    public int EnemySprite;
    public int BaseAttack;
    public int BaseHealth;
    public Specie Species;
}

public enum EnemyType
{
    Beast,
    Demon,
    Undead
}

[System.Serializable]
public class Enemy {
    [Header("Core Stats")]
    public int Attack;
    public int Health;
    public Sprite EnemySprite;
    public int SpecificEnemy;
    public Specie TypeOfEnemy;
    
    [Header("Status Effects")]
    public List<StateOfCharacter> States;
    public bool WasFreezed;
    
    // NUEVA PROPIEDAD AGREGADA
    public Vector2Int Position { get; set; } = new Vector2Int(-1, -1);

    public Enemy(int attack, int health, Sprite sprite, int specificenemy) {
        
        Attack = attack;
        Health = health;
        EnemySprite = sprite;
        SpecificEnemy = specificenemy;
        
        // Determinar el tipo basado en el ID (misma lógica que tu código original)
        if (SpecificEnemy >= 0 && SpecificEnemy <= 8) {
            TypeOfEnemy = Specie.Beast;
        } else if (SpecificEnemy >= 9 && SpecificEnemy <= 20) {
            TypeOfEnemy = Specie.Demon;
        } else {
            TypeOfEnemy = Specie.Undead;
        }
        
        States = new List<StateOfCharacter>();
        WasFreezed = false;
    }
    
    public void TryToAddState(StateOfCharacter stateToAdd) {
        switch (stateToAdd) {
            case StateOfCharacter.Fired:
                if (this.TypeOfEnemy == Specie.Beast) {
                    this.States.Add(stateToAdd);
                }
                break;
            
            case StateOfCharacter.Freezed:
                this.States.Add(stateToAdd);
                break;

            case StateOfCharacter.Poisoned:
                if (this.TypeOfEnemy != Specie.Demon) {
                    this.States.Add(stateToAdd);
                }
                break;

            case StateOfCharacter.Blessing:
                if (this.TypeOfEnemy != Specie.Beast) {
                    this.States.Add(stateToAdd);
                }
                break;

            case StateOfCharacter.Cursed:
                if (this.TypeOfEnemy != Specie.Demon) {
                    this.States.Add(stateToAdd);
                }
                break;

            case StateOfCharacter.Bleeding:
                if (this.TypeOfEnemy != Specie.Undead) {
                    this.States.Add(stateToAdd);
                }
                break;
        }
    }
    
    // Método helper para verificar si tiene armadura (del código original)
    public bool HasArmor() {
        return (SpecificEnemy >= 8 && SpecificEnemy <= 10) || 
               SpecificEnemy == 30 || 
               SpecificEnemy == 32;
    }
}