using UnityEngine;
using Utils.EnumType;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    private NoteGenerator noteGenerator;

    public AudioSource audioSource;
    public MusicState state = MusicState.Stop;

    public float noteTravelTime = 2.0f;
    public double songStartDspTime;// 오디오가 실제로 재생되기 시작한 절대 시간

    public float songTime
    {
        get
        {
            return (float)(AudioSettings.dspTime - songStartDspTime);
        }
    }

    public bool IsSongFinished()
    {
        return songTime >= audioSource.clip.length;
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

    private void Init()
    {
        noteGenerator = FindAnyObjectByType<NoteGenerator>();
        audioSource = GetComponent<AudioSource>();
    }

    // 플레이
    public void Play()
    {
        state = MusicState.Playing;

        songStartDspTime = AudioSettings.dspTime + noteTravelTime;
        audioSource.PlayScheduled(songStartDspTime);
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