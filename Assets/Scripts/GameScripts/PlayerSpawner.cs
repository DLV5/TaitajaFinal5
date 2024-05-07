using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public static event Action OnBothPlayersConnected;

    [SerializeField] private Transform[] _spawnPositions;

    [SerializeField] private GameObject[] _playersPrefabs;

    private List<PlayerInput> _spawnedPlayers = new List<PlayerInput>();

    private PlayerInputManager _playerInputManager;

    private void Awake()
    {
        _playerInputManager = GameObject.Find("PlayerInputManager").GetComponent<PlayerInputManager>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChangedEventHandelr;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChangedEventHandelr;
    }

    private void OnGameStateChangedEventHandelr(GameState state)
    {
        switch (state)
        {
            case GameState.NewTurnStarted:
                SetPlayersPositionToSpawnPoints();
                break;
        }
    }

    public void SpawnPlayer(PlayerInput player)
    {
        _spawnedPlayers.Add(player);

        player.transform.position = _spawnPositions[_spawnedPlayers.Count - 1].position;

        if(_spawnedPlayers.Count < _playersPrefabs.Length)
        {
            _playerInputManager.playerPrefab = _playersPrefabs[_spawnedPlayers.Count];
        }
        else
        {
            OnBothPlayersConnected?.Invoke();

            AudioManager.Instance.PlayMusic("BackgroudBattle");
        }
    }

    private void SetPlayersPositionToSpawnPoints()
    {
        for (int i = 0; i < _spawnedPlayers.Count; i++)
        {
            _spawnedPlayers[i].gameObject.transform.position = _spawnPositions[i].position;
        }
    }
}
