using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    public static event Action OnDied;

    private static int _playerID = 0;

    public ObjectHUD ObjectHUD { get; set; }

    [SerializeField] private int _maxHealth;

    [SerializeField] private float _invincibilityTime = .5f;

    private int _currentHealth;

    private bool _isInvincible;

    public int CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = value;

            ObjectHUD.SetHealth(_currentHealth);

            if (_currentHealth <= 0)
            {
                OnDied?.Invoke();
            }
        }
    }

    private void Awake()
    {
        ObjectHUD = FindObjectsByType<ObjectHUD>(FindObjectsSortMode.InstanceID)[_playerID];
        _playerID++;
    }

    private void Start()
    {
        CurrentHealth = _maxHealth;
        ObjectHUD.SetHUD(_maxHealth, _currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (_isInvincible) return;

        CurrentHealth -= damage;

        StartCoroutine(EnterAndExitInvincibility());
    }

    private IEnumerator EnterAndExitInvincibility()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(_invincibilityTime);
        _isInvincible = false;
    }
}