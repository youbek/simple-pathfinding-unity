using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 Size;
    public Vector3 Pos;
    public Bounds bounds;

    private LayerMask _obstacles;

    public Node(Vector3 size, Vector3 pos, LayerMask obstacles)
    {
        Size = new Vector3(size.x, 1, size.z);
        Pos = pos;
        _obstacles = obstacles;

        bounds = new Bounds(Pos, Size);
    }

    public List<Node> Neighbours
    {
        get {
            List<Node> neighbours = new List<Node>();

            Vector3 defaultChange = new Vector3(Size.x, 0, -Size.z);
            Vector3 change = defaultChange;

            // THERE WILL 8 NEIGHBOURS, DOING 9 ITIRATION IN ORDER TO SKIP SELF EASILY
            for (int i = 0; i < 9; i++)
            {
                Node neighbour = new Node(Size, Pos + change, _obstacles);

                // CHECK IF CURRENT INDEX THE END OF CURRENT ROW
                if ((i + 1) % 3 == 0 && i != 0)
                {
                    // JUMP TO NEXT ROW, AND BEGINNING OF COLUMN
                    change = new Vector3(defaultChange.x, 0, change.z + Size.z);
                }
                else
                {
                    change = new Vector3(change.x - Size.x, 0, change.z);
                }

                if (neighbour.Pos == Pos)
                {
                    continue;
                }

                neighbours.Add(neighbour);
            }

            return neighbours;
        }
    }

    public bool IsMovable
    {
        get
        {
            Collider[] hits = Physics.OverlapSphere(Pos, Size.x, _obstacles);

            if (hits.Length > 0)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
