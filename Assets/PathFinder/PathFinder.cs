using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathFinder
{
  // SIZE OF THE OBJECT THAT WANTS TO FIND A PATH
  private Vector3 _size;
  private List<Vector3> _openList = new List<Vector3>();
  private List<Vector3> _closedList = new List<Vector3>();
  private List<Vector3> _lastPath = new List<Vector3>();
  private Dictionary<Vector3, (float g, float h, float f)> _scores = new Dictionary<Vector3, (float g, float h, float f)>();

  private LayerMask _obstacles;

  public PathFinder(Vector3 size, LayerMask obstacles)
  {
    _size = size;
    _obstacles = obstacles;
  }

  public void DrawVisuals(bool showScoreLabels)
  {

    foreach (Vector3 NodePos in _openList)
    {
      Node.DrawVisual(NodePos, _scores[NodePos], Color.green, showScoreLabels);
    }

    foreach (Vector3 NodePos in _closedList)
    {
      Node.DrawVisual(NodePos, _scores[NodePos], Color.red, showScoreLabels);
    }

    foreach (Vector3 NodePos in _lastPath)
    {
      Node.DrawVisual(NodePos, _scores[NodePos], Color.yellow, showScoreLabels);
    }
  }

  public List<Vector3> GetPaths(Vector3 startPos, GameObject targetGameObject)
  {
    // Needs refactoring
    return GetPaths(startPos, targetGameObject.transform.position);
  }

  public List<Vector3> GetPaths(Vector3 startPos, Vector3 targetPosition)
  {

    Node startingNode = new Node(_size, startPos, _obstacles);
    Node targetNode = new Node(_size, targetPosition, _obstacles);

    Debug.Log(targetNode.Pos);

    _openList = new List<Vector3>();
    _closedList = new List<Vector3>();

    _scores = new Dictionary<Vector3, (float g, float h, float f)>();
    Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>(); // KEY: WHERE NOW, VALUE: WHERE CAME FROM TO THAT KEY POSITION

    _scores[startingNode.Pos] = (0, 0, 0);
    _openList.Add(startPos);

    int limit = 2000;
    int i = 0;
    while (_openList.Count > 0)
    {
      i++;
      if (i > limit)
      {
        Debug.Log("Infinite LOOP: couldn't ReconstructPath");
        break;
      }

      Node current = new Node(_size, _openList[0], _obstacles);

      if (current != startingNode)
      {
        foreach (Vector3 nodePos in _openList)
        {
          // TRY TO PICK LOWEST F SCORE
          // IF SAME ROUNDED F SCORES THEN PICK ONE WITH LOWEST
          (float _, float lowestH, float lowestF) = _scores[current.Pos];
          (float _, float newH, float newF) = _scores[nodePos];

          int roundedLowestF = Mathf.FloorToInt(lowestF);
          int roundedNewF = Mathf.FloorToInt(newF);

          if (roundedNewF < roundedLowestF || roundedNewF == roundedLowestF && newH < lowestH)
          {
            current = new Node(_size, nodePos, _obstacles);
          }
        }
      }

      if (current.bounds.Contains(targetNode.Pos))
      {
        _lastPath = ReconstructPath(cameFrom, current.Pos);
        return _lastPath;
      }

      _closedList.Add(current.Pos);
      _openList.Remove(current.Pos);

      foreach (Node neighbourNode in current.Neighbours)
      {
        if (_closedList.Contains(neighbourNode.Pos))
        {
          continue;
        }

        // ROUNDING: BECAUSE MINOR NOT SO IMPORTANT
        float tempG = _scores[current.Pos].g + 1;
        float h = Vector3.Distance(neighbourNode.Pos, targetPosition);
        float f = tempG + h;

        if (!_openList.Contains(neighbourNode.Pos) && neighbourNode.IsMovable)
        {
          _openList.Add(neighbourNode.Pos);
        }

        if (!_scores.ContainsKey(neighbourNode.Pos))
        {
          _scores[neighbourNode.Pos] = (tempG, h, f);
        }
        else if (tempG >= _scores[neighbourNode.Pos].g)
        {
          continue;
        }

        cameFrom[neighbourNode.Pos] = current.Pos;
      }
    }

    Debug.Log("Couldn't find!");

    return new List<Vector3>();
  }

  private List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 currentKey)
  {
    List<Vector3> paths = new List<Vector3>();

    int limit = 2000;
    int i = 0;
    while (cameFrom.ContainsKey(currentKey))
    {
      i++;
      if (i > limit)
      {
        Debug.Log("Infinite LOOP: couldn't ReconstructPath");
        break;
      }

      paths.Add(cameFrom[currentKey]);
      currentKey = cameFrom[currentKey];
    }

    paths.Reverse();

    return paths;
  }
}
