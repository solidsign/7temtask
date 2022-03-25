#if UNITY_EDITOR

using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid setup")]
    [SerializeField] private Vector2 firstPoint;
    [SerializeField] [Min(0)] private int gridSizeX;
    [SerializeField] [Min(0)] private int gridSizeY;
    [SerializeField] [Min(0f)] private float horizontalOffsetX;
    [SerializeField] [Min(0f)] private float horizontalOffsetY;
    [SerializeField] [Min(0f)] private float verticalOffsetX;
    [SerializeField] [Min(0f)] private float verticalOffsetY;

    [Header("Initialization")]
    [SerializeField] private CellInfo emptyCellInfo;
    [SerializeField] private CellInfo stoneCellInfo;
    [SerializeField] private Sprite stoneSprite;
    [SerializeField] private Transform gridParent;

    public Transform GridParent => gridParent;

    public CellInfo EmptyCellInfo => emptyCellInfo;
    public CellInfo StoneCellInfo => stoneCellInfo;

    public Sprite StoneSprite => stoneSprite;

    public void SetupGrid()
    {
        gridParent.GetComponent<MapGrid>().Init(
            firstPoint,
            new Vector2Int(gridSizeX, gridSizeY),
            new Vector2(horizontalOffsetX, horizontalOffsetY),
            new Vector2(verticalOffsetX, verticalOffsetY)
            );
    }

    private void OnDrawGizmosSelected()
    {
        var pointsRow = new Vector3[gridSizeX];
        pointsRow[0] = new Vector3(firstPoint.x, firstPoint.y, 0);
        Gizmos.DrawWireSphere(pointsRow[0], 0.1f);
        var offset = new Vector3(horizontalOffsetX, horizontalOffsetY, 0);
        for (int i = 1; i < gridSizeX; i++)
        {
            pointsRow[i] = pointsRow[i - 1] + offset;
            Gizmos.DrawWireSphere(pointsRow[i], 0.1f);
        }

        offset.x = verticalOffsetX;
        offset.y = verticalOffsetY;
        for (int j = 1; j < gridSizeY; j++)
        {
            for (int i = 0; i < gridSizeX; i++)
            {
                pointsRow[i] += offset;
                Gizmos.DrawWireSphere(pointsRow[i], 0.1f);
            }
        }
    }
}
#endif