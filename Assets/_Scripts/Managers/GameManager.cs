using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MarkerType _markerType;

    public void SetMarkerType(string markerType)
    {
        _markerType = markerType.Equals("HIT") ? MarkerType.HIT : MarkerType.MISS;
    }

    public MarkerType GetMarkerType()
    {
        return _markerType;
    }
}
