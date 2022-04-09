using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ShipOwner turnIndicator;
    [SerializeField] private bool _canPlaceMarkers;
    [SerializeField] private bool _isDragEnabled;
    [SerializeField] private List<Cell> _shipBearingCells;
    [SerializeField] private int playerShipsLeft;
    [SerializeField] private int enemyShipsLeft;

    private void Awake()
    {
        turnIndicator = ShipOwner.PLAYER;
        _shipBearingCells = new List<Cell>();
        _canPlaceMarkers = false;
        playerShipsLeft = 5;
        enemyShipsLeft = 5;
    }

    public void SetPlayerTurn()
    {
        turnIndicator = ShipOwner.PLAYER;
    }

    public void SetEnemyTurn()
    {
        turnIndicator = ShipOwner.ENEMY;
    }

    public void SubtractFromEnemyShipPool()
    {
        enemyShipsLeft--;
    }

    public void SubtractFromPlayerShipPool()
    {
        playerShipsLeft--;
    }

    public void TogglePlaceMarkerState()
    {
        _canPlaceMarkers = !_canPlaceMarkers;
    }

    public bool GetPlaceMarkerState()
    {
        return _canPlaceMarkers;
    }

    public void AddShipBearingCell(Cell cell)
    {
        _shipBearingCells.Add(cell);
    }
    
    public void RemoveShipBearingCell(Cell cell)
    {
        _shipBearingCells.Remove(cell);
    }

    public List<Cell> GetShipBearingCells()
    {
        return _shipBearingCells;
    }

    public bool GetDragEnabled()
    {
        return _isDragEnabled;
    }

    public void SetDragEnabled(bool state)
    {
        _isDragEnabled = state;
    }

    public void SetCanPlaceMarkers(bool state)
    {
        _canPlaceMarkers = state;
    }
}
