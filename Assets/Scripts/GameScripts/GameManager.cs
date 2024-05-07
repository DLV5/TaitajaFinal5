using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChanged;

    [SerializeField] private int _maxWins = 2;

    public int FirstPlayerWins { get; private set; } = 0;
    public int SecondPlayerWins {get; private set; } = 0;

    private GameState _currentState;

    public GameState CurrentState { get => _currentState; }

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ChangeGameState(GameState.WaitingPlayersToConnect);
    }

    public void ChangeGameState(GameStateComponent state)
    {
        if(_currentState == GameState.FirstPlayerWin)
        {
            FirstPlayerWins++;
        } 
        
        if(_currentState == GameState.SecondPlayerWin)
        {
            SecondPlayerWins++;
        } 

        if (FirstPlayerWins == _maxWins || SecondPlayerWins == _maxWins)
        {
            ChangeGameState(GameState.GameEnded);
        }

        _currentState = state.GameState;

        OnGameStateChanged?.Invoke(_currentState);

    }
    
    public void ChangeGameState(GameState state)
    {
        if (_currentState == GameState.FirstPlayerWin)
        {
            FirstPlayerWins++;
        }

        if (_currentState == GameState.SecondPlayerWin)
        {
            SecondPlayerWins++;
        }

        if (FirstPlayerWins == _maxWins || SecondPlayerWins == _maxWins)
        {
            ChangeGameState(GameState.GameEnded);
        }

        _currentState = state;

        OnGameStateChanged?.Invoke(_currentState);
    }
}
