using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float inputActivationDistance;
    [SerializeField] private float cellSelectionDistance;
    [SerializeField] private float doubleTapTimeGap;

    private List<PathReader> _pigs;
    private MapGrid _grid;
    private BombManager _bombManager;

    private Camera _cam;

    private PathReader _activePig = null;

    private Cell _from;
    private Transform _fromT;
    
    private bool _touchActive = false;
    private float _doubleTapTimer = 0;

    public void Init(MapGrid grid, List<PathReader> pigs, BombManager bombManager)
    {
        _grid = grid;
        _pigs = pigs;
        _bombManager = bombManager;
        _cam = Camera.main;
    }

    private void Update()
    {
        _doubleTapTimer += Time.deltaTime;
        if (Input.touchCount == 0)
        {
            _activePig = null;
            _touchActive = false;
            return;
        }
        var touch = Input.touches[0];
        
        if (!_touchActive && touch.phase == TouchPhase.Began)
        {
            var touchPosWorld = _cam.ScreenToWorldPoint(touch.position);
            var pig = NearestPig(touchPosWorld);
            if (!EnoughNearToBecomeActive(pig, touchPosWorld))
            {
                return;
            }

            _touchActive = true;
            _activePig = pig;
            _from = _grid.ClosestPassableCell(pig.transform.position);
            _fromT = _from.transform;
            pig.Clear();

            if (doubleTapTimeGap >= _doubleTapTimer)
            {
                _bombManager.TryPlaceBomb(_from.Coords);
            }
            _doubleTapTimer = 0;
            return;
        }
        
        if (_touchActive && touch.phase == TouchPhase.Moved)
        {
            ProcessTouch(_cam.ScreenToWorldPoint(touch.position));
        }
    }

    private void ProcessTouch(Vector2 touchPosWorld)
    {
        if (!_pigs.Contains(_activePig))
        {
            _touchActive = false;
            return;
        }
        var closestCell = _grid.ClosestPassableCell(touchPosWorld);
        if (closestCell == _from) return;
        var closestCellTransform = closestCell.transform;
        var distance = Vector2.Distance(closestCellTransform.position, touchPosWorld);
        if (distance > cellSelectionDistance)
        {
            return;
        }

        if (!(_from.Coords.x == closestCell.Coords.x && Mathf.Abs(_from.Coords.y - closestCell.Coords.y) == 1
            || _from.Coords.y == closestCell.Coords.y && Mathf.Abs(_from.Coords.x - closestCell.Coords.x) == 1)) return;

        _activePig.Adjust(_fromT.position, closestCellTransform.position, _from.Info.SpeedModifier, closestCell.Info.SpeedModifier);
        _from = closestCell;
        _fromT = closestCellTransform;
    }

    private PathReader NearestPig(Vector3 touchPos)
    {
        var min = _pigs[0];
        var minDist = Vector2.Distance(_pigs[0].transform.position, touchPos);
        for (int i = 1; i < _pigs.Count; i++)
        {
            var dist = Vector2.Distance(_pigs[i].transform.position, touchPos);
            if (dist < minDist)
            {
                min = _pigs[i];
                minDist = dist;
            }
        }
        return min;
    }

    private bool EnoughNearToBecomeActive(PathReader pig, Vector3 touchPos)
    {
        var distance = Vector2.Distance(pig.transform.position, touchPos);
        return distance <= inputActivationDistance;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        var position = transform.position;
        Gizmos.DrawWireSphere(position - Vector3.left * 5f, inputActivationDistance);
        Gizmos.DrawWireSphere(position + Vector3.left * 5f, cellSelectionDistance);
    }
    #endif
}