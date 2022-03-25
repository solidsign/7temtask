using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class MapGrid : MonoBehaviour
{
    [SerializeField] [HideInInspector] private Vector2 _firstPoint;
    [SerializeField] [HideInInspector] private Vector2Int _size;
    [SerializeField] [HideInInspector] private Vector2 _horizontalOffset;
    [SerializeField] [HideInInspector] private Vector2 _verticalOffset;

    private Cell[,] _grid;

    public int Width => _size.x;
    public int Height => _size.y;
    public void Init(Vector2 firstPoint, Vector2Int size, Vector2 horizontalOffset, Vector2 verticalOffset)
    {
        _firstPoint = firstPoint;
        _size = size;
        _horizontalOffset = horizontalOffset;
        _verticalOffset = verticalOffset;
    }

    private void Awake()
    {
        _grid = new Cell[_size.x, _size.y];
        foreach (var cell in GetComponentsInChildren<Cell>())
        {
            _grid[cell.Coords.x, cell.Coords.y] = cell;
        }
    }

    public Cell ClosestPassableCell(Vector2 worldPosition)
    {
        Cell res = null;
        float minDist = Single.MaxValue;
        
        foreach (var cell in _grid)
        {
            if (!cell.Info.Passable) continue;
            var distance = Vector2.Distance(worldPosition, cell.transform.position);
            if (distance < minDist)
            {
                res = cell;
                minDist = distance;
            }
        }
        
        return res;
    }

    public Cell GetCell(int x, int y) => _grid[x, y];
    public Cell GetCell(Vector2Int coords) => _grid[coords.x, coords.y];

    public List<Cell> GetRandomPassableCells(int amount)
    {
        var res = new List<Cell>(amount);
        int i = 0;
        while (i < amount)
        {
            var cell = _grid[Random.Range(0, _size.x), Random.Range(0, _size.y)];
            if (!cell.Info.Passable || res.Contains(cell)) continue;
            res.Add(cell);
            i++;
        }

        return res;
    }

    public Cell GetRandomPassableNearCell(Vector2Int coords)
    {
        List<Cell> l = new List<Cell>(4);
        if(_size.x > coords.x + 1 && _grid[coords.x + 1, coords.y].Info.Passable) l.Add(_grid[coords.x + 1, coords.y]);
        if(0 <= coords.x - 1 && _grid[coords.x - 1, coords.y].Info.Passable) l.Add(_grid[coords.x - 1, coords.y]);
        if(_size.y > coords.y + 1 && _grid[coords.x, coords.y + 1].Info.Passable) l.Add(_grid[coords.x, coords.y + 1]);
        if(0 <= coords.y - 1 && _grid[coords.x, coords.y - 1].Info.Passable) l.Add(_grid[coords.x, coords.y - 1]);

        return l[Random.Range(0, l.Count)];
    }
}