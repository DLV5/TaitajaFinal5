using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject _loseScreen;
    private GameObject _winScreen;
    private GameObject _pauseScreen;

    private void Awake()
    {
        _loseScreen = GameObject.Find("LoseScreen");
        _winScreen = GameObject.Find("WinScreen");
        _pauseScreen = GameObject.Find("PauseScreen");
    }

    private void Start()
    {
        HideScreen(_loseScreen);
        HideScreen(_winScreen);
        HideScreen(_pauseScreen);
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
            case GameState.Playing:
                HideScreen(_pauseScreen); 
                break;
            case GameState.Paused:
                ShowScreen(_pauseScreen);
                break;
            case GameState.Win:
                ShowScreen(_winScreen);
                break;
            case GameState.Lost:
                ShowScreen(_loseScreen);
                break;
        }
    }

    private void ShowScreen(GameObject screen) => screen.SetActive(true);
    private void HideScreen(GameObject screen) => screen.SetActive(false);
}
