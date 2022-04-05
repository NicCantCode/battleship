using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Cell : MonoBehaviour
{
    [SerializeField] private Markers markers;
    
    private GameManager _gameManager;
    private GameObject _cellMarker;
    private bool _hasMarker;
    private bool _hasShip;

    [SerializeField] private Vector2 gridLocation;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    private void SetMarker(MarkerType markerType)
    {
        var thisTransform = transform;
        var thisPosition = thisTransform.position;

        if (_hasMarker) return;
        
        switch (markerType)
        {
            case MarkerType.HIT:
                _cellMarker = Instantiate(markers.hitMarkerPrefab, new Vector3(thisPosition.x, thisPosition.y, -0.5f), Quaternion.identity, thisTransform);
                _hasMarker = true;
                break;
            case MarkerType.MISS:
                _cellMarker = Instantiate(markers.missMarkerPrefab, new Vector3(thisPosition.x, thisPosition.y, -0.5f), Quaternion.identity, thisTransform);
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

    public Vector2 GetGridLocation()
    {
        return gridLocation;
    }

    public void SetGridLocation(Vector2 gridLocation)
    {
        this.gridLocation = gridLocation;
    }

    public bool ContainsShip()
    {
        return _hasShip;
    }

    private void OnMouseDown()
    {
        switch (_hasMarker)
        {
            case false:
                SetMarker(_gameManager.GetMarkerType());
                return;
            case true:
                RemoveMarker();
                return;
        }
    }
}
