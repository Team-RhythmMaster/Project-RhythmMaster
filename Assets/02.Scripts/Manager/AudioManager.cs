using UnityEngine;
using Utils.GameDefinitions;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    public MusicState state = MusicState.Stop;
    public AudioSource audioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Init();
    }

    private void Init()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public float Length
    {
        get
        {
            float len = 0f;
            if (audioSource.clip != null)
                len = audioSource.clip.length;
            return len;
        }
    }

    public float progressTime
    {
        get
        {
            float time = 0f;
            if (audioSource.clip != null)
                time = audioSource.time;
            return time;
        }
        set
        {
            if (audioSource.clip != null)
                audioSource.time = value;
        }
    }

    public void Play()
    {
        state = MusicState.Playing;
        audioSource.Play();
    }

    public void Pause()
    {
        state = MusicState.Paused;
        audioSource.Pause();
    }

    public void UnPause()
    {
        state = MusicState.Unpaused;
        audioSource.UnPause();
    }

    public void Stop()
    {
        state = MusicState.Stop;
        audioSource.Stop();
    }

    public void MovePosition(float time)
    {
        float currentTime = audioSource.time;

        currentTime += time;
        currentTime = Mathf.Clamp(currentTime, 0f, audioSource.clip.length - 0.0001f);

        audioSource.time = currentTime;
    }

    public void Insert(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    public float GetMilliSec()
    {
        return audioSource.time * 1000;
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }
}