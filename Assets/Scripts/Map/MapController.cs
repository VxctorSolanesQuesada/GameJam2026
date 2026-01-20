using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
   
    public GameObject tilePrefab;
    public int width = 5;
    public int height = 5;
    public float spacing = 2f;

    public Tile[,] grid;

    private void Awake()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new Tile[width, height];

        // --- LISTA EXACTA DE TIPOS ---
        List<TileType> pool = new List<TileType>();

        Add(pool, TileType.Damage8, 1);
        Add(pool, TileType.Damage5, 2);
        Add(pool, TileType.Damage3, 3);
        Add(pool, TileType.Damage2, 7);
        Add(pool, TileType.Damage1, 6);
        Add(pool, TileType.Empty, 4);

        // --- CREAR TODAS LAS CASILLAS ---
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile = new Tile();
                grid[x, y] = tile;

                Vector3 pos = new Vector3(x * spacing, 0, y * spacing);
                GameObject go = Instantiate(tilePrefab, pos, Quaternion.identity, transform);

                TileVisual visual = go.GetComponent<TileVisual>();
                tile.visual = visual;

                visual.SetHidden();
            }
        }

        // --- SPAWN EN EL CENTRO ---
        int cx = width / 2; 
        int cy = height / 2; 
        grid[cx, cy].type = TileType.Spawn; 
        
        // Revelar inmediatamente el spawn
        grid[cx, cy].revealed = true; 
        grid[cx, cy].visual.SetType(TileType.Spawn);

        // --- VICTORY EN UN BORDE ---
        List<Vector2Int> borderTiles = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            borderTiles.Add(new Vector2Int(x, 0));
            borderTiles.Add(new Vector2Int(x, height - 1));
        }
        for (int y = 1; y < height - 1; y++)
        {
            borderTiles.Add(new Vector2Int(0, y));
            borderTiles.Add(new Vector2Int(width - 1, y));
        }

        Vector2Int winPos = borderTiles[Random.Range(0, borderTiles.Count)];
        grid[winPos.x, winPos.y].type = TileType.Victory;

        // --- RELLENAR EL RESTO CON EL POOL ---
        foreach (var pos in AllPositions())
        {
            Tile tile = grid[pos.x, pos.y];

            if (tile.type == TileType.Spawn || tile.type == TileType.Victory)
                continue;

            int index = Random.Range(0, pool.Count);
            tile.type = pool[index];
            pool.RemoveAt(index);
        }

        // --- ASIGNAR VALOR DE DAÑO ---
        foreach (var pos in AllPositions())
        {
            Tile tile = grid[pos.x, pos.y];

            switch (tile.type)
            {
                case TileType.Damage1: tile.damageValue = 1; break;
                case TileType.Damage2: tile.damageValue = 2; break;
                case TileType.Damage3: tile.damageValue = 3; break;
                case TileType.Damage5: tile.damageValue = 5; break;
                case TileType.Damage8: tile.damageValue = 8; break;
                default: tile.damageValue = 0; break;
            }
        }
    }

    void Add(List<TileType> list, TileType type, int count)
    {
        for (int i = 0; i < count; i++)
            list.Add(type);
    }

    IEnumerable<Vector2Int> AllPositions()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                yield return new Vector2Int(x, y);
    }

    public void RevealTile(int x, int y)
    {
        Tile tile = grid[x, y];

        if (tile.revealed)
            return;

        tile.revealed = true;
        tile.visual.SetType(tile.type);

        if (tile.damageValue > 0)
        {
            Debug.Log($"Tile hace {tile.damageValue} de daño");
            // player.TakeDamage(tile.damageValue);
        }
    }
}



