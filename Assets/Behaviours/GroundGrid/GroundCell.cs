using System.Collections.Generic;
using UnityEngine;


public class GroundCell
{
    public Vector3 Size;
    public Vector3 Pos;
    public Vector3 Pivot;

    public List<GroundCell> Neighbours;

    private bool _shouldDraw; 

    public GroundCell(Vector3 size, Vector3 pos)
    {
        Size = size;
        Pos = pos;

        _shouldDraw = true;
    }

    public bool IsFreeToMove()
    {
        return true;
    }

    public void Draw()
    {

        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawCube(Pos, Size);
        Gizmos.color = Color.white;
    }
}
