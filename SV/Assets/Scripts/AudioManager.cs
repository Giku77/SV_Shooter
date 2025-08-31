using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip mainClip;
    private AudioSource audioSource;
    //public AudioSource musicSource;
    //public AudioSource sfxSource;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = mainClip;
        audioSource.volume = musicVolume;
        audioSource.Play();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (audioSource != null)
        {
            audioSource.volume = musicVolume;
        }
    }
    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        //if (sfxSource != null)
        //{
        //    sfxSource.volume = sfxVolume;
        //}
    }
}
