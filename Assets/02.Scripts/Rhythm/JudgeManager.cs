using UnityEngine;

public class JudgeManager : MonoBehaviour
{
    private static JudgeManager instance;
    public static JudgeManager Instance { get { return instance; } }

    public int combo = 0;
    public int score = 0;

    private void Awake()
    {
        if(instance != null & instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // 판정 결과 처리
    public void Judge(string result)
    {
        switch (result)
        {
            case "Perfect":
                combo++;
                score += 1000;
                break;

            case "Great":
                combo++;
                score += 500;
                break;

            case "Good":
                combo++;
                score += 200;
                break;

            case "Bad":
                combo = 0;
                score += 100;
                break;

            case "Miss":
                combo = 0;
                break;
        }

        UIManager.Instance.ShowJudge(result);
    }
}
