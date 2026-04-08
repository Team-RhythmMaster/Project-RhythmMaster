using UnityEngine;
using UnityEngine.UI;
using Utils.EnumType;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    private Slider songTimeSlider;

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

    private void Update()
    {
        songTimeSlider.value = songTime / audioSource.clip.length;
    }

    private void Init()
    {
        songTimeSlider = GameObject.Find("SongTimeSlider").GetComponent<Slider>();
        audioSource = GetComponent<AudioSource>();
    }

    // 플레이
    public void Play()
    {
        state = MusicState.Playing;

        songStartDspTime = AudioSettings.dspTime + noteTravelTime;
        audioSource.PlayScheduled(songStartDspTime);
        songTimeSlider.value = 0;
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