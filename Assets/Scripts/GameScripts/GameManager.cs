using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChanged;

    [SerializeField] private int _maxTurns;

    private int _currentTurn;

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
        ChangeGameState(GameState.NewTurnStarted);
    }

    public void ChangeGameState(GameStateComponent state)
    {
        _currentState = state.GameState;

        OnGameStateChanged?.Invoke(_currentState);
    }
    
    public void ChangeGameState(GameState state)
    {
        _currentState = state;

        OnGameStateChanged?.Invoke(_currentState);
    }
}
