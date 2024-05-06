using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPositions;

    [SerializeField] private GameObject[] _players;

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
                CreatePlayers();
                break;
        }
    }

    public void SpawnPlayer()
    {
        Instantiate(_players[0], _spawnPositions[0].position, Quaternion.identity);
    }

    private void CreatePlayers()
    {
        for (int i = 0; i < _players.Length; i++)
        {
            GameObject player = Instantiate(_players[i], _spawnPositions[i].position, Quaternion.identity);

            player.GetComponent<Health>().ObjectHUD = UIManager.Instance.HealthSliders[i];
        }
    }
}
