using System.Collections;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _damageDelay = .1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();

            if (damagable != null)
            {
                StartCoroutine(DamageWithDelay(damagable));
            }
        }
    }

    private IEnumerator DamageWithDelay(IDamagable damagable)
    {
        yield return new WaitForSeconds(_damageDelay);
        damagable.TakeDamage(_damage);
        gameObject.SetActive(false);
    }
}
