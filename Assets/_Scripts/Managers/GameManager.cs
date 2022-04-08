using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MarkerType _markerType;
    private bool _canPlaceMarkers;
    private List<Cell> _shipBearingCells;

    private void Awake()
    {
        _shipBearingCells = new List<Cell>();
        _canPlaceMarkers = false;
    }

    public void SetMarkerType(MarkerType markerType)
    {
        _markerType = markerType;
    }

    public MarkerType GetMarkerType()
    {
        return _markerType;
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
}
