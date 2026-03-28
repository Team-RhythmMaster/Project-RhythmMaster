using UnityEngine;
using System.Collections.Generic;

// FFT 기반으로 음악에서 비트(Onset)를 검출
public class BeatAnalyzer : MonoBehaviour
{
    private Queue<float> fluxHistory = new Queue<float>(); // Flux 기록 (에너지 변화량)
    private float[] prevSpectrum = new float[1024];        // 이전 스펙트럼
    private float[] spectrum = new float[1024];            // 현재 스펙트럼

    private int historySize = 60;     // 평균 계산용 히스토리 버퍼의 최대 길이 (50~100 사이 안정적)
    private float sensitivity = 4.5f; // 민감도 (낮을수록 많이 감지)

    private float bpm = 120f;
    private float beatInterval; // 비트 간격

    private void Awake()
    {
        beatInterval = 60f / bpm;
    }

    // 현재 프레임이 비트인지 판단
    public bool IsOnset()
    {
        // 오디오 스펙트럼 데이터 추출
        AudioManager.Instance.audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        // 현재 프레임의 에너지 변화량 계산
        float flux = CalculateFlux();
        // 과거 평균 Flux 계산 (Adaptive Threshold)
        float avg = GetAverageFlux(flux);

        // 비트 판정 (노이즈 제거 포함)
        return (flux > avg * sensitivity) && (flux > 0.01f);
    }

    // 스펙트럼 변화량 계산
    private float CalculateFlux()
    {
        float flux = 0f;

        for (int i = 0; i < spectrum.Length; i++)
        {
            float diff = spectrum[i] - prevSpectrum[i];

            // 증가한 에너지만 반영 (onset 특징)
            if (diff > 0)
                flux += diff;

            // 이전 값 갱신
            prevSpectrum[i] = spectrum[i];
        }

        return flux;
    }

    // 현재 flux를 포함한 평균값 반환
    private float GetAverageFlux(float _currentFlux)
    {
        // 현재 flux를 히스토리에 추가
        fluxHistory.Enqueue(_currentFlux);

        if (fluxHistory.Count > historySize)
            fluxHistory.Dequeue();

        // 평균 계산
        float sum = 0f;
        foreach (var f in fluxHistory)
            sum += f;
        float avg = (fluxHistory.Count > 0) ? (sum / fluxHistory.Count) : 0f;

        // 너무 작은 값 방지 (노이즈 안정화)
        return Mathf.Max(avg, 0.0001f);
    }

    // 입력된 시간을 가장 가까운 박자에 맞게 보정
    public float Quantize(float _time)
    {
        return Mathf.Round(_time / beatInterval) * beatInterval;
    }

    // 한 박자 간격 반환
    public float GetBeatInterval()
    {
        return beatInterval;
    }
}