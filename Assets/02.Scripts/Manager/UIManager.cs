using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils.EnumType;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    public Func<string, UIActor> find;
    private Dictionary<string, UIActor> uiObjectDic = new Dictionary<string, UIActor>();

    public Sequence seq;
    public Text judgeText;
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    private float moveDistance = 100f;
    private float moveDuration = 0.3f;
    private float stayDuration = 0.4f;
    private float fadeDuration = 0.4f;

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

        Init();
    }

    public void Init()
    {
        rect = judgeText.GetComponent<RectTransform>();
        canvasGroup = judgeText.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        find = FindUI;

        UIObject[] objs = FindObjectsByType<UIObject>(FindObjectsSortMode.None);
        foreach (UIObject obj in objs)
        {
            uiObjectDic.Add(obj.Name, new UIActor(obj, null));
        }
    }

    public void ShowJudge(JudgeType _result)
    {
        switch (_result)
        {
            case JudgeType.Perfect:
                judgeText.color = Color.yellow;
                break;
            case JudgeType.Great:
                judgeText.color = Color.green;
                break;
            case JudgeType.Good:
                judgeText.color = Color.blue;
                break;
            case JudgeType.Bad:
                judgeText.color = Color.gray;
                break;
            case JudgeType.Miss:
                judgeText.color = Color.red;
                break;
        }

        judgeText.text = _result.ToString();
        Play();
    }

    public void Play()
    {
        if (seq != null && seq.IsActive())
        {
            seq.Kill();
        }

        // 시작 위치 (아래)
        Vector2 startPos = rect.anchoredPosition - new Vector2(0, moveDistance);
        Vector2 endPos = rect.anchoredPosition;

        rect.anchoredPosition = startPos;
        seq = DOTween.Sequence();

        // 1. 올라오면서 나타남
        seq.Append(canvasGroup.DOFade(1f, 0.2f));
        seq.Join(rect.DOAnchorPos(endPos, moveDuration).SetEase(Ease.OutCubic));

        // 2. 잠깐 유지
        seq.AppendInterval(stayDuration);

        // 3. 위로 살짝 더 올라가면서 사라짐
        seq.Append(rect.DOAnchorPos(endPos + new Vector2(0, 50f), fadeDuration)
            .SetEase(Ease.OutCubic));
        seq.Join(canvasGroup.DOFade(0f, fadeDuration));

        // 4. 끝나면 삭제 (옵션)
        seq.OnComplete(() =>
        {
            //judgeText.gameObject.SetActive(false);
        });
    }

    public UIActor FindUI(string uiName)
    {
        UIActor actor = uiObjectDic[uiName];
        if (actor.action != null)
            actor.action.Invoke(actor.uiObject);
        return actor;
    }

    public UIActor GetUI(string uiName)
    {
        UIActor actor = uiObjectDic[uiName];
        return actor;
    }
}
