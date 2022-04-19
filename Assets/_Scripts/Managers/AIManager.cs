using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private GameManager _gameManager;
    private BoardManager _boardManager;
    private LogManager _logManager;
    private Cell _searchOrigin;
    private Vector2 _directionToSearch;
    private Stack<Cell> _playerBoardStack;
    private bool _cycledThroughDirections;
    private bool _foundHitCell;
    private int _searchOffset;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        _boardManager = GameObject.FindGameObjectWithTag("Board Manager").GetComponent<BoardManager>();
        _logManager = GameObject.FindGameObjectWithTag("Log Manager").GetComponent<LogManager>();

        _foundHitCell = false;
        _cycledThroughDirections = false;
        _directionToSearch = Vector2.right;
        _searchOffset = 1;
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
        
        if (_gameManager.GetGameOverState()) yield break;
        
        _logManager.LogMessage("Enemy's turn!", Color.yellow);
        
        //yield return new WaitForSecondsRealtime(Random.Range(1,6));
        yield return new WaitForSecondsRealtime(0); // Debug

        DoAI(DifficultyManager.Instance.difficulty, out var guessedCell, out var result);
        
        _logManager.LogMessage($"Enemy {result} at {Utils.GridPositionToBattleshipPositionAsString(guessedCell.GetGridLocation())}!", result == MarkerType.HIT ? Color.red : Color.white);
        
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
    
    // This AI chooses a random cell each turn using a randomized stack of the board cells
    // This AI is hit ship aware and will progressively search around hit markers for the ship
    private void AIIntermediate(Stack<Cell> playerBoard, out Cell guessedCell, out MarkerType result)
    {
        if (_foundHitCell)
        {
            var cellInDirectionLocation = _searchOrigin.GetGridLocation() + _directionToSearch * _searchOffset;
            var cellInDirection = ShipUtils.GetCellByGridIndex(_boardManager.GetPlayerBoardGrid(), cellInDirectionLocation);

            while (!ShipUtils.CheckIfCellIsInsideBoardBounds(new Vector2(10, 10), cellInDirectionLocation))
            {
                cellInDirectionLocation = _searchOrigin.GetGridLocation() + _directionToSearch * _searchOffset;
                cellInDirection = ShipUtils.GetCellByGridIndex(_boardManager.GetPlayerBoardGrid(), cellInDirectionLocation);

                if (!ShipUtils.CheckIfCellIsInsideBoardBounds(new Vector2(10, 10), cellInDirectionLocation))
                {
                    _searchOffset = 1;
                    GetNextSearchDirection();

                    if (_cycledThroughDirections)
                    {
                        _foundHitCell = false;
                        _directionToSearch = Vector2.right;
                        _cycledThroughDirections = false;
                    }
                    
                    if (_directionToSearch == Vector2.down)
                    {
                        _cycledThroughDirections = true;
                    }
                }
            }
            
            if (!cellInDirection.GetHasMarker())
            {
                guessedCell = cellInDirection;
                result = guessedCell.GetShipState() ? MarkerType.HIT : MarkerType.MISS;
                guessedCell.SetMarker(result);
                RemoveGuessedCellFromStack(guessedCell);

                if (result == MarkerType.HIT)
                {
                    _searchOffset++;
                }
                else
                {
                    _searchOffset = 1;
                    GetNextSearchDirection();

                    if (_cycledThroughDirections)
                    {
                        _foundHitCell = false;
                        _directionToSearch = Vector2.right;
                        _cycledThroughDirections = false;
                    }
                    
                    if (_directionToSearch == Vector2.down)
                    {
                        _cycledThroughDirections = true;
                    }
                }
            }
            else
            {
                guessedCell = playerBoard.Pop();
                _searchOrigin = guessedCell;
                result = guessedCell.GetShipState() ? MarkerType.HIT : MarkerType.MISS;
                guessedCell.SetMarker(result);
                _foundHitCell = result == MarkerType.HIT;
            }
        }
        else
        {
            guessedCell = playerBoard.Pop();
            _searchOrigin = guessedCell;
            result = guessedCell.GetShipState() ? MarkerType.HIT : MarkerType.MISS;
            guessedCell.SetMarker(result);
            _foundHitCell = result == MarkerType.HIT;
        }
    }
    
    private void GetNextSearchDirection()
    {
        if (_directionToSearch == Vector2.right) _directionToSearch = Vector2.left;
        else if (_directionToSearch == Vector2.left) _directionToSearch = Vector2.up;
        else if (_directionToSearch == Vector2.up) _directionToSearch = Vector2.down;
        else if (_directionToSearch == Vector2.down) _directionToSearch = Vector2.right;
    }
    
    private void RemoveGuessedCellFromStack(Cell guessedCell)
    {
        var tempList = new List<Cell>(_playerBoardStack);

        foreach (var cell in tempList)
        {
            if (guessedCell.GetGridLocation() == cell.GetGridLocation())
            {
                tempList.Remove(cell);
                break;
            }
        }

        _playerBoardStack = Utils.ToStack(tempList);
    }
}