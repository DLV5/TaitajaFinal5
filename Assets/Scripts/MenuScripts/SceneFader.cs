using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _fadeTime = 1.5f;

    private string _sceneName;

    public void FadeIn(string sceneName)
    {
        _sceneName = sceneName;
        StartCoroutine(TransferToScene());
    }

    //Invokes righ after animation is played
    public IEnumerator TransferToScene()
    {
        _animator.SetTrigger("FadeIn");

        yield return new WaitForSecondsRealtime(_fadeTime);

        SceneManager.LoadScene(_sceneName);
    }
}
