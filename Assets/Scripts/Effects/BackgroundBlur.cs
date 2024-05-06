using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BackgroundBlur : MonoBehaviour
{
    [SerializeField] private float _userTargerBlurValue = 0.1f;
    [SerializeField] private float _blurLerpSpeed = 0.05f;

    private DepthOfField _blurEffect;

    private float _basicBlurValue;
    private float _targetBlurValue;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChangedEventHandelr;
    }

    private void Awake() => SetDepthOfField();

    private void Start()
    {
        SetBasicValue();
        Unblur();
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

    public void Blur() => _targetBlurValue = _userTargerBlurValue;

    public void Unblur() => _targetBlurValue = _basicBlurValue;

    private void OnGameStateChangedEventHandelr(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                Unblur();
                break;
            case GameState.Paused:
                Blur();
                break;
            case GameState.Win:
                Blur();
                break;
            case GameState.Lost:
                Blur();
                break;
        }
    }

    private bool IsReachedTargetValue() => _targetBlurValue == _blurEffect.focusDistance.value;

    private void BlurLerp()
    {
        _blurEffect.focusDistance.value = Mathf.Lerp(_blurEffect.focusDistance.value, _targetBlurValue, _blurLerpSpeed);
    }

    private void SetDepthOfField()
    {
        PostProcessVolume blurVolume = FindFirstObjectByType<PostProcessVolume>();

        blurVolume.profile.TryGetSettings(out _blurEffect);
    }

    private void SetBasicValue() => _basicBlurValue = _blurEffect.focusDistance.value;
}
