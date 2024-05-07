using System.Collections;
using System.Data;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _knockbackPower = 1f;

    [SerializeField] private float _actionsDelay = .1f;

    [SerializeField] private string _audioClipName;

    [SerializeField] private float _shakeTime = .1f;
    [SerializeField] private float _shakeStrenght = 1;

    [SerializeField] private GameObject _hitParticles;

    private CinemachineShake _shake;
    private ChromaticAberrationEffect _aberrationEffect;

    private void Awake()
    {
        _shake = GameObject.Find("Virtual Camera").GetComponent<CinemachineShake>();
        _aberrationEffect = GameObject.Find("BackgroundBlur").GetComponent<ChromaticAberrationEffect>();
    }

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

            FlashEffect flashEffect = collision.gameObject.GetComponent<FlashEffect>();

            if(flashEffect != null)
            {
                StartCoroutine(FlashWithDelay(flashEffect));
            }

            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();

            if (damagable != null)
            {
                StartCoroutine(ParticlesWithDelay(collision.gameObject.transform.position));
                StartCoroutine(ShakeWithDelay());
                StartCoroutine(DamageWithDelay(damagable));
            }

        }
    }

    private IEnumerator DamageWithDelay(IDamagable damagable)
    {
        yield return new WaitForSeconds(_actionsDelay);

        AudioManager.Instance.PlaySFX(_audioClipName);
        
        damagable.TakeDamage(_damage);
        gameObject.SetActive(false);
    }
    
    private IEnumerator KnockbackWithDelay(IKnockable knockable, Vector2 direction)
    {
        yield return new WaitForSeconds(_actionsDelay);
        knockable.Knockback(_knockbackPower, direction);
    }  
    
    private IEnumerator AberrationWithDelay()
    {
        yield return new WaitForSeconds(_actionsDelay);
        _aberrationEffect.FadeToAberration(_shakeTime);
    } 
    
    private IEnumerator ParticlesWithDelay(Vector2 position)
    {
        yield return new WaitForSeconds(_actionsDelay);
        Instantiate(_hitParticles, position, Quaternion.identity);
    }
    
    private IEnumerator ShakeWithDelay()
    {
        yield return new WaitForSeconds(_actionsDelay);
        _shake.ShakeCamera(_shakeStrenght, _shakeTime);
    }
    
    private IEnumerator FlashWithDelay(FlashEffect _flashEffect)
    {
        yield return new WaitForSeconds(_actionsDelay);
        _flashEffect.Flash(.25f);
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
