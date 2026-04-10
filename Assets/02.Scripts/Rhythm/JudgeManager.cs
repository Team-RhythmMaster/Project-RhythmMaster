using UnityEngine;
using UnityEngine.UI;
using Utils.EnumType;
using Utils.ClassUtility;
using System.Collections.Generic;

public class JudgeManager : MonoBehaviour
{
    private static JudgeManager instance;
    public static JudgeManager Instance { get { return instance; } }

    // UI 오브젝트풀
    public JudgmentUI judgePrefab;
    private Transform parentTransform;
    private Queue<JudgmentUI> judgePool = new Queue<JudgmentUI>();

    private PlayerController playerController;
    private Text scoreText;

    // lane별 판정 UI 생성 위치
    private Vector2[] lanePositions = { new Vector2(-480.0f, 290.0f), new Vector2(-480.0f, -35.0f) };

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

        Init();
    }

    private void Init()
    {
        parentTransform = GameObject.Find("JudgePool").transform;
        playerController = FindAnyObjectByType<PlayerController>();
        scoreText = GameObject.Find("ScoreText").GetComponentInChildren<Text>();
    }

    public JudgmentUI JudgmentUIGet()
    {
        if (judgePool.Count > 0)
        {
            var obj = judgePool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        return Instantiate(judgePrefab, parentTransform);
    }

    public void JudgementUIReturn(JudgmentUI _obj)
    {
        _obj.gameObject.SetActive(false);
        judgePool.Enqueue(_obj);
    }

    // 판정 결과 처리
    public void Judgment(JudgeType _result, NoteObject _note)
    {
        switch (_result)
        {
            case JudgeType.Perfect:
                scoreData.combo++;
                scoreData.score += perfectScore * FeverManager.Instance.GetMultiplier();
                FeverManager.Instance.AddGauge(3f);
                break;
            case JudgeType.Great:
                scoreData.combo++;
                scoreData.score += greatScore * FeverManager.Instance.GetMultiplier();
                FeverManager.Instance.AddGauge(2f);
                break;
            case JudgeType.Good:
                scoreData.combo++;
                scoreData.score += goodScore * FeverManager.Instance.GetMultiplier();
                FeverManager.Instance.AddGauge(1f);
                break;
            case JudgeType.Bad:
                scoreData.combo = 0;
                scoreData.score += badScore * FeverManager.Instance.GetMultiplier();
                FeverManager.Instance.AddGauge(-5f);
                break;
            case JudgeType.Miss:
                scoreData.combo = 0;
                FeverManager.Instance.AddGauge(-10f);
                playerController.OnDamage(missScore);
                break;
        }

        FeedbackSystem.Instance.PlayFeedback(_result, _note.GetLane());
    }

    // 판정 UI 생성 및 콤보 UI 업데이트
    public void ShowJudge(JudgeType _data, int _lane, int _score)
    {
        JudgmentUIGet().Play(_data, lanePositions[_lane]);
        scoreText.text = _score.ToString();
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
}