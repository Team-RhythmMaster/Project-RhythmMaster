using UnityEngine;
using Utils.EnumType;

public class JudgeManager : MonoBehaviour
{
    private static JudgeManager instance;
    public static JudgeManager Instance { get { return instance; } }

    // 판정 범위
    public const float perfect = 0.05f;
    public const float great = 0.15f;
    public const float good = 0.2f;
    public const float bad = 0.25f;
    public const float miss = 0.5f;

    public int combo = 0;
    public int score = 0;

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

    // 판정 결과 처리
    public void Judge(JudgeType _result)
    {
        switch (_result)
        {
            case JudgeType.Perfect:
                combo++;
                score += 1000;
                break;

            case JudgeType.Great:
                combo++;
                score += 500;
                break;

            case JudgeType.Good:
                combo++;
                score += 200;
                break;

            case JudgeType.Bad:
                combo = 0;
                score += 100;
                break;

            case JudgeType.Miss:
                combo = 0;
                break;
        }

        UIManager.Instance.ShowJudge(_result);
    }
}
