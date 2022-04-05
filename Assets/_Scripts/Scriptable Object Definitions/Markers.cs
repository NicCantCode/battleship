using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Markers Definition", menuName = "SO/Markers Definition")]
public class Markers : SerializedScriptableObject
{
    public GameObject hitMarkerPrefab;
    public GameObject missMarkerPrefab;
}
