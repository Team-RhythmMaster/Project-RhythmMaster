using UnityEngine;
using Utils.EnumType;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    // lane별 판정 UI 생성 위치
    private Vector2[] lanePositions = { new Vector2(-480.0f, 300.0f), new Vector2(-480.0f, -25.0f) };

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ShowJudge(JudgeType _data, int _lane)
    {
        var ui = JudgeManager.Instance.JudgmentUIGet();
        ui.Play(_data, lanePositions[_lane]);
        //ComboUI.Instance.UpdateCombo(_data.combo);
    }
}