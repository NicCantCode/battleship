using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Cell : MonoBehaviour
{
    [SerializeField] private Markers markers;
    
    private GameManager _gameManager;
    private BoardManager _boardManager;
    private GameObject _cellMarker;
    private ShipType _occupiedShipType;
    private Ship _occupiedShip;
    private bool _hasMarker;
    private bool _hasShip;
    private bool _isInPlayerBoard;

    [SerializeField] private Vector2 _gridLocation;

    private void Awake()
    {
        _occupiedShipType = ShipType.NONE;
        _gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        _boardManager = GameObject.FindGameObjectWithTag("Board Manager").GetComponent<BoardManager>();
    }

    public void DebugSetMarker()
    {
        SetMarker(MarkerType.HIT);
    }

    public void SetMarker(MarkerType markerType)
    {
        var thisTransform = transform;
        var thisPosition = thisTransform.position;

        if (_hasMarker) return;
        
        switch (markerType)
        {
            case MarkerType.HIT:
                _cellMarker = Instantiate(markers.hitMarkerPrefab, new Vector3(thisPosition.x, thisPosition.y, -0.7f), Quaternion.identity, thisTransform);
                _hasMarker = true;
                break;
            case MarkerType.MISS:
                _cellMarker = Instantiate(markers.missMarkerPrefab, new Vector3(thisPosition.x, thisPosition.y, -0.7f), Quaternion.identity, thisTransform);
                _hasMarker = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(markerType), markerType, "Marker ID Doesn't Exist.");
        }
    }

    private void RemoveMarker()
    {
        Destroy(_cellMarker);
        _hasMarker = false;
    }

    public bool GetHasMarker()
    {
        return _hasMarker;
    }

    public Vector2 GetGridLocation()
    {
        return _gridLocation;
    }

    public void SetGridLocation(Vector2 gridLocation)
    {
        _gridLocation = gridLocation;
    }

    public bool GetShipState()
    {
        return _hasShip;
    }

    public ShipType GetShipType()
    {
        return _occupiedShipType;
    }

    public void SetShipState(bool hasShip, ShipType shipType, Ship ship)
    {
        _hasShip = hasShip;
        _occupiedShipType = shipType;
        _occupiedShip = ship;
    }

    public void SetIsInPlayerBoard(bool state)
    {
        _isInPlayerBoard = state;
    }

    private void OnMouseDown()
    {
        if (!_gameManager.GetPlaceMarkerState() && _isInPlayerBoard) return;
        
        switch (_hasMarker)
        {
            case false:
                var result = _hasShip ? MarkerType.HIT : MarkerType.MISS;
                SetMarker(result);
                
                if (_hasShip) _occupiedShip.MarkShipAsHit();
                
                print($"Player {result} at ({_gridLocation.x}, {_gridLocation.y})!");
                
                _boardManager.EnemyTurnStart();
                return;
            case true:
                return;
        }
    }

    public Ship GetOccupyingShip()
    {
        return _occupiedShip;
    }

    public void SetOccupyingShip(Ship ship)
    {
        _occupiedShip = ship;
    }
}
