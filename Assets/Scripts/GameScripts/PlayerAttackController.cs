using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public bool IsAttacking { get; set; } = false;

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsAttacking = true;
        }
    }
}
