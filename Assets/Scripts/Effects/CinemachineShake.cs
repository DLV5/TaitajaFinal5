using Cinemachine;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private float shakeTimer;

    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensivity, float time)
    {
        var _cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensivity;

        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                var _cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }
}
