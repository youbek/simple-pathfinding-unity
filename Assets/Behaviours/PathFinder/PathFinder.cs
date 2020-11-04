using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PathFinder
{
    // SIZE OF THE OBJECT THAT WANTS TO FIND A PATH
    private Vector3 _size;
    private List<Vector3> _openList = new List<Vector3>();
    private List<Vector3> _closedList = new List<Vector3>();
    private List<Vector3> _lastPath = new List<Vector3>();

    private LayerMask _obstacles;

    public PathFinder(Vector3 size, LayerMask obstacles)
    {
        _size = size;
        _obstacles = obstacles;
    }

    public void DrawVisuals()
    {

        foreach(Vector3 NodePos in _openList)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(NodePos, 0.1f);
            Gizmos.color = Color.white;
        }

        foreach(Vector3 NodePos in _closedList)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(NodePos, 0.1f);
            Gizmos.color = Color.white;
        }

        foreach (Vector3 NodePos in _lastPath)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(NodePos, 0.1f);
            Gizmos.color = Color.white;
        }
    }

    public List<Vector3> GetPaths(Vector3 startPos, GameObject target)
    {

        Node startingNode = new Node(_size, startPos, _obstacles);
        Node targetNode = new Node(_size, target.transform.position, _obstacles);

        _openList = new List<Vector3>();
        _closedList = new List<Vector3>();

        Dictionary<Vector3, (float g, float h, float f)> scores = new Dictionary<Vector3, (float g, float h, float f)>();
        Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>(); // KEY: WHERE NOW, VALUE: WHERE CAME FROM TO THAT KEY POSITION

        scores[startingNode.Pos] = (0, 0, 0);
        _openList.Add(startPos);

        int maxItiration = 1000;
        int currentItiration = 0;

        while(_openList.Count > 0 && currentItiration < maxItiration)
        {
            currentItiration++;
            Node current = new Node(_size, _openList[0], _obstacles);

            if (current != startingNode)
            {
                float lowestF = scores[current.Pos].f;

                foreach (Vector3 nodePos in _openList)
                {
                    float newFScore = scores[nodePos].f;

                    if (newFScore < lowestF)
                    { 
                        lowestF = newFScore;
                        current = new Node(_size, nodePos, _obstacles);
                    }
                }
            }

            if (current.bounds.Contains(targetNode.Pos))
            {
                _lastPath = ReconstructPath(cameFrom, current.Pos);
                return _lastPath;
            }

            if (!_closedList.Contains(current.Pos))
            {
                _closedList.Add(current.Pos);
            }

            _openList.Remove(current.Pos);

            foreach (Node neighbourNode in current.Neighbours)
            {
                if (_closedList.Contains(neighbourNode.Pos))
                {
                    continue;
                }

                float tempG = scores[current.Pos].g + Vector3.Distance(current.Pos, neighbourNode.Pos);
                float h = Vector3.Distance(neighbourNode.Pos, target.transform.position);
                float f = tempG + h;

                cameFrom[neighbourNode.Pos] = current.Pos;

                if (!scores.ContainsKey(neighbourNode.Pos))
                {
                    scores[neighbourNode.Pos] = (tempG, h, f);
                }
                else
                {
                    if (tempG < scores[neighbourNode.Pos].g)
                    {
                        scores[neighbourNode.Pos] = (tempG, h, f);
                    }
                }

                if (!_openList.Contains(neighbourNode.Pos) && neighbourNode.IsMovable)
                {
                    _openList.Add(neighbourNode.Pos);
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
