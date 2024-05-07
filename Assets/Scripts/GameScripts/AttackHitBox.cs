using System.Collections;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _knockbackPower = 1f;

    [SerializeField] private float _actionsDelay = .1f;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChangedEventHandelr;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChangedEventHandelr;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            IKnockable knockable = collision.gameObject.GetComponent<IKnockable>();

            if(knockable != null)
            {
                var direction = gameObject.transform.parent.transform.localScale.x > 0 
                    ? gameObject.transform.right : -gameObject.transform.right;

                StartCoroutine(KnockbackWithDelay(knockable, direction));
            }

            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();

            if (damagable != null)
            {
                StartCoroutine(DamageWithDelay(damagable));
            }

        }
    }

    private IEnumerator DamageWithDelay(IDamagable damagable)
    {
        yield return new WaitForSeconds(_actionsDelay);
        damagable.TakeDamage(_damage);
        gameObject.SetActive(false);
    }
    
    private IEnumerator KnockbackWithDelay(IKnockable knockable, Vector2 direction)
    {
        Debug.LogWarning("Knocking back");
        yield return new WaitForSeconds(_actionsDelay);
        knockable.Knockback(_knockbackPower, direction);
    }

    private void OnGameStateChangedEventHandelr(GameState state)
    {
        switch (state)
        {
            case GameState.NewTurnStarted:
                gameObject.SetActive(false);
                break;
        }
    }
}
