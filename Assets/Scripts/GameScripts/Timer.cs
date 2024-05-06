using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private int _maxTurnTime = 30;

    private bool _shoudCount = false;

    private int _currentTime;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChangedEventHandelr;
    }

    private void Start()
    {
        StartCoroutine(TimerCoroutine());
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
                ResetTimer();
                _shoudCount = true;
                break;
            case GameState.Playing:
                _shoudCount = true;
                break;
            case GameState.Paused:
                _shoudCount = false;
                break;
        }
    }

    private void ResetTimer()
    {
        _currentTime = _maxTurnTime;
        _shoudCount = false;
    }

    private IEnumerator TimerCoroutine()
    {
        while (true)
        {
            if (_shoudCount)
            {
                _currentTime -= 1;
                UIManager.Instance.LevelTimer.text = _currentTime.ToString();
            }

            yield return new WaitForSeconds(1);
        }
    }
}
