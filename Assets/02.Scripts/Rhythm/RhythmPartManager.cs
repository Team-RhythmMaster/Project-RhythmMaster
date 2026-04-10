using UnityEngine;
using Utils.ClassUtility;
using System.Collections;

public class RhythmPartManager : MonoBehaviour
{
    private static RhythmPartManager instance;
    public static RhythmPartManager Instance {get { return instance; } }

    public SaveData saveData;
    public SongDataSO songData;
    public SongRecord songRecord;

    private NoteGenerator noteGenerator;
    private bool isGameEnded = false;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        Init();
    }

    private IEnumerator Start()
    {
        // 모든 초기화 끝날 때까지 대기
        noteGenerator.Init();
        AudioManager.Instance.Play();

        yield return new WaitForSeconds(AudioManager.Instance.noteTravelTime);

        noteGenerator.isSpawning = true;
    }

    private void Update()
    {
        if (isGameEnded || !noteGenerator.IsAllNotesActivated() || !NoteManager.Instance.IsAllNotesCleared())
            return;

        EndGame();
    }

    private void Init()
    {
        noteGenerator = FindAnyObjectByType<NoteGenerator>();
    }

    // 게임 종료 처리
    void EndGame()
    {
        isGameEnded = true;
        ShowResultUI();
        SaveResult(songRecord.songID, JudgeManager.Instance.scoreData.score, JudgeManager.Instance.scoreData.combo);
    }

    public void SaveResult(string songID, int score, int combo)
    {
        var record = saveData.songRecords
            .Find(r => r.songID == songID);

        if (record == null)
        {
            record = new SongRecord();
            record.songID = songID;
           saveData.songRecords.Add(record);
        }

        if (score > record.maxScore)
            record.maxScore = score;

        if (combo > record.maxCombo)
            record.maxCombo = combo;

        DataManager.Instance.SaveJson(saveData, "Song");
    }

    public void ShowResultUI()
    {

    }

    public string SetRank()
    {
        float accuracy = JudgeManager.Instance.ReturnScore();

        if (accuracy >= 0.98f) return "S";
        if (accuracy >= 0.95f) return "A";
        if (accuracy >= 0.90f) return "B";
        if (accuracy >= 0.80f) return "C";
        if (accuracy >= 0.70f) return "D";
        return "F";
    }
}