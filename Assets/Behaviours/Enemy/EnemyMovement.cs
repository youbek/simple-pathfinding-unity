using UnityEngine;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float speed = 10.0f;

    private PathFinder _pathFinder;

    private List<Vector3> _paths = new List<Vector3>();
    private int _currentPath = 0;

    private void Start()
    {
        _pathFinder = new PathFinder();


    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _paths = _pathFinder.GetPaths(transform.position, target.position);
        }

        if(_currentPath == _paths.Count - 1 || _paths.Count == 0)
        {
            _paths = new List<Vector3>();
            _currentPath = 0;
        } else if(_paths[_currentPath] == transform.position)
        {
            _currentPath++;
        } else
        {
            transform.position = Vector3.MoveTowards(transform.position, _paths[_currentPath], speed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        foreach(Vector3 path in _paths)
        {
            Gizmos.DrawCube(path, new Vector3(1.0f, 1.0f, 1.0f));
        }
    }
}
