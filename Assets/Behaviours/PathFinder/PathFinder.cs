using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PathFinder
{
    private GroundGrid _grid;

    public PathFinder()
    {
        _grid = GroundGrid.current;
    }

    public List<Vector3> GetPaths(Vector3 startPos, Vector3 targetPos)
    {
        GroundCell startingCell = _grid.GetCellInGrid(startPos);
        GroundCell targetCell = _grid.GetCellInGrid(targetPos);

        if(startingCell == null)
        {
            Debug.Log("Exiting, not found starting cell!");
            return new List<Vector3>();
        }

        if(targetCell == null)
        {
            Debug.Log(targetPos);
            Debug.Log("Exiting, not found target cell!");
            return new List<Vector3>();
        }

        List<GroundCell> openList = new List<GroundCell>();
        List<GroundCell> closedList = new List<GroundCell>();

        Dictionary<Vector3, (float g, float h, float f)> scores = new Dictionary<Vector3, (float g, float h, float f)>();
        Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>(); // KEY: WHERE NOW, VALUE: WHERE CAME FROM TO THAT KEY POSITION

        openList.Add(startingCell);
        scores[startingCell.Pos] = (0, 0, 0);


        while(openList.Count > 0)
        {
            GroundCell current = openList[0];

            if(current != startingCell)
            {
                float lowestF = scores[current.Pos].f;

                foreach (GroundCell cell in openList)
                {
                    float newFScore = scores[cell.Pos].f;

                    if (newFScore < lowestF)
                    { 
                        lowestF = newFScore;
                        current = cell;
                    }
                }
            }

            if (current == targetCell)
            {
                return ReconstructPath(cameFrom, current.Pos);
            }

            if (!closedList.Contains(current))
            {
                closedList.Add(current);
            }

            openList.Remove(current);

            foreach (Vector3 neighbour in current.Neighbours)
            {
                GroundCell neighbourCell = _grid.GetCellInGrid(neighbour);

                if(neighbourCell == null)
                {
                    continue;
                }

                if (closedList.Contains(neighbourCell))
                {
                    continue;
                }

                float tempG = scores[current.Pos].g + Vector3.Distance(current.Pos, neighbourCell.Pos);
                float h = Vector3.Distance(neighbourCell.Pos, targetPos);
                float f = tempG + h;

                cameFrom[neighbourCell.Pos] = current.Pos;

                if (!scores.ContainsKey(neighbourCell.Pos))
                {
                    scores[neighbourCell.Pos] = (tempG, h, f);
                }
                else
                {
                    if (tempG < scores[neighbourCell.Pos].g)
                    {
                        scores[neighbourCell.Pos] = (tempG, h, f);
                    }
                }

                if (!openList.Contains(neighbourCell))
                {
                    openList.Add(neighbourCell);
                }
            }
        }

        Debug.Log("Couldn't find!");

        return new List<Vector3>();
    }

    private List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 currentKey)
    {
        List<Vector3> paths = new List<Vector3>();

        while(cameFrom.ContainsKey(currentKey))
        {
            paths.Add(cameFrom[currentKey]);
            
            currentKey = cameFrom[currentKey];
        }

        paths.Reverse();

        return paths;
    }
}
