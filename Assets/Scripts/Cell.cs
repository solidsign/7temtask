using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private CellInfo info;

    [SerializeField] [HideInInspector] private Vector2Int coords;

    public CellInfo Info => info;

    public Vector2Int Coords => coords;

    public void InitInfo(CellInfo cellInfo, Vector2Int coords)
    {
        info = cellInfo;
        this.coords = coords;
    }
}