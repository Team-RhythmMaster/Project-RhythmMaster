using UnityEngine;
using Utils.ClassUtility;
using Utils.GameDefinitions;

// Note 생성 및 오디오 분석
public class NoteGenerator : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform spawnPoint;

    public Difficulty difficulty = Difficulty.Normal;

    private float[] spectrum = new float[512];
    private float lastEnergy = 0f;
    private float spawnCooldown = 0f;

    private void Update()
    {
        AnalyzeAudio();
    }

    // FFT 기반 주파수 분석
    private void AnalyzeAudio()
    {
        // GetSpectrumData를 사용하여 오디오의 주파수 스펙트럼을 분석
        AudioManager.Instance.audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
        float energy = 0f;

        // 저음~중음 영역 (킥/비트 감지)
        for (int i = 0; i < 50; i++)
        {
            energy += spectrum[i];
        }

        // 난이도별 민감도
        float threshold = GetThreshold();

        // 이전 에너지 대비 증가령 / 피크 감지 (비트 순간)
        if (energy > lastEnergy * threshold && spawnCooldown <= 0f)
        {
            // 노트 생성
            SpawnNote();
            spawnCooldown = GetCooldown();
        }

        // 이전값 저장
        lastEnergy = energy;
        // 쿨타임 감소
        spawnCooldown -= Time.deltaTime;
    }

    // Note 생성
    private void SpawnNote()
    {
        GameObject obj = Instantiate(notePrefab, Vector3.zero, Quaternion.identity);
        NoteObject noteObj = obj.GetComponent<NoteObject>();

        float currentTime = AudioManager.Instance.audioSource.time;

        float speed = 5f;
        float rightEdgeX = 15f;

        float spawnAheadTime = rightEdgeX / speed;

        noteObj.note = new Note();
        noteObj.note.time = currentTime + spawnAheadTime;
    }

    private float GetThreshold()
    {
        switch (difficulty)
        {
            case Difficulty.Easy: 
                return 1.5f;
            case Difficulty.Normal: 
                return 1.3f;
            case Difficulty.Hard: 
                return 1.1f;
        }

        return 1.3f;
    }

    private float GetCooldown()
    {
        switch (difficulty)
        {
            case Difficulty.Easy: 
                return 0.5f;
            case Difficulty.Normal: 
                return 0.3f;
            case Difficulty.Hard: 
                return 0.15f;
        }

        return 0.3f;
    }
}
