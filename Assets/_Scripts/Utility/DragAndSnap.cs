using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DragAndSnap : MonoBehaviour
{
    private Camera _mainCamera;
    private bool _isValidShipPosition;

    private Ship _thisShip;
    private Vector3 _originalShipPosition;

    private GameManager _gameManager;
    
    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        _mainCamera = Camera.main;
        _thisShip = GetComponent<Ship>();
        _originalShipPosition = transform.position;
        _gameManager.SetDragEnabled(true);
    }

    private void OnMouseDrag()
    {
        if (!_gameManager.GetDragEnabled()) return;
        
        ResetCells();
        
        _isValidShipPosition = _thisShip.IsValidShipPosition(_thisShip.OccupiedCells);

        transform.position = GetMousePosition();
        
        if (Input.GetKeyDown(KeyCode.R)) Rotate();
    }

    private void OnMouseUp()
    {
        if (!_gameManager.GetDragEnabled()) return;
        
        if (_isValidShipPosition)
        {
            ResetShipTransform(false);
            var cellPosition = _thisShip.OccupiedCells.Peek().transform.GetChild(0).position;
            var offset = (_thisShip.ShipSize - 1) * 0.25f;
            var shipRotationEulerAnglesZ = transform.rotation.eulerAngles.z;

            switch (shipRotationEulerAnglesZ)
            {
                case 90:
                case 270:
                    transform.position = new Vector3(cellPosition.x, cellPosition.y + offset, -0.5f);
                    break;
                case 0:
                case 180:
                    transform.position = new Vector3(cellPosition.x + offset, cellPosition.y, -0.5f);
                    break;
            }
            _thisShip.SetIsPlaced(true);
            MarkCellsOccupied(_thisShip.OccupiedCells.ToList());
        }
        else
        {
            ResetShipTransform(true);
            _thisShip.SetIsPlaced(false);
        }
        
    }

    private void ResetCells()
    {
        foreach (var cell in _thisShip.OccupiedCells.ToList())
        {
            if (cell.GetShipType() != _thisShip.ShipType) continue;
            cell.SetShipState(false, ShipType.NONE, null);
            _gameManager.RemoveShipBearingCell(cell);
        }
    }

    private void MarkCellsOccupied(List<Cell> occupiedCells)
    {
        foreach (var cell in occupiedCells)
        {
            cell.SetShipState(true, _thisShip.ShipType, _thisShip);
            _gameManager.AddShipBearingCell(cell);
        }
    }

    private void ResetShipTransform(bool rotationReset)
    {
        transform.position = _originalShipPosition;
        if (rotationReset) transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private Vector3 GetMousePosition()
    {
        var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = -0.5f;
        return mousePosition;
    }

    private void Rotate()
    {
        transform.Rotate(0, 0, 90, Space.World);
    }
}