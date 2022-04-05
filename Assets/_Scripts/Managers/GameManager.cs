using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MarkerType _markerType;
    private bool _canPlaceMarkers;

    public void SetMarkerType(string markerType)
    {
        _markerType = markerType.Equals("HIT") ? MarkerType.HIT : MarkerType.MISS; // Clean this up later
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
}
