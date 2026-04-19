using UnityEngine;
using UnityEngine.UI;
using Utils.EnumType;
using Utils.ClassUtility;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance { get { return instance; } }

    private PlayerController playerController;
    private Text scoreText;

    // 점수 데이터
    public ScoreData scoreData;

    // 판정 범위
    public const float perfect = 0.05f;
    public const float great = 0.1f;
    public const float good = 0.15f;
    public const float bad = 0.2f;
    public const float miss = 0.25f;

    // 판정별 점수
    private const int perfectScore = 1000;
    private const int greatScore = 500;
    private const int goodScore = 250;
    private const int badScore = 100;
    private const int missScore = 5;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        scoreData = new ScoreData();
        playerController = FindAnyObjectByType<PlayerController>();
        scoreText = GameObject.Find("ScoreText").GetComponentInChildren<Text>();
    }

    // 초기화
    private void Init()
    {
        scoreData = new ScoreData();
    }

    // 판정 결과 처리
    public void Judgment(JudgeType _result, NoteObject _note)
    {
        switch (_result)
        {
            case JudgeType.Perfect:
                scoreData.combo++;
                scoreData.perfect++;
                scoreData.score += perfectScore * FeverManager.Instance.GetMultiplier();
                FeverManager.Instance.AddGauge(3f);
                break;
            case JudgeType.Great:
                scoreData.combo++;
                scoreData.great++;
                scoreData.score += greatScore * FeverManager.Instance.GetMultiplier();
                FeverManager.Instance.AddGauge(2f);
                break;
            case JudgeType.Good:
                scoreData.combo++;
                scoreData.good++;
                scoreData.score += goodScore * FeverManager.Instance.GetMultiplier();
                FeverManager.Instance.AddGauge(1f);
                break;
            case JudgeType.Bad:
                scoreData.maxCombo = (scoreData.maxCombo < scoreData.combo) ? scoreData.combo : scoreData.maxCombo;
                scoreData.combo = 0;
                scoreData.bad++;
                scoreData.score += badScore * FeverManager.Instance.GetMultiplier();
                FeverManager.Instance.AddGauge(-5f);
                break;
            case JudgeType.Miss:
                scoreData.maxCombo = (scoreData.maxCombo < scoreData.combo) ? scoreData.combo : scoreData.maxCombo;
                scoreData.combo = 0;
                scoreData.miss++;
                FeverManager.Instance.AddGauge(-10f);
                playerController.OnDamage(missScore);
                break;
        }

        JudgeManager.Instance.PlayFeedback(_result, _note.GetLane());
    }

    // 정확도 계산
    public float CalculateAccuracy()
    {
        if (RhythmPartManager.Instance.songData.notes.Count == 0) 
            return 0f;

        // 가증치합 = (판정별 개수 × 판정별 가중치)
        float weightedSum =
            scoreData.perfect * 1.0f +
            scoreData.great * 0.8f +
            scoreData.good * 0.5f +
            scoreData.bad * 0.2f +
            scoreData.miss * 0f;

        // 정확도 = (가중치합 / 전체 노트 수) × 100
        return (weightedSum / RhythmPartManager.Instance.songData.notes.Count) * 100f;
    }

   public void SetScore(string _score)
    {
        scoreText.text = _score;
    }
}