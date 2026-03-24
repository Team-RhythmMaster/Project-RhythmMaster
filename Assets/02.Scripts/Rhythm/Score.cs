using System;
using UnityEngine;
using Utils.EnumType;
using Utils.ClassUtility;

public class Score : MonoBehaviour
{
    private static Score instance;
    public static Score Instance
    {
        get { return instance; }
    }

    public ScoreData data;

    private UIText uiJudgement;
    private UIText uiCombo;
    private UIText uiScore;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init()
    {
        AniPreset.Instance.Join(uiJudgement.uiObject.Name);
        AniPreset.Instance.Join(uiCombo.uiObject.Name);
        AniPreset.Instance.Join(uiScore.uiObject.Name);
    }

    public void Clear()
    {
        data = new ScoreData();
        data.judgeText = Enum.GetNames(typeof(JudgeType));
        data.judgeColor = new Color[3] { Color.blue, Color.yellow, Color.red };
        uiJudgement.SetText("");
        uiCombo.SetText("");
        uiScore.SetText("0");
    }

    public void SetScore()
    {
        uiJudgement.SetText(data.judgeText[(int)data.judge]);
        uiJudgement.SetColor(data.judgeColor[(int)data.judge]);
        uiCombo.SetText($"{data.combo}");
        uiScore.SetText($"{data.score}");

        AniPreset.Instance.PlayPop(uiJudgement.uiObject.Name, uiJudgement.uiObject.rect);
        AniPreset.Instance.PlayPop(uiCombo.uiObject.Name, uiCombo.uiObject.rect);
    }

    public void Ani(UIObject uiObject)
    {
        //AniPreset.Instance.PlayPop(uiObject.Name, uiObject.rect);
    }
}
