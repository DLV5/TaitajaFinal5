using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private float _speed = 5f;

    private Rigidbody2D _rigidbody2D;

    private bool _isFacingRight = true;

    public float Direction { get; private set; } = 0;

    public bool CanMove { get; set; } = false;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChangedEventHandelr;
    }

    private void Update()
    {
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

        _rigidbody2D.velocity = new Vector2(Direction * _speed, _rigidbody2D.velocity.y);
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
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!CanMove)
            return;

        if (context.performed && IsGrounded())
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpPower);
        }

        if (context.canceled && _rigidbody2D.velocity.y > 0f)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * 0.5f);
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
}
