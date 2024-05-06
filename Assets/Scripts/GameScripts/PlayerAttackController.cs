using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] public GameObject _firstAttackCollider;
    [SerializeField] public GameObject _secondAttackCollider;
    [SerializeField] public GameObject _thirdAttackCollider;

    public bool IsAttacking { get; set; } = false;

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsAttacking = true;
        }
    }
}
