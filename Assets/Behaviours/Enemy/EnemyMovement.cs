using UnityEngine;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    public GameObject target;
    public LayerMask obstacles;
    public float speed = 10.0f;

    public bool showPathsGizmos;

    private PathFinder _pathFinder;

    private List<Vector3> _paths = new List<Vector3>();
    private int _currentPath = 0;

    private void Start()
    {
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        Vector3 size = new Vector3(capsuleCollider.radius * 2, capsuleCollider.height, capsuleCollider.radius * 2);
        _pathFinder = new PathFinder(size, obstacles);
    }

    void Update()
    {
        _pathFinder.GetPaths(transform.position, target);
        if (Input.GetMouseButtonDown(0))
        {
            _paths = _pathFinder.GetPaths(transform.position, target);
        }

        if (_currentPath == _paths.Count - 1 || _paths.Count == 0)
        {
            _paths = new List<Vector3>();
            _currentPath = 0;
        }
        else if (_paths[_currentPath] == transform.position)
        {
            _currentPath++;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _paths[_currentPath], speed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        if(_pathFinder != null && showPathsGizmos)
            _pathFinder.DrawVisuals(true);
    }
}
