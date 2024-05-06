using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChangedEventHandler;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChangedEventHandler;
    }

    private void OnDestroy() => Unpause();

    private void OnGameStateChangedEventHandler(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                Unpause();
                break;
            case GameState.Paused:
                Pause();
                break;
            case GameState.Win:
                Pause();
                break;
            case GameState.Lost:
                Pause();
                break;
        }
    }

    private void Pause() => Time.timeScale = 0;
    private void Unpause() => Time.timeScale = 1;
}
