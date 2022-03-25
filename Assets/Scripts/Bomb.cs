using System.Collections;

using UnityEngine;

public class Bomb : MonoBehaviour
{
    private GameManager _gameManager;
    private GameObject _self;
    private Vector2Int _coords;
    private float _explosionDelay;

    public void Init(GameManager gameManager, float explosionDelay)
    {
        _gameManager = gameManager;
        _explosionDelay = explosionDelay;
    }
    private void Awake()
    {
        _self = gameObject;
    }

    public void Activate(Vector2Int coords)
    {
        _coords = coords;
        _self.SetActive(true);
        StartCoroutine(TimeUpExplosion());
    }

    private IEnumerator TimeUpExplosion()
    {
        yield return new WaitForSeconds(_explosionDelay);
        Explode();
    }

    private void Explode()
    {
        _gameManager.ProcessExplosion(_coords);
        _self.SetActive(false);
    }
}