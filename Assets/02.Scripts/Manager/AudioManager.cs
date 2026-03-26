using UnityEngine;
using Utils.EnumType;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    public MusicState state = MusicState.Stop;
    public AudioSource audioSource;

    private double dspStartTime;      // 오디오가 실제로 재생되기 시작한 절대 시간
    public float audioOffset = 0.03f; // 오디오 싱크 보정값

    public float songTime
    {
        get
        {
            // 현재 DSP 시간 - 시작 DSP 시간
            return (float)(AudioSettings.dspTime - dspStartTime) + audioOffset;
        }
    }

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

    private void Start()
    {
        Play();
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

    // 플레이 시작
    public void Play()
    {
        state = MusicState.Playing;
        dspStartTime = AudioSettings.dspTime + 0.1f;
        audioSource.PlayScheduled(dspStartTime);
    }

    // 일시정지
    public void Pause()
    {
        state = MusicState.Paused;
        audioSource.Pause();
    }

    // 일시정지 해제
    public void UnPause()
    {
        state = MusicState.Unpaused;
        audioSource.UnPause();
    }

    // 종료
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