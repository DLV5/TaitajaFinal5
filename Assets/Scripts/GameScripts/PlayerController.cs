using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IKnockable
{
    public event Action EnterruptAttack;

    [SerializeField] private GameObject _defendingEffect;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private float _speed = 5f;

    private Health _health;

    private bool _isFacingRight = true;

    public float Direction { get; private set; } = 0;

    public Rigidbody2D Rigidbody2D { get; set; }
    public bool CanMove { get; set; } = false;
    public bool IsDefending { get; set; } = false;

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChangedEventHandelr;
    }

    private void Update()
    {
        if (!CanMove)
            return;

        if (!_isFacingRight && Direction > 0f)
        {
            Flip();
        }
        else if (_isFacingRight && Direction < 0f)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (!CanMove)
            return;

        Rigidbody2D.velocity = new Vector2(Direction * _speed, Rigidbody2D.velocity.y);
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
                CanMove = false;
                break;
            case GameState.Playing:
                CanMove = true;
                break;
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        var stateToChange = 
            GameManager.Instance.CurrentState == GameState.Paused ? GameState.Playing : GameState.Paused;
        GameManager.Instance.ChangeGameState(stateToChange);
    }

    public void Move(InputAction.CallbackContext context)
    {
        Direction = context.ReadValue<float>();

        if (Mathf.Abs(Direction) > 0.2)
        {
            EnterruptAttack?.Invoke();
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!CanMove)
            return;

        if (context.performed && IsGrounded())
        {
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, _jumpPower);
        }

        if (context.canceled && Rigidbody2D.velocity.y > 0f)
        {
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Rigidbody2D.velocity.y * 0.5f);
        }
    }

    public void Defend(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.CurrentState != GameState.Playing)
            return;

        if (!CanMove && !IsDefending)
            return;

        if(context.started)
        {
            Debug.Log("Defending");
            CanMove = false;
            IsDefending = true;

            Rigidbody2D.velocity = Vector2.zero;

            _health.IsInvincible = true;

            _defendingEffect.SetActive(true);
        }

        if (context.canceled)
        {
            Debug.Log("Not defending");
            CanMove = true;
            IsDefending = false;
            _health.IsInvincible = false;

            _defendingEffect.SetActive(false);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Knockback(float power, Vector2 direction)
    {
        StartCoroutine(KnockbackCoroutine(power, direction));
    }

    private IEnumerator KnockbackCoroutine(float power, Vector2 direction)
    {
        CanMove = false;
        Rigidbody2D.velocity = direction * power;
        yield return new WaitForSeconds(.1f);
        CanMove = true;
    }
}
