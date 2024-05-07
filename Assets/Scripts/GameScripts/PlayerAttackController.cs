using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private PlayerController _controller;

    [field: SerializeField] public GameObject FirstAttackCollider {  get; set; }
    [field: SerializeField] public GameObject SecondAttackCollider { get; set; }
    [field: SerializeField] public GameObject ThirdAttackCollider { get; set; }

    public bool IsAttacking { get; set; } = false;

    public void Attack(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.CurrentState != GameState.Playing)
            return;

        if (_controller.IsDefending == true)
            return;

        if (context.started)
        {
            IsAttacking = true;
        }
    }
}
