using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
// public class Enemy {
//     [Header("Core Stats")]
//     public int Attack;
//     public int Health;
//     public Sprite EnemySprite;
//     public int SpecificEnemy;
//     public Specie TypeOfEnemy;
    
//     [Header("Status Effects")]
//     public List<StateOfCharacter> States;
//     public bool WasFreezed;
    
//     // NUEVA PROPIEDAD AGREGADA
//     public Vector2Int Position { get; set; } = new Vector2Int(-1, -1);

//     public Enemy(int attack, int health, Sprite sprite, int specificenemy) {
//         Attack = attack;
//         Health = health;
//         EnemySprite = sprite;
//         SpecificEnemy = specificenemy;
        
//         // Determinar el tipo basado en el ID (misma lógica que tu código original)
//         if (SpecificEnemy >= 0 && SpecificEnemy <= 8) {
//             TypeOfEnemy = Specie.Beast;
//         } else if (SpecificEnemy >= 9 && SpecificEnemy <= 20) {
//             TypeOfEnemy = Specie.Demon;
//         } else {
//             TypeOfEnemy = Specie.Undead;
//         }
        
//         States = new List<StateOfCharacter>();
//         WasFreezed = false;
//     }
    
//     public void TryToAddState(StateOfCharacter stateToAdd) {
//         switch (stateToAdd) {
//             case StateOfCharacter.Fired:
//                 if (this.TypeOfEnemy == Specie.Beast) {
//                     this.States.Add(stateToAdd);
//                 }
//                 break;
            
//             case StateOfCharacter.Freezed:
//                 this.States.Add(stateToAdd);
//                 break;

//             case StateOfCharacter.Poisoned:
//                 if (this.TypeOfEnemy != Specie.Demon) {
//                     this.States.Add(stateToAdd);
//                 }
//                 break;

//             case StateOfCharacter.Blessing:
//                 if (this.TypeOfEnemy != Specie.Beast) {
//                     this.States.Add(stateToAdd);
//                 }
//                 break;

//             case StateOfCharacter.Cursed:
//                 if (this.TypeOfEnemy != Specie.Demon) {
//                     this.States.Add(stateToAdd);
//                 }
//                 break;

//             case StateOfCharacter.Bleeding:
//                 if (this.TypeOfEnemy != Specie.Undead) {
//                     this.States.Add(stateToAdd);
//                 }
//                 break;
//         }
//     }
    
//     // Método helper para verificar si tiene armadura (del código original)
//     public bool HasArmor() {
//         return (SpecificEnemy >= 8 && SpecificEnemy <= 10) || 
//                SpecificEnemy == 30 || 
//                SpecificEnemy == 32;
//     }
// }
