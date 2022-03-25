using UnityEngine;

public class PathElement
{
    public readonly Vector2 From;
    public readonly Vector2 To;
    public readonly float FromSpeedModifier;
    public readonly float ToSpeedModifier;

    public PathElement(Vector2 from, Vector2 to, float fromSpeedModifier, float toSpeedModifier)
    {
        From = from;
        To = to;
        FromSpeedModifier = fromSpeedModifier;
        ToSpeedModifier = toSpeedModifier;
    }
}