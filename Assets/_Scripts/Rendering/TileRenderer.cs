using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileRenderer
{
    void RenderCell(Cell cell);
}

public class TilemapRenderer : ITileRenderer
{
    private readonly UnityEngine.Tilemaps.Tilemap itemsTilemap;
    private readonly UnityEngine.Tilemaps.Tilemap topTilemap;
    private readonly UnityEngine.Tilemaps.Tilemap superTopTilemap;
    private readonly BoardTileAssets tileAssets;
    
    public TilemapRenderer(UnityEngine.Tilemaps.Tilemap items, UnityEngine.Tilemaps.Tilemap top, 
                          UnityEngine.Tilemaps.Tilemap superTop, BoardTileAssets assets)
    {
        itemsTilemap = items;
        topTilemap = top;
        superTopTilemap = superTop;
        tileAssets = assets;
    }

    public void RenderCell(Cell cell)
    {
        Vector3Int tilePosition = new Vector3Int(cell.Pos.x, cell.Pos.y, 0);

        // Render items
        RenderItem(cell, tilePosition);

        // Render enemies
        //RenderEnemy(cell, tilePosition);
        
        // Render cell type
        RenderCellType(cell, tilePosition);
    }
    
    private void RenderCellType(Cell cell, Vector3Int position)
    {
        switch (cell.Type)
        {
            case CellType.Empty:
                if (cell.isRevelated)
                {
                    topTilemap.SetTile(position, null);
                    //RenderRandomFloorTile(position);
                }
                else if (cell.Locks > 0)
                {
                    superTopTilemap.SetTile(position, tileAssets.blockedTile);
                }
                else
                {
                    RenderRandomFloorTile(position);
                    superTopTilemap.SetTile(position, null);
                }
                break;

            case CellType.Entrance:
                topTilemap.SetTile(position, tileAssets.entranceTile);
                superTopTilemap.SetTile(position, tileAssets.entranceTile);
                break;

            case CellType.Wall:
                superTopTilemap.SetTile(position, null);
                topTilemap.SetTile(position, tileAssets.wallTile);
                break;

            case CellType.Exit:
                if (cell.isRevelated)
                {
                    topTilemap.SetTile(position, null);
                }
                else if (cell.Locks > 0)
                {
                    superTopTilemap.SetTile(position, tileAssets.blockedTile);
                }
                else
                {
                    if(!DungeonGameManager.Instance.playerManager.HasKey()) RenderRandomFloorTile(position);
                    superTopTilemap.SetTile(position, null);
                }
                break;
        }
    }
    
    private void RenderRandomFloorTile(Vector3Int position)
    {
        int tileRandom = Random.Range(0, 100);
        int rotationRandom = Random.Range(0, 4);
        
        UnityEngine.Tilemaps.Tile tileToUse;
        if (tileRandom > 30) tileToUse = tileAssets.normalTile;
        else if (tileRandom > 20) tileToUse = tileAssets.normalTileBroked1;
        else if (tileRandom > 10) tileToUse = tileAssets.normalTileBroked2;
        else tileToUse = tileAssets.normalTileBroked3;
        
        Quaternion rotation = Quaternion.Euler(0, 0, rotationRandom * 90);
        tileToUse.transform = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        
        topTilemap.SetTile(position, tileToUse);
    }
    
    private void RenderItem(Cell cell, Vector3Int position)
    {
        UnityEngine.Tilemaps.Tile itemTile = null;
        
        switch (cell.Item)
        {
            case ItemType.Key:
                itemTile = tileAssets.stairKeyTile;
                break;
            case ItemType.Potion:
                itemTile = tileAssets.potionTile;
                break;
            case ItemType.BigPotion:
                itemTile = tileAssets.bigPotionTile;
                break;
            case ItemType.Sword:
                itemTile = tileAssets.swordTile;
                break;
            case ItemType.Coin:
                itemTile = tileAssets.coinTile;
                break;
            case ItemType.Chest:
                itemTile = tileAssets.chestTile;
                break;
            case ItemType.Exit:
                bool hasKey = DungeonGameManager.Instance.playerManager.HasKey();
                itemTile = hasKey ? tileAssets.stairTile : tileAssets.blockedStairTile;
                break;
            case ItemType.None:
                itemTile = null;
                break;
        }
        
        itemsTilemap.SetTile(position, itemTile);
    }
    
}