using System.Collections.Generic;
using UnityEngine;


public class BombPool : MonoBehaviour
{
    private Bomb _bombPrefab;
    private List<Bomb> _bombs;
    private int _last = 0;
    private Transform _t;
    private GameManager _gameManager;
    private float _explosionDelay;

    
    public void InitBombPool(int amount, Bomb bombPrefab, GameManager gameManager, float explosionDelay)
    {
        _bombs = new List<Bomb>(amount);
        _bombPrefab = bombPrefab;
        _gameManager = gameManager;
        _explosionDelay = explosionDelay;
        _t = transform;
        for (int i = 0; i < amount; i++)
        {
            InstantiateBomb();
        }
    }

    public Bomb Get()
    {
        for (int i = 0; i < _bombs.Count; i++)
        {
            if (!_bombs[(_last + i) % _bombs.Count].gameObject.activeSelf)
            {
                _last = (_last + i) % _bombs.Count;
                return _bombs[_last];
            }
        }

        InstantiateBomb();
        _last = _bombs.Count - 1;
        return _bombs[_last];
    }

    private void InstantiateBomb()
    {
        var b = Instantiate(_bombPrefab, Vector3.zero, Quaternion.identity, _t);
        b.Init(_gameManager, _explosionDelay);
        b.gameObject.SetActive(false);
        _bombs.Add(b);
    }
}