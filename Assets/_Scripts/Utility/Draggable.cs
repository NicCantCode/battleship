using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Camera _mainCamera;
    private bool _isDraggable;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        ToggleDrag();
    }

    private void OnMouseDrag()
    {
        if (!_isDraggable) return;
        
        transform.position = GetMousePosition();
        
        if (Input.GetKeyDown(KeyCode.R)) Rotate();
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

    public void ToggleDrag()
    {
        _isDraggable = !_isDraggable;
    }
}