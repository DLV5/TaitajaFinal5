using UnityEngine;
using UnityEngine.UI;

public class ObjectHUD : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _easeHealthSlider;

    [SerializeField] private float _easeLerpSpeed = 0.05f;

    private bool _wasObjectHealed = false;

    private void OnEnable()
    {
        Health.OnHealthChanged += SetHealth;
    }

    private void Update()
    {
        if (Mathf.Abs(_healthSlider.value - _easeHealthSlider.value) < 0.001f)
        {
            _wasObjectHealed = false;
            return;
        }

        if (_wasObjectHealed)
        {
            _healthSlider.value = Mathf.Lerp(_healthSlider.value, _easeHealthSlider.value, _easeLerpSpeed);
        } else
        {
            _easeHealthSlider.value = Mathf.Lerp(_easeHealthSlider.value, _healthSlider.value, _easeLerpSpeed);
        }
    }

    private void OnDisable()
    {
        Health.OnHealthChanged -= SetHealth;
    }

    public void SetHUD(int maxHealth, int currentHealth)
    {
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = currentHealth;

        _easeHealthSlider.maxValue = maxHealth;
        _easeHealthSlider.value = currentHealth;
    }

    public void SetHealth(int health)
    {
        if(_healthSlider.value > health)
            _healthSlider.value = health;
        else
        {
            _easeHealthSlider.value = health;
            _wasObjectHealed = true;
        }
    }
}