using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 _positionOffset;
    private Camera _mainCamera;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    private void OnMouseDown()
    {
        _positionOffset = transform.position - GetMousePosition();
    }
    
    private void OnMouseDrag()
    {
        transform.position = GetMousePosition() + _positionOffset;
    }

    private Vector3 GetMousePosition()
    {
        var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        return mousePosition;
    }
}