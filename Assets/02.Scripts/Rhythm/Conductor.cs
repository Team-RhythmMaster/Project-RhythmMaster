using UnityEngine;

// 음악 재샘 및 시간 관리 (모든 판정의 기준)
public class Conductor : MonoBehaviour
{
    public float songPosition; // 현재 재생 위치 (초)
    public float songPositionInBeats;

    public float bpm = 120f;
    private float secPerBeat;

    void Start()
    {
        secPerBeat = 60f / bpm;
        AudioManager.Instance.audioSource.Play();
    }

    void Update()
    {
        songPosition = AudioManager.Instance.audioSource.time;
        songPositionInBeats = songPosition / secPerBeat;
    }
}