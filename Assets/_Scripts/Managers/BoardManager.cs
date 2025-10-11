using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Board Settings")]
    public int boardWidth = 9;
    public int boardHeight = 7;
    private Camera _camara;

    [Header("Visual Components")]
    public UnityEngine.Tilemaps.Tilemap itemsTilemap;
    public UnityEngine.Tilemaps.Tilemap topTilemap;
    public UnityEngine.Tilemaps.Tilemap superTopTilemap;

    [Header("Tile Assets")]
    public BoardTileAssets tileAssets;

    // Board state
    private List<Cell> board = new List<Cell>();
    private ITileRenderer tileRenderer;

    // Events
    public Action<Cell> OnCellRevealed;
    public Action<Cell> OnCellInteraction;
    public Action<Enemy> NewEnemyRevelated;
    public Action<Vector2Int> OnCellClicked;

    [SerializeField] private List<AudioClip> EnemyApparenceSFX;
    [SerializeField] private List<AudioClip> DiscoverTileSFX;

    public void Initialize()
    {
        tileRenderer = new TilemapRenderer(itemsTilemap, topTilemap, superTopTilemap, tileAssets);
        _camara = Camera.main;
    }

    public void LoadLevel(LevelData levelData)
    {
        board.Clear();
        board.AddRange(levelData.cells);

        // Render all cells
        foreach (var cell in board)
        {

            tileRenderer.RenderCell(cell);

        }
    }

    void Update()
    {

        Vector3? touchedPos = GetTouchedPos();

        if (!touchedPos.HasValue)
        {

            return;

        }

        var pos = _camara.ScreenToWorldPoint(touchedPos.Value);
        //Debug.Log(pos.x + " " + pos.y);
        bool inBounds = pos.x > -0.5f && pos.x < boardWidth && pos.y > 1.5f && pos.y < boardHeight + 2;

        if (!inBounds)
        {

            return;

        }

        HandleCellClick(new Vector2Int(Mathf.FloorToInt(pos.x + 0.5f), Mathf.FloorToInt(pos.y + 0.5f)));

    }

    public void HandleCellClick(Vector2Int position)
    {

        var cell = GetCell(position);

        if (cell == null || !CanRevealCell(cell)) return;

        RevealCell(cell);

    }

    private void RevealCell(Cell cell)
    {
        if (cell.isRevelated && cell.Item == ItemType.None && cell.Enemy == null) return;

        if (!cell.isRevelated)
        {
            cell.isRevelated = true;

            // Lock adjacent cells if enemy present
            if (cell.Enemy != null)
            {
                cell.Enemy.Position = cell.Pos;
                NewEnemyRevelated?.Invoke(cell.Enemy);
                LockAdjacentCells(cell.Pos);
                AudioManager.SharedInstance.PlaySound(EnemyApparenceSFX[UnityEngine.Random.Range(0, DiscoverTileSFX.Count)]);
            }
            else
            {
                AudioManager.SharedInstance.PlaySound(DiscoverTileSFX[UnityEngine.Random.Range(0, DiscoverTileSFX.Count)]);
                OnCellRevealed?.Invoke(cell);
            }

        }
        else
        {
            //Si hay objeto debajo del enemigo no pillarlo
            tileRenderer.RenderCell(cell);
            OnCellInteraction?.Invoke(cell);

        }

        tileRenderer.RenderCell(cell);

    }

    private bool CanRevealCell(Cell cell)
    {
        if (cell.Locks > 0 || cell.Type == CellType.Wall) return false;

        var neighbors = GetNeighbors(cell.Pos);
        var adjacentRevealed = neighbors.FindAll(n =>
            (n.Pos.x == cell.Pos.x || n.Pos.y == cell.Pos.y) &&
            (n.isRevelated || n.Type == CellType.Entrance));

        return adjacentRevealed.Count > 0;
    }

    public void UnlockAdjacentCells(Vector2Int position)
    {
        var neighbors = GetNeighbors(position);
        foreach (var neighbor in neighbors)
        {
            if (neighbor.Locks > 0)
            {
                neighbor.Locks--;
                tileRenderer.RenderCell(neighbor);
            }
        }
    }

    private void LockAdjacentCells(Vector2Int position)
    {
        var neighbors = GetNeighbors(position);
        foreach (var neighbor in neighbors)
        {
            if (!neighbor.isRevelated && neighbor.Type != CellType.Wall)
            {
                neighbor.Locks++;
                tileRenderer.RenderCell(neighbor);
            }
        }
    }

    public Cell GetCell(Vector2Int position)
    {
        return board.Find(c => c.Pos.x == position.x && c.Pos.y == position.y);
    }

    public List<Cell> GetNeighbors(Vector2Int position)
    {
        List<Vector2Int> neighborPositions = new List<Vector2Int>();

        // Adjacent cells
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                Vector2Int neighborPos = new Vector2Int(position.x + dx, position.y + dy);
                if (IsValidPosition(neighborPos))
                {
                    neighborPositions.Add(neighborPos);
                }
            }
        }

        return board.FindAll(c => neighborPositions.Contains(c.Pos));
    }

    Vector3? GetTouchedPos()
    {

        if (Input.GetMouseButtonDown(0))
        {

            return Input.mousePosition;

        }
        return null;

    }

    private bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.x < boardWidth &&
               position.y >= 2 && position.y < boardHeight + 2;
    }

    public void UnlockExit()
    {
        var exit = board.Find(c => c.Item == ItemType.Exit);
        if (exit != null)
        {
            tileRenderer.RenderCell(exit);
        }
    }

    public void RenderEntrance()
    {
        var entrance = board.Find(c => c.Type == CellType.Entrance);
        if (entrance != null)
        {
            tileRenderer.RenderCell(entrance);
        }
    }

    public void RemoveEnemyFrom(Vector2Int pos)
    {

        var cell = board.Find(c => c.Pos == pos);
        if (cell != null) cell.Enemy = null;

    }
    
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

public enum CellType
{

    Empty,
    Wall,
    Entrance,
    Exit

}

