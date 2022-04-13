using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private GameManager _gameManager;
    private LogManager _logManager;
    
    private Stack<Cell> _playerBoardStack;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        _logManager = GameObject.FindGameObjectWithTag("Log Manager").GetComponent<LogManager>();
    }

    public void SetPlayerBoardStack(Stack<Cell> playerBoardStack)
    {
        _playerBoardStack = playerBoardStack;
    }

    public void EnemyTurnStart()
    {
        StartCoroutine(EnemyTurnStartCoroutine());
    }

    IEnumerator EnemyTurnStartCoroutine()
    {
        _gameManager.SetCanPlaceMarkers(false);
        _logManager.LogMessage("Enemy's turn!", Color.yellow);
        
        yield return new WaitForSecondsRealtime(0); // Debug
        
        DoAI(Difficulty.SIMPLE, out var guessedCell, out var result); // DO AI
        
        _logManager.LogMessage($"Enemy {result} at ({guessedCell.GetGridLocation().x}, {guessedCell.GetGridLocation().y})!", result == MarkerType.HIT ? Color.red : Color.white);
        
        if (result == MarkerType.HIT) guessedCell.GetOccupyingShip().MarkShipAsHit();
        
        _gameManager.SetCanPlaceMarkers(true);
        _logManager.LogMessage("Player's turn!", Color.green);
    }

    private void DoAI(Difficulty gameDifficulty, out Cell guessedCell, out MarkerType result)
    {
        switch (gameDifficulty)
        {
            case Difficulty.SIMPLE:
                AISimple(_playerBoardStack, out var guessedCellSimple, out var resultSimple);
                guessedCell = guessedCellSimple;
                result = resultSimple;
                break;
            case Difficulty.INTERMEDIATE:
                AIIntermediate(_playerBoardStack, out var guessedCellIntermediate, out var resultIntermediate);
                guessedCell = guessedCellIntermediate;
                result = resultIntermediate;
                break;
            default:
                guessedCell = null;
                result = MarkerType.MISS;
                break;
        }
    }
    
    // This AI chooses a random cell each turn using a randomized stack of the board cells
    private void AISimple(Stack<Cell> playerBoard, out Cell guessedCell, out MarkerType result)
    {
        guessedCell = playerBoard.Pop();
        result = guessedCell.GetShipState() ? MarkerType.HIT : MarkerType.MISS;
        guessedCell.SetMarker(result);
    }

    // TODO: Come back to AIIntermediate later with a fresh mind.
    // This AI chooses a random cell each turn until a ship is hit, than chooses cells in the vicinity of the hit ship
    private void AIIntermediate(Stack<Cell> playerBoard, out Cell guessedCell, out MarkerType result)
    {
        guessedCell = null;
        result = MarkerType.HIT;
    }
}
