using UnityEngine;
using Utils.ClassUtility;
using System.Collections.Generic;

// FFT 기반 노래 분석 및 Note 생성
public class BeatDetection : MonoBehaviour
{
    private NoteGenerator noteGenerator;
    private Queue<float> fluxHistory = new Queue<float>();
    private Dictionary<int, float> lastLaneTime = new Dictionary<int, float>(); // lane별 마지막 노트 시간 (겹침 방지)

    private float[] spectrum = new float[1024];
    private float[] prevSpectrum = new float[1024];

    private int historySize = 100;
    private float sensitivity = 3.5f;

    private float minInterval = 0.4f;  // 노트간 최소 간격
    private float lastBeatTime = 0f;

    private float bpm = 100f;
    private float beatInterval;

    private void Awake()
    {
        noteGenerator = FindFirstObjectByType<NoteGenerator>();
        beatInterval = 60f / bpm;
    }

    private void Start()
    {
        for (int i = 0; i < NoteManager.Instance.laneY.Length; i++)
            lastLaneTime[i] = -999f;
    }

    private void Update()
    {
        AudioManager.Instance.audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float flux = CalculateFlux();

        if (IsBeat(flux))
        {
            float time = AudioManager.Instance.songTime;

            if (time - lastBeatTime > minInterval)
            {
                CreateQuantizedNote(time);
                lastBeatTime = time;
            }
        }
    }

    float CalculateFlux()
    {
        float flux = 0f;

        for (int i = 0; i < spectrum.Length; i++)
        {
            float diff = spectrum[i] - prevSpectrum[i];

            if (diff > 0)
                flux += diff;

            prevSpectrum[i] = spectrum[i];
        }

        return flux;
    }

    bool IsBeat(float flux)
    {
        float avg = 0f;

        foreach (var f in fluxHistory)
            avg += f;

        if (fluxHistory.Count > 0)
            avg /= fluxHistory.Count;

        fluxHistory.Enqueue(flux);

        if (fluxHistory.Count > historySize)
            fluxHistory.Dequeue();

        return flux > avg * sensitivity;
    }

    // BPM 정렬 + 패턴 생성
    void CreateQuantizedNote(float time)
    {
        float spawnAheadTime = noteGenerator.GetSpawnAheadTime();
        float quantized = Mathf.Round(time / beatInterval) * beatInterval;  // BPM 정렬

        // 판정선 기준 최종 도착 시간
        float finalTime = quantized + spawnAheadTime;
        float songLength = AudioManager.Instance.Length;

        if (finalTime > songLength)
            return;

        int lane = Random.Range(0, NoteManager.Instance.laneY.Length);

        // 겹침 방지
        if (finalTime - lastLaneTime[lane] < beatInterval * 0.5f)
            return;

        Note n = new Note();
        n.time = finalTime;
        n.lane = lane;

        // 롱노트 생성 (박자 기반)
        if (Random.value < 0.2f)
        {
            n.isHold = true;

            // 박자 기반 길이
            float[] lengths = { 1.0f, 1.5f, 2f };
            float beatCount = lengths[Random.Range(0, lengths.Length)];
            n.endTime = n.time + beatInterval * beatCount;

            // 롱노트 끝도 겹침 방지
            lastLaneTime[lane] = n.endTime;

            if (n.endTime > songLength)
                return;
        }
        else
        {
            lastLaneTime[lane] = n.time;
        }

        noteGenerator.notes.Add(n);
        noteGenerator.notes.Sort((a, b) => a.time.CompareTo(b.time));
    }
}