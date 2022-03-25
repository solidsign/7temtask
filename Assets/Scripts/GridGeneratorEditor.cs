#if UNITY_EDITOR

using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridGenerator))]
class GridGeneratorEditor : Editor {
    private SerializedProperty _firstPoint;
    private SerializedProperty _gridSizeX;
    private SerializedProperty _gridSizeY;
    private SerializedProperty _horizontalOffsetX;
    private SerializedProperty _horizontalOffsetY;
    private SerializedProperty _verticalOffsetX;
    private SerializedProperty _verticalOffsetY;
    private Transform _gridParent;
    private CellInfo _emptyCellInfo;
    private CellInfo _stoneCellInfo;
    private Vector2[,] _grid;
    private SerializedProperty _cells;
    
    private void OnEnable()
    {
        _firstPoint = serializedObject.FindProperty("firstPoint");
        _gridSizeX = serializedObject.FindProperty("gridSizeX");
        _gridSizeY = serializedObject.FindProperty("gridSizeY");
        _horizontalOffsetX = serializedObject.FindProperty("horizontalOffsetX");
        _horizontalOffsetY = serializedObject.FindProperty("horizontalOffsetY");
        _verticalOffsetX = serializedObject.FindProperty("verticalOffsetX");
        _verticalOffsetY = serializedObject.FindProperty("verticalOffsetY");
        var mapGrid = (GridGenerator) target;
        _gridParent = mapGrid.GridParent;
        _emptyCellInfo = mapGrid.EmptyCellInfo;
        _stoneCellInfo = mapGrid.StoneCellInfo;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(GUILayout.Button("Delete grid")) DeleteLastGrid();
        if (GUILayout.Button("Create new grid"))
        {
            DeleteLastGrid();
            InstantiateGrid();
        }
    }

    private void DeleteLastGrid()
    {
        var c = _gridParent.GetComponentsInChildren<Transform>();
        foreach (var t in c)
        {
            if (t == _gridParent) continue;
            DestroyImmediate(t.gameObject);
        }
    }

    private void InitGridArray()
    {
        var gridSizeX = _gridSizeX.intValue;
        var gridSizeY = _gridSizeY.intValue;
        _grid = new Vector2[gridSizeX, gridSizeY];

        _grid[0, 0] = _firstPoint.vector2Value;
        var offset = new Vector2(_horizontalOffsetX.floatValue, _horizontalOffsetY.floatValue);
        for (int i = 1; i < gridSizeX; i++)
        {
            _grid[i, 0] = _grid[i - 1, 0] + offset;
        }

        offset.x = _verticalOffsetX.floatValue;
        offset.y = _verticalOffsetY.floatValue;
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 1; j < gridSizeY; j++)
            {
                _grid[i, j] = _grid[i, j - 1] + offset;
            }
        }
    }

    private void InstantiateGrid()
    {
        InitGridArray();
        var gridSizeX = _gridSizeX.intValue;
        var gridSizeY = _gridSizeY.intValue;
        var stoneSprite = ((GridGenerator) target).StoneSprite;
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                var g = new GameObject();
                var c = g.AddComponent<Cell>();
                if(i % 2 == 1 && j % 2 == 1)
                {
                    c.InitInfo(_stoneCellInfo, new Vector2Int(i,j));
                    g.AddComponent<SpriteRenderer>().sprite = stoneSprite;
                }
                else c.InitInfo(_emptyCellInfo, new Vector2Int(i,j));
                var t = g.transform;
                t.SetParent(_gridParent);
                t.position = _grid[i, j];
                g.name = new StringBuilder().Append(i).Append(';').Append(j).ToString();
            }
        }
        ((GridGenerator) target).SetupGrid();
    }
}

#endif