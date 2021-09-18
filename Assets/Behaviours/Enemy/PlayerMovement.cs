using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
  public GameObject target;
  public LayerMask obstacles;
  public LayerMask ground;
  public float speed = 10.0f;
  public bool showPathsGizmos;

  private PathFinder _pathFinder;
  private List<Vector3> _paths = new List<Vector3>();
  private int _nextPathIndex = 0;
  private Camera _camera;

  private void Start()
  {
    _camera = Camera.main;
    CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
    Vector3 size = new Vector3(capsuleCollider.radius * 2, capsuleCollider.height, capsuleCollider.radius * 2);
    _pathFinder = new PathFinder(size, obstacles);
  }

  void Update()
  {
    MovePlayerToClickedPosition();
  }

  private void MovePlayerToClickedPosition()
  {
    if (GetMouseClickPosition(out Vector3 clickedPosition))
    {
      _paths = _pathFinder.GetPaths(transform.position, new Vector3(clickedPosition.x, transform.position.y, clickedPosition.z));
      _nextPathIndex = 0;
    }

    if (_paths.Count > 0)
    {
      bool isReachedTheNextPath = _paths[_nextPathIndex].x == transform.position.x && _paths[_nextPathIndex].z == transform.position.z;
      if (isReachedTheNextPath)
      {
        _nextPathIndex++;

        bool isReachedTheEnd = _paths.Count == _nextPathIndex;
        if (isReachedTheEnd)
        {
          _nextPathIndex = 0;
          _paths = new List<Vector3>();
          return;
        }
      }

      transform.position = Vector3.MoveTowards(transform.position, _paths[_nextPathIndex], speed * Time.deltaTime);
    }
  }

  private bool GetMouseClickPosition(out Vector3 position)
  {
    if (Input.GetMouseButtonDown(1))
    {
      Ray castPoint = _camera.ScreenPointToRay(Input.mousePosition);

      RaycastHit hit;

      if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, ground))
      {
        position = hit.point;
        return true;
      }
    }

    position = new Vector3();
    return false;
  }

  private void OnDrawGizmos()
  {
    if (_pathFinder != null && showPathsGizmos)
      _pathFinder.DrawVisuals(true);
  }
}
