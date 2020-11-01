using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class GroundGrid : MonoBehaviour
{
    public static GroundGrid current;

    public List<GroundCell> Cells = new List<GroundCell>();

    private Vector3 _cellSize = new Vector3(1, 1, 1);
    private Vector3 _cellPivot = new Vector3(-0.5f, 0.0f, 0.5f);
    private Vector3 _groundSize;
    private Vector3 _groundTopLeft;
    private Vector3 _groundTopRight;

    private MeshRenderer _mesh;

    void Awake ()
    {
        current = this;
    }

    private void Start()
    {
        MeshRenderer mesh = GetComponent<MeshRenderer>();

        _mesh = mesh;
        _groundSize = _mesh.bounds.size;
        _groundTopLeft = new Vector3(-_groundSize.x, 0, _groundSize.z) / 2;
        _groundTopRight = new Vector3(_groundTopLeft.x * -1, 0, _groundTopLeft.z);

        GenerateGrid();
    }

    void GenerateGrid()
    {
        int horizontalCount = Mathf.RoundToInt(_groundSize.x / _cellSize.x);
        int verticalCount = Mathf.RoundToInt(_groundSize.z / _cellSize.z);

        Vector3 startPos = _groundTopLeft - _cellPivot;
        Vector3 prevPos = Vector3.zero;
        
        for(int cellCount = horizontalCount * verticalCount; cellCount > 0; cellCount--) {
            Vector3 pos;
            // IF IT IS FIRST CELL, START WITH startPos
            if(Cells.Count == 0)
            {
                pos = startPos;
            } else
            {
                // IF IT IS LAST IN THE ROW
                if(prevPos.x + _cellSize.x >= _groundTopRight.x)
                {
                    // JUMP TO NEXT LINE
                    pos = new Vector3(startPos.x, 0, prevPos.z - _cellSize.z);
                } else
                {
                    pos = new Vector3(prevPos.x + _cellSize.x, 0, prevPos.z);
                }
            }

            GroundCell cell = new GroundCell(_cellSize, pos);

            prevPos = pos;

            Cells.Add(cell);
        }

        Debug.Log(Cells.Count);
    }

    
    private void OnDrawGizmos()
    {

        for(int i = 0; i < Cells.Count; i++)
        {
            GroundCell cell = Cells[i];

            cell.Draw();
        }
    }
}
