using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ChromaticAberrationEffect : MonoBehaviour
{
    [SerializeField] private float _userTargerAbriviattionValue = 0.1f;
    [SerializeField] private float _abriviattionLerpSpeed = 0.05f;

    private ChromaticAberration _abriviattionEffect;

    private float _basicAbriviattionValue;
    private float _targetAbriviattionValue;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChangedEventHandelr;
    }

    private void Awake() => SetDepthOfField();

    private void Start()
    {
        SetBasicValue();
        UnfadeToAberration();
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChangedEventHandelr;
    }

    private void Update()
    {
        if (!IsReachedTargetValue())
        {
            BlurLerp();
        }
    }

    public void FadeToAberration() => _targetAbriviattionValue = _userTargerAbriviattionValue;
    public void FadeToAberration(float time)
    {
        _targetAbriviattionValue = _userTargerAbriviattionValue;
        StartCoroutine(FadeBackWithDelay(time));
    }

    public void UnfadeToAberration() => _targetAbriviattionValue = _basicAbriviattionValue;

    private void OnGameStateChangedEventHandelr(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                UnfadeToAberration();
                break;
            case GameState.Paused:
                FadeToAberration();
                break;
            case GameState.FirstPlayerWin:
                break;
            case GameState.SecondPlayerWin:
                break;
            case GameState.GameEnded:
                FadeToAberration();
                break;
        }
    }

    private IEnumerator FadeBackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnfadeToAberration();
    }

    private bool IsReachedTargetValue() => _targetAbriviattionValue == _abriviattionEffect.intensity.value;

    private void BlurLerp()
    {
        _abriviattionEffect.intensity.value = Mathf.Lerp(_abriviattionEffect.intensity.value, _targetAbriviattionValue, _abriviattionLerpSpeed);
    }

    private void SetDepthOfField()
    {
        PostProcessVolume blurVolume = FindFirstObjectByType<PostProcessVolume>();

        blurVolume.profile.TryGetSettings(out _abriviattionEffect);
    }

    private void SetBasicValue() => _basicAbriviattionValue = _abriviattionEffect.intensity.value;
}

