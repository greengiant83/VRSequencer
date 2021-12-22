using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGrid : MonoBehaviour
{
    private const float tileSize = 0.02f;

    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private Transform TileContainer;

    private List<Tile> tiles = new List<Tile>();
    private Sequencer source;
    private Vector2 size = new Vector2();
    
    public event Action Changed;

    private static InputGrid instance;

    public static int RowCount = 12;
    public static int ColCount = 4;

    private void Start()
    {
        instance = this;
        generateTiles(RowCount, ColCount);
    }

    public void SetSource(Sequencer Source)
    {
        this.source = Source;

        for(int row=0;row<RowCount;row++)
        {
            for(int col=0;col<ColCount;col++)
            {
                var tile = tiles[getIndex(row, col)];
                tile.IsOn = source.Data.Columns[col].Cells[row].Value;
            }
        }
    }

    private void generateTiles(int rows, int cols)
    {
        TileContainer.ClearChildren();
        tiles.Clear();

        Vector3 offset = new Vector3((cols - 1) * tileSize / 2, (rows - 1) * tileSize / 2, 0);

        for(int row = 0; row < rows; row++)
        {
            for(int col = 0; col < cols; col++)
            {
                var tile = Instantiate(TilePrefab).GetComponent<Tile>();
                tile.gameObject.name = "Tile " + row + ":" + col;
                tile.transform.SetParent(TileContainer, false);
                tile.transform.localPosition = new Vector3(col * tileSize, row * tileSize, 0) - offset;
                tile.transform.localScale = Vector3.one * tileSize * 0.9f;
                tile.Row = row;
                tile.Col = col;
                tile.Changed += Tile_Changed;
                tiles.Add(tile);
            }
        }
        size = new Vector2(tileSize * cols, tileSize * rows);
    }

    private void Tile_Changed(Tile sender)
    {
        if (source == null) return;

        source.Data.Columns[sender.Col].Cells[sender.Row].Value = sender.IsOn;
        source.DataUpdated();
        Changed?.Invoke();
    }

    private int getIndex(int row, int col)
    {
        return row * ColCount + col;
    }
}
