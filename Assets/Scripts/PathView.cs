using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathView : MonoBehaviour
{
    private LineRenderer _line;
    private int _pointsAmount = 0;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
    }

    public void Adjust(Vector2 newPoint)
    {
        _pointsAmount++;
        if(_line.positionCount < _pointsAmount) 
            _line.positionCount = (int)((float) _line.positionCount * 1.5f);
        var positionCount = _line.positionCount;
        
        for (int i = _pointsAmount - 1; i < positionCount; i++)
        {
            _line.SetPosition(i, new Vector3(newPoint.x, newPoint.y, 1));
        }
    }

    public void DecreaseEnd()
    {
        _pointsAmount--;
        var p = _line.GetPosition(_pointsAmount);
        var linePositionCount = _line.positionCount;
        for (int i = _pointsAmount; i < linePositionCount; i++)
        {
            _line.SetPosition(i,p);
        }
    }
    
    public void DecreaseStart()
    {
        _pointsAmount--;

        for (int i = 0; i < _pointsAmount - 1; i++)
        {
            _line.SetPosition(i, _line.GetPosition(i + 1));
        }
        var linePositionCount = _line.positionCount;
        var p = _line.GetPosition(_pointsAmount - 2);
        for (int i = _pointsAmount - 1; i < linePositionCount; i++)
        {
            _line.SetPosition(i,p);
        }
    }

    public void Clear()
    {
        for (int i = 0; i < _line.positionCount; i++)
        {
            _line.SetPosition(i, Vector3.zero);
        }

        _pointsAmount = 0;
    }
}