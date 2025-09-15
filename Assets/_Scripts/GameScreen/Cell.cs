using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Enemy {

    public int Attack;
    public int Health;
    public Sprite EnemySprite;
    public int SpecificEnemy;
    public Specie TypeOfEnemy;
    public List<StateOfCharacter> States;
    public bool WasFreezed;

    public Enemy(int attack, int health, Sprite tile, int specificenemy) {

        Attack = attack;
        Health = health;
        EnemySprite = tile;
        SpecificEnemy = specificenemy;

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

public enum CellType {

    Empty,
    Wall,
    Entrance

}

public enum ItemType {

    None,
    Key,
    Potion,
    BigPotion,
    Coin,
    Sword,
    Chest,
    Exit

}

[Serializable]
public class Cell {
    
    public Vector2Int Pos = Vector2Int.zero;
    public bool isRevelated = false;
    public int Locks = 0;
    public CellType Type = CellType.Empty;
    public ItemType Item = ItemType.None;
    public Enemy Enemy; 

    public Cell () {}

    public Cell (int x, int y) {

        Pos.x = x;
        Pos.y = y;

    }
}
