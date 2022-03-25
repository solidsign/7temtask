using UnityEngine;
[CreateAssetMenu(fileName = "New Cell", menuName = "New Cell")]
public class CellInfo : ScriptableObject
{
    [SerializeField] private bool passable;
    [SerializeField] private float speedModifier;


    public bool Passable => passable;
    public float SpeedModifier => speedModifier;

    public Vector2Int Coords { get; set; }
}