using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _SFXSlider;

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    [SerializeField] private AudioData _audioData;

    private Dictionary<string, AudioClip> _audios = new Dictionary<string, AudioClip>();

    public const string MUSIC_VOLUME_KEY = "musicVolume";
    public const string SFX_VOLUME_KEY = "sfxVolume";

    private VolumeController _volumeController;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _volumeController = new VolumeController(_audioMixer, _musicSlider, _SFXSlider);
    }

    private void Start()
    {
        foreach (var obj in _audioData.AudioObjects)
        {
            string remake = obj.audioName.ToLower().Trim(); // clear (no whitespaces, no uppercase) version of AudioCLip name
            if (_audios.ContainsKey(remake))
                continue;
            _audios.Add(remake, obj.audioClip);
        }
    }

    /// <summary>
    /// NOT case or whitespaces sensitive param
    /// </summary>
    /// <param name="audioName"></param>
    private AudioClip GetAudio(string audioName)
    {
        string remake = audioName.ToLower().Trim(); // clear (no whitespaces, no uppercase) version of AudioCLip name
        if (_audios.TryGetValue(remake, out AudioClip clip))
        {
            return clip;
        }
        else
            return null;
    }
    public void PlaySFX(string audioName)
    {
        _sfxSource.clip = (GetAudio(audioName));
        if (_sfxSource.clip != null)
            _sfxSource.Play();
    }
    public void PlayMusic(string audioName)
    {
        _musicSource.clip = (GetAudio(audioName));
        if (_musicSource.clip != null)
            _musicSource.Play();
    }

    public void StopPlayMusic()
    {
        _musicSource.Stop();
    }

    public void ResumeMusic()
    {
        _musicSource.UnPause();
    }
}