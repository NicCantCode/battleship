using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _canPlaceMarkers;
    [SerializeField] private bool _isDragEnabled;
    [SerializeField] private int playerShipsLeft;
    [SerializeField] private int enemyShipsLeft;
    [SerializeField] private List<Cell> _shipBearingCells;
    [SerializeField] private Button restartGameButton;

    [SerializeField] private bool _isGameOver;
    
    private LogManager _logManager;

    private void Awake()
    {
        _logManager = GameObject.FindGameObjectWithTag("Log Manager").GetComponent<LogManager>();
        
        restartGameButton.gameObject.SetActive(false);
        _shipBearingCells = new List<Cell>();
        _canPlaceMarkers = false;
        _isGameOver = false;
        playerShipsLeft = 5;
        enemyShipsLeft = 5;
    }

    private void Update()
    {
        if (_isGameOver) return;
        
        if (enemyShipsLeft == 0)
        {
            restartGameButton.gameObject.SetActive(true);
            _canPlaceMarkers = false;
            _logManager.LogMessage("You've won! :D", Color.green);
            _isGameOver = true;
            return;
        }
        
        if (playerShipsLeft == 0)
        {
            restartGameButton.gameObject.SetActive(true);
            _canPlaceMarkers = false;
            _logManager.LogMessage("The enemy has won! :(", Color.red);
            _isGameOver = true;
        }
    }

    public void SubtractFromEnemyShipPool()
    {
        enemyShipsLeft--;
    }

    public void SubtractFromPlayerShipPool()
    {
        playerShipsLeft--;
    }

    public bool GetGameOverState()
    {
        return _isGameOver;
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
