using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum GameState
{
    Playing,
    Win,
    Lose,
    Draw
}

public class GameManager : MonoBehaviour
{
    [Header("Initialization")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private BombManager bombManager;
    [SerializeField] private MapGrid mapGrid;
    [SerializeField] private Text finishText;
    [Header("Spawning")]
    [SerializeField] private PathReader pigPrefab;
    [SerializeField] private Enemy farmerPrefab;
    [SerializeField] private Enemy dogPrefab;
    [SerializeField] [Tooltip("Not more than Grid's height")] private int pigsAmount;

    private List<PathReader> _alivePigs;
    private List<Enemy> _enemies;
    private GameState _currentGameState = GameState.Playing;
    private Coroutine _finishGame;
    
    private void Start()
    {
        Spawn();
        inputManager.Init(mapGrid, _alivePigs, bombManager);
        enemyManager.Init(mapGrid, _enemies);
        bombManager.Init(mapGrid, this);
    }

    private void Spawn()
    {
        _alivePigs = new List<PathReader>(pigsAmount);
        _enemies = new List<Enemy>(2);
        var cells = mapGrid.GetRandomPassableCells(pigsAmount + 2);
        for (var i = 0; i < cells.Count; i++)
        {
            var cell = cells[i];
            if(i < cells.Count - 2)
            {
                var p = Instantiate(pigPrefab, cell.transform.position, Quaternion.identity);
                _alivePigs.Add(p);
            }else
            {
                var p = Instantiate(i % 2 == 0 ? dogPrefab : farmerPrefab, cell.transform.position, Quaternion.identity);
                _enemies.Add(p);
            }
        }
    }

    public void ProcessExplosion(Vector2Int coords)
    {
        var enemiesForRemove = new List<Enemy>(_enemies.Count);
        foreach (var enemy in _enemies)
        {
            if(Vector2Int.Distance(enemy.GridPosition, coords) > bombManager.ExplosionDistance) continue;
            enemiesForRemove.Add(enemy);
        }

        var pigsForRemove = new List<PathReader>(_alivePigs.Count);
        foreach (var pig in _alivePigs)
        {
            var pigGridPosition = mapGrid.ClosestPassableCell(pig.transform.position).Coords;
            if(Vector2Int.Distance(pigGridPosition, coords) > bombManager.ExplosionDistance) continue;
            pigsForRemove.Add(pig);
        }

        foreach (var enemy in enemiesForRemove)
        {
            RemoveEnemy(enemy);
        }

        foreach (var pig in pigsForRemove)
        {
            RemovePig(pig);
        }
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
        Destroy(enemy.gameObject);
        if (_enemies.Count == 0) SetFinishGameState(GameState.Win);
    }

    private void RemovePig(PathReader pig)
    {
        _alivePigs.Remove(pig);
        Destroy(pig.gameObject);
        if (_alivePigs.Count == 0) SetFinishGameState(GameState.Lose);
    }

    private void SetFinishGameState(GameState state)
    {
        switch (_currentGameState)
        {
            case GameState.Playing:
                _currentGameState = state;
                break;
            case GameState.Win:
                if (state == GameState.Lose)
                    _currentGameState = GameState.Draw;
                break;
            case GameState.Lose:
                if (state == GameState.Win)
                    _currentGameState = GameState.Draw;
                break;
            case GameState.Draw:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (_finishGame == null)
            _finishGame = StartCoroutine(FinishGame());
    }

    private IEnumerator FinishGame()
    {
        yield return null;
        switch (_currentGameState)
        {
            case GameState.Win:
                Win();
                break;
            case GameState.Lose:
                Lose();
                break;
            case GameState.Draw:
                Draw();
                break;
            default:
                Debug.LogError($"GameState corrupted! GameState = {_currentGameState.ToString()}");
                break;
        }

        inputManager.enabled = false;
        enemyManager.enabled = false;
    }
    
    private void Lose()
    {
        finishText.text = "You lost!";
    }

    private void Win()
    {
        finishText.text = "You won!";
    }

    private void Draw()
    {
        finishText.text = "Draw!";
    }
}