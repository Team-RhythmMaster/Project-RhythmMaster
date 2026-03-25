using UnityEngine;
using Utils.ClassUtility;
using System.Collections.Generic;

// FFT 기반 노래 분석 및 Note 생성
public class BeatDetection : MonoBehaviour
{
    private NoteGenerator noteGenerator;

    // FFT 관련
    private float[] spectrum = new float[1024];     // 현재 프레임 주파수 데이터
    private float[] prevSpectrum = new float[1024]; // 이전 프레임 주파수 데이터

    // Flux 기록
    private Queue<float> fluxHistory = new Queue<float>(); // 과거 flux 저장
    private int historySize = 100;                         // 노이즈 안정화 평균 계산용 (80~120 추천)
    private float sensitivity = 3.0f; // 감도 (2.0~3.0 추천)

    private float lastBeatTime = 0f;  // Beat 중복 방지
    private float minInterval = 0.3f;// 노트의 최소 간격 (0.2~0.3 추천)

    private void Awake()
    {
        noteGenerator = FindFirstObjectByType<NoteGenerator>();
    }

    private void Update()
    {
        // 현재 스펙트럼 데이터 가져오기
        AudioManager.Instance.audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        // Flux 계산
        float flux = CalculateFlux();

        // Beat 판정
        if (IsBeat(flux))
        {
            float time = AudioManager.Instance.songTime;

            // 중복 방지
            if (time - lastBeatTime > minInterval)
            {
                OnBeatDetected(time);
                lastBeatTime = time;
            }
        }
    }

    // Spectral Flux 계산
    private float CalculateFlux()
    {
        float flux = 0f;

        for (int i = 0; i < spectrum.Length; i++)
        {
            // 현재 - 이전
            float diff = spectrum[i] - prevSpectrum[i];

            // 증가한 부분만 사용
            if (diff > 0)
            {
                flux += diff;
            }

            // 현재 값을 이전 배열에 저장
            prevSpectrum[i] = spectrum[i];
        }

        return flux;
    }

    // Beat 판정
    private bool IsBeat(float flux)
    {
        // 평균 계산
        float avg = 0f;

        foreach (var f in fluxHistory)
            avg += f;

        if (fluxHistory.Count > 0)
            avg /= fluxHistory.Count;

        // 현재 flux 저장
        fluxHistory.Enqueue(flux);

        if (fluxHistory.Count > historySize)
            fluxHistory.Dequeue();

        // 평균 대비 증가 여부 확인
        return flux > avg * sensitivity;
    }

    // Beat 발생 시
    private void OnBeatDetected(float time)
    {
        Note n = new Note();
        float spawnAheadTime = noteGenerator.GetSpawnAheadTime();

        // 지금 시간을 미래 시간으로 변환
        n.time = time + spawnAheadTime;
        n.lane = Random.Range(0, 2);

        // 롱노트 랜덤 생성
        if (Random.value < 0.2f)
        {
            n.isHold = true;
            n.endTime = n.time + Random.Range(0.5f, 1.5f);
        }

        // 추가 + 정렬 (실시간 생성이라 필수)
        noteGenerator.notes.Add(n);
        noteGenerator.notes.Sort((a, b) => a.time.CompareTo(b.time));
    }
}