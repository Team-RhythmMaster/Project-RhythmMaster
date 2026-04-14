using UnityEngine;
using Utils.EnumType;
using Utils.ClassUtility;
using System.Collections;

public class RhythmPartManager : MonoBehaviour
{
    private static RhythmPartManager instance;
    public static RhythmPartManager Instance {get { return instance; } }

    private NoteGenerator noteGenerator;
    private GameObject gameEndPhanel;

    public SaveData saveData;
    public SongDataSO songData;

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
        gameEndPhanel = GameObject.Find("MainCanvas").transform.GetChild(1).gameObject;
    }

    // 게임 종료 처리
    public void EndGame()
    {
        isGameEnded = true;
        noteGenerator.isSpawning = false;
        AudioManager.Instance.Stop();

        ShowResultUI();
        SaveResult(songData.id, JudgeManager.Instance.scoreData.score, JudgeManager.Instance.scoreData.combo, JudgeManager.Instance.CalculateAccuracy());
    }

    public void SaveResult(int _songID, int _score, int _combo, float _accuracy)
    {
        var record = saveData.songRecords
            .Find(r => r.songID == _songID);

        if (record == null)
        {
            record = new SongRecord();
            record.songID = _songID;
           saveData.songRecords.Add(record);
        }

        RankType newRank = (RankType)System.Enum.Parse(typeof(RankType), GetRank(_accuracy));

        if (newRank > record.rank)
            record.rank = newRank;

        if (_score > record.maxScore)
            record.maxScore = _score;

        if (_combo > record.maxCombo)
            record.maxCombo = _combo;

        DataManager.Instance.SaveJson(saveData, "SongDatabase");
    }

    public string GetRank(float _accuracy)
    {
        if (_accuracy >= 0.98f) return "S";
        if (_accuracy >= 0.95f) return "A";
        if (_accuracy >= 0.90f) return "B";
        if (_accuracy >= 0.80f) return "C";
        if (_accuracy >= 0.70f) return "D";
        return "F";
    }

    public void ShowResultUI()
    {
        gameEndPhanel.SetActive(true);
    }
}