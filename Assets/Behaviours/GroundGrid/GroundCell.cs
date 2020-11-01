using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class GroundCell
{
    public Vector3 Size;
    public Vector3 Pos;
    public Vector3 Pivot;

    // ALL CELLS ARE SAVED IN KeyValuePars<KEY = Vector3, VALUE = GroundCell>. LOOK AT GroundGrid.cs
    public List<Vector3> Neighbours;

    public GroundCell(Vector3 size, Vector3 pos)
    {
        Size = size;
        Pos = pos;

        Neighbours = CalculateNeighbours();

        Debug.Log(Neighbours.Count);
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

        foreach (Vector3 neighbour in Neighbours)
        {
            Gizmos.color = new Color(255, 255, 0);
            Gizmos.DrawLine(Pos, neighbour);
            Gizmos.color = Color.white;
        }
    }

    public List<Vector3> CalculateNeighbours()
    {
        List<Vector3> neighbours = new List<Vector3>();

        Vector3 defaultChange = new Vector3(Size.x, 0, -Size.z);
        Vector3 change = defaultChange;

        // THERE WILL 8 NEIGHBOURS, DOING 9 ITIRATION IN ORDER TO SKIP SELF EASILY
        for(int i = 0; i < 9; i++)
        {
            Vector3 neighbour = Pos + change;

            // CHECK IF CURRENT INDEX THE END OF CURRENT ROW
            if ((i + 1) % 3 == 0 && i != 0)
            {
                // JUMP TO NEXT ROW, AND BEGINNING OF COLUMN
                change = new Vector3(defaultChange.x, 0, change.z + Size.z);
            } else
            {
                change = new Vector3(change.x - Size.x, 0, change.z);
            }

            if(neighbour == Pos)
            {
                continue;
            }

            neighbours.Add(neighbour);
        }

        return neighbours;
    }
}
