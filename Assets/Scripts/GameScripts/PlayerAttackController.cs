using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [field: SerializeField] public GameObject FirstAttackCollider {  get; set; }
    [field: SerializeField] public GameObject SecondAttackCollider { get; set; }
    [field: SerializeField] public GameObject ThirdAttackCollider { get; set; }

    public bool IsAttacking { get; set; } = false;

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsAttacking = true;
        }
    }
}
