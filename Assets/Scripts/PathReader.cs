using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathView))]
public class PathReader : MonoBehaviour
{
    [SerializeField] private float speed;
    private LinkedList<PathElement> _waypoints;
    private PathElement _current = null;
    private bool _finished = true;
    private Transform _t;
    private float _lerpTime;
    private PathView _view;

    private void Awake()
    {
        _t = transform;
        _waypoints = new LinkedList<PathElement>();
        _view = GetComponent<PathView>();
    }

    private void Update()
    {
        if (_finished)
        {
            if (_waypoints.Count == 0) return;
            _finished = false;
            _current = _waypoints.First.Value;
            _waypoints.RemoveFirst();
            _view.DecreaseStart();
            _view.DecreaseStart();
            _lerpTime = 0;
        }

        _t.position = Vector3.Lerp(_current.From, _current.To, _lerpTime);
        
        if (_lerpTime < 0.5f) _lerpTime += Time.deltaTime * speed * _current.FromSpeedModifier;
        else _lerpTime += Time.deltaTime * speed * _current.ToSpeedModifier;
        
        if (1f - _lerpTime <= Single.Epsilon) _finished = true;
    }

    public void Adjust(Vector2 from, Vector2 to, float fromSpeedModifier, float toSpeedModifier)
    {
        if (_waypoints.Count > 0 && Vector2.Distance(_waypoints.Last.Value.From, to) < Single.Epsilon)
        {
            if (Vector2.Distance(_waypoints.Last.Value.To, from) < Single.Epsilon)
            {
                _waypoints.RemoveLast();
                _view.DecreaseEnd();
                _view.DecreaseEnd();
                return;
            }
        }
        _waypoints.AddLast(new PathElement(from, to, fromSpeedModifier, toSpeedModifier));
        _view.Adjust(from);
        _view.Adjust(to);
    }

    public void Clear()
    {
        _waypoints.Clear();
        _view.Clear();
    }
}