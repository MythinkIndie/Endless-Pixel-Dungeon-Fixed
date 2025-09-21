using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardTileAssets
{
    [Header("Floor Tiles")]
    public UnityEngine.Tilemaps.Tile normalTile;
    public UnityEngine.Tilemaps.Tile normalTileBroked1;
    public UnityEngine.Tilemaps.Tile normalTileBroked2;
    public UnityEngine.Tilemaps.Tile normalTileBroked3;
    public UnityEngine.Tilemaps.Tile blockedTile;
    
    [Header("Structure Tiles")]
    public UnityEngine.Tilemaps.Tile wallTile;
    public UnityEngine.Tilemaps.Tile entranceTile;
    public UnityEngine.Tilemaps.Tile floorTile;
    
    [Header("Stair Tiles")]
    public UnityEngine.Tilemaps.Tile stairKeyTile;
    public UnityEngine.Tilemaps.Tile stairTile;
    public UnityEngine.Tilemaps.Tile blockedStairTile;
    
    [Header("Item Tiles")]
    public UnityEngine.Tilemaps.Tile potionTile;
    public UnityEngine.Tilemaps.Tile bigPotionTile;
    public UnityEngine.Tilemaps.Tile swordTile;
    public UnityEngine.Tilemaps.Tile coinTile;
    public UnityEngine.Tilemaps.Tile chestTile;
    
    [Header("Enemy Tiles")]
    public UnityEngine.Tilemaps.Tile enemyTile;
}
