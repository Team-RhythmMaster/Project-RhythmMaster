using UnityEngine;
using Utils.ClassUtility;
using System.Collections.Generic;

// 비트 정보를 기반으로 실제 노트 패턴 생성
public class PatternGenerator : MonoBehaviour
{
    private NoteGenerator noteGenerator;
    private BeatAnalyzer beatAnalyzer;

    private Dictionary<int, float> lastLaneTime = new Dictionary<int, float>();
    private const float longNoteLengthMin = 1.0f;
    private const float longNoteLenghtMax = 5.0f;

    private float noteSpawnDelay = 4f; // Note 셍성 딜레이 시간
    private float longNoteRate = 0.2f; // longNote 생성 비율
    private float minInterval = 1.5f;  // 같은 레인에서 노트 간 최소 간격 (초 단위)

    private void Awake()
    {
        noteGenerator = FindAnyObjectByType<NoteGenerator>();
        beatAnalyzer = FindAnyObjectByType<BeatAnalyzer>();
    }

    private void Start()
    {
        for (int i = 0; i < NoteManager.Instance.laneY.Length; i++)
            lastLaneTime[i] = -999f;
    }

    private void Update()
    {
        // Beat 기반 Note 자동 생성
        if (beatAnalyzer.IsOnset())
        {
            float time = AudioManager.Instance.songTime;
            Generate(time);
        }
    }

    // 비트 발생 시 노트 생성
    public void Generate(float _rawTime)
    {
        int lane = Random.Range(0, NoteManager.Instance.laneY.Length);
        float time = beatAnalyzer.Quantize(_rawTime) + noteSpawnDelay;

        // 같은 레인에서 이전 노트 이후 최소 0.5비트 이상 간격이 있어야만 새 노트를 생성 (8분음표)
        if (time - lastLaneTime[lane] < beatAnalyzer.GetBeatInterval() * minInterval)
            return;

        NoteData note = new NoteData();
        note.time = time;
        note.lane = lane;

        if (Random.value < longNoteRate)
        {
            float length = beatAnalyzer.GetBeatInterval() * Random.Range(longNoteLengthMin, longNoteLenghtMax);
            note.endTime = time + length;
            lastLaneTime[lane] = note.endTime;
        }
        else
        {
            note.endTime = time;
            lastLaneTime[lane] = time;
        }

        noteGenerator.AddNote(note);
    }
}