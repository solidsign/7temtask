using UnityEngine;

[RequireComponent(typeof(BombPool))]
public class BombManager : MonoBehaviour
{
    [SerializeField, Tooltip("In grid's cells")] private float explosionDistance;
    [SerializeField] private float bombPlaceCooldown;
    [SerializeField] private float explosionDelay;
    [SerializeField] private Bomb bombPrefab;
    private MapGrid _grid;
    private BombPool _bombs;
    private float _cooldownTimer;

    public float ExplosionDistance => explosionDistance;

    public void Init(MapGrid grid, GameManager gameManager)
    {
        _grid = grid;
        _bombs = GetComponent<BombPool>();
        _bombs.InitBombPool(6, bombPrefab, gameManager, explosionDelay);
    }
    
    public void TryPlaceBomb(Vector2Int gridCoords)
    {
        if (_cooldownTimer > 0f) return;
        var cell = _grid.GetCell(gridCoords);
        if (!cell.Info.Passable) return;

        var bomb = _bombs.Get();
        bomb.transform.position = cell.transform.position;
        bomb.Activate(gridCoords);
        _cooldownTimer = bombPlaceCooldown;
    }

    private void Update()
    {
        _cooldownTimer -= Time.deltaTime;
    }
}