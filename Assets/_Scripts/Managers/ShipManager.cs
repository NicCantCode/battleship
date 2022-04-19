using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipManager : MonoBehaviour
{
    [SerializeField] private List<Ship> spawnedShips;
    [SerializeField] private Button beginGameButton;
    
    private GameManager _gameManager;
    private BoardManager _boardManager;
    private LogManager _logManager;
    private AnimationManager _animationManager;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        _boardManager = GameObject.FindGameObjectWithTag("Board Manager").GetComponent<BoardManager>();
        _logManager = GameObject.FindGameObjectWithTag("Log Manager").GetComponent<LogManager>();
        _animationManager = GameObject.FindGameObjectWithTag("Animation Manager").GetComponent<AnimationManager>();
    }
    
    public void SetFinishedPlacingShips()
    {
        if (!AreAllShipsPlaced())
        {
            _logManager.LogMessage("Finish placing your ships!", Color.red);
            return;
        }
        
        _gameManager.SetDragEnabled(false);
        beginGameButton.interactable = false;
        beginGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Good\nLuck!";

        foreach (var cell in _boardManager.GetPlayerBoardGrid())
        {
            cell.GetComponent<Collider2D>().enabled = false;
        }
        
        _logManager.LogMessage("Ship placement has ended!", Color.gray);
        _boardManager.BuildTargetingBoard();
        _animationManager.GrowLogAnimation();
        
    }

    private bool AreAllShipsPlaced()
    {
        return spawnedShips.All(ship => ship.IsPlaced);
    }
}
