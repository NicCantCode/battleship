using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private const float CellSize = 0.5f;
    private List<Cell> _boardGrid = new List<Cell>();
    
    public GameObject cellPrefab;

    // Debug
    public Vector2 gridSize = new Vector2(5, 5);
    
    private void Awake()
    {
        BuildBoard((int) gridSize.x, (int) gridSize.y, transform);
    }

    private void BuildBoard(int width, int height, Transform parent)
    {
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var cell = Instantiate(cellPrefab, new Vector3(i * CellSize, j * CellSize, 0) + transform.position, Quaternion.identity, parent);
                cell.name = "Cell " + "(" + i +"," + j +")";
                var cellComponent = cell.GetComponent<Cell>();
                cellComponent.SetGridLocation(new Vector2(i, j));
                _boardGrid.Add(cellComponent);
            }
        }
    }

    public Cell GetCellByGridIndex(Vector2 gridIndex)
    {
        return _boardGrid.Find(c => c.GetGridLocation() == gridIndex);
    }

    public void SetCellShipState(Vector2 cellIndex, bool hasShip)
    {
        GetCellByGridIndex(cellIndex).SetShipState(hasShip);
    }

    public bool GetCellShipState(Vector2 cellIndex)
    {
        return GetCellByGridIndex(cellIndex).GetShipState();
    }
}
