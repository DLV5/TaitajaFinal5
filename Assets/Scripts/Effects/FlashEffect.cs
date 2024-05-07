using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    [SerializeField] private Material _flashMaterial;

    private SpriteRenderer[] _spriteRenderers;
    private List<Material> _materialCopies = new List<Material>();

    private bool _isFlashing = false;

    private void Start()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materialCopies.Add(_spriteRenderers[i].material);
        }
    }

    public void Flash(float flashTime)
    {
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _spriteRenderers[i].material = _flashMaterial;
        }

        StartCoroutine(ReturnToNormal(flashTime));
    }

    private IEnumerator ReturnToNormal(float flashTime)
    {
        if (_isFlashing)  
            yield return null;

        _isFlashing = true;

        yield return new WaitForSeconds(flashTime);

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _spriteRenderers[i].material = _materialCopies[i];
        }

        _isFlashing = false;
    }
}
