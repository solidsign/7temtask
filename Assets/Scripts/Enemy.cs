using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Transform _t;
    private float _lerpTime;
    private PathElement _current;
    private Vector2Int _newGridPosition;
    
    public Vector2Int GridPosition { get; private set; }

    public bool FinishedWalk { get; private set; }


    public void SetNewDestination(Vector2 from, Vector2 to, float fromSpeedModifier, float toSpeedModifier, Vector2Int newGridPosition)
    {
        _current = new PathElement(from, to, fromSpeedModifier, toSpeedModifier);
        FinishedWalk = false;
        _newGridPosition = newGridPosition;
    }

    private void Awake()
    {
        _t = transform;
    }

    private void Update()
    {
        if(FinishedWalk) return;
        
        _t.position = Vector3.Lerp(_current.From, _current.To, _lerpTime);
        
        if (_lerpTime < 0.5f) _lerpTime += Time.deltaTime * speed * _current.FromSpeedModifier;
        else _lerpTime += Time.deltaTime * speed * _current.ToSpeedModifier;
        
        if (1f - _lerpTime <= Single.Epsilon)
        {
            FinishedWalk = true;
            GridPosition = _newGridPosition;
            _lerpTime = 0;
        }
    }
}