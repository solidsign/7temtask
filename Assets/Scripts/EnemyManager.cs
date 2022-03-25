using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private MapGrid _grid;
    private List<Enemy> _enemies;

    public void Init(MapGrid grid, List<Enemy> enemies)
    {
        _grid = grid;
        _enemies = enemies;
        foreach (var enemy in _enemies)
        {
            var newPos = _grid.GetRandomPassableNearCell(enemy.GridPosition);
            var lastCell = _grid.GetCell(enemy.GridPosition.x, enemy.GridPosition.y);
            enemy.SetNewDestination(lastCell.transform.position, newPos.transform.position, lastCell.Info.SpeedModifier, newPos.Info.SpeedModifier, newPos.Coords);
        }
    }

    private void Update()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.FinishedWalk)
            {
                var newPos = _grid.GetRandomPassableNearCell(enemy.GridPosition);
                var lastCell = _grid.GetCell(enemy.GridPosition.x, enemy.GridPosition.y);
                enemy.SetNewDestination(lastCell.transform.position, newPos.transform.position, lastCell.Info.SpeedModifier, newPos.Info.SpeedModifier, newPos.Coords);
            }
        }
    }
}