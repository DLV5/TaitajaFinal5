using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    public event Action<int> OnDied;

    private static int _playerID = 0;

    private int _localPlayerID;

    public ObjectHUD ObjectHUD { get; set; }

    [SerializeField] private int _maxHealth;

    [SerializeField] private float _invincibilityTime = .5f;

    private int _currentHealth;

    public bool IsInvincible { get; set; }

    private bool _isDead = false;

    public int CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = value;

            ObjectHUD.SetHealth(_currentHealth);

            if (_currentHealth <= 0 && !IsInvincible)
            {
                OnDied?.Invoke(_localPlayerID);
            }
        }
    }

    private void Awake()
    {
        _localPlayerID = _playerID;
        ObjectHUD = FindObjectsByType<ObjectHUD>(FindObjectsSortMode.InstanceID)[_localPlayerID];
        _playerID++;
    }

    private void OnEnable()
    {
        OnDied += OnDiedHandler;
        GameManager.Instance.OnGameStateChanged += OnGameStateChangedEventHandler;
    }

    private void Start()
    {
        CurrentHealth = _maxHealth;
        ObjectHUD.SetHUD(_maxHealth, _currentHealth);
    }

    private void OnDisable()
    {
        OnDied -= OnDiedHandler;
        GameManager.Instance.OnGameStateChanged -= OnGameStateChangedEventHandler;
    }

    public void TakeDamage(int damage)
    {
        if (IsInvincible) return;

        CurrentHealth -= damage;

        StartCoroutine(EnterAndExitInvincibility());
    }

    private void OnGameStateChangedEventHandler(GameState state)
    {
        switch (state)
        {
            case GameState.NewTurnStarted:
                CurrentHealth = _maxHealth;
                ObjectHUD.SetHUD(_maxHealth, _currentHealth);
                IsInvincible = false;
                _isDead = false;
                break;
            case GameState.FirstPlayerWin:
                IsInvincible = true;
                break;
            case GameState.SecondPlayerWin:
                IsInvincible = true;
                break;
        }
    }

    private IEnumerator EnterAndExitInvincibility()
    {
        IsInvincible = true;
        yield return new WaitForSeconds(_invincibilityTime);
        IsInvincible = false;
    }

    private void OnDiedHandler(int id)
    {
        if (_isDead)
            return;

        _isDead = true;

        switch (id)
        {
            case 0:
                GameManager.Instance.ChangeGameState(GameState.SecondPlayerWin);
                break;
            case 1:
                GameManager.Instance.ChangeGameState(GameState.FirstPlayerWin);
                break;
        }
    }
}