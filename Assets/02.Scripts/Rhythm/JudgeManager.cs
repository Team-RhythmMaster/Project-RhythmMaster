using UnityEngine;
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

    // 점수 데이터
    public ScoreData data;
    private int combo = 0;
    private int score = 0;

    // 판정 범위
    public const float perfect = 0.1f;
    public const float great = 0.15f;
    public const float good = 0.2f;
    public const float bad = 0.25f;
    public const float miss = 0.3f;

    // 판정별 점수
    private const int perfectScore = 1000;
    private const int greatScore = 500;
    private const int goodScore = 250;
    private const int badScore = 100;
    private const int missScore = 0;

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
        parentTransform = GameObject.Find("Canvas/JudgePool").transform;
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

    public void Return(JudgmentUI _obj)
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
                combo++;
                score += perfectScore;
                break;
            case JudgeType.Great:
                combo++;
                score += greatScore;
                break;
            case JudgeType.Good:
                combo++;
                score += goodScore;
                break;
            case JudgeType.Bad:
                combo = 0;
                score += badScore;
                break;
            case JudgeType.Miss:
                combo = missScore;
                break;
        }

        UIManager.Instance.ShowJudge(_result, _note.GetLane());
    }
}
