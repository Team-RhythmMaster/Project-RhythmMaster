using UnityEngine;
using DG.Tweening;
using Utils.EnumType;
using UnityEngine.UI;
using System.Collections.Generic;

// 판정 텍스트 View
public class JudgmentUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private RectTransform rect;
    private Image judgeImage;
    public Sprite[] judges;

    private Dictionary<int, Vector2> judgeScales = new Dictionary<int, Vector2>();

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        judgeImage = GetComponent<Image>();

        judgeScales.Add(0, new Vector2(508, 216));
        judgeScales.Add(1, new Vector2(376, 106));
        judgeScales.Add(2, new Vector2(336, 98));
        judgeScales.Add(3, new Vector2(248, 108));
        judgeScales.Add(4, new Vector2(280, 96));
    }

    public void Play(JudgeType _type, Vector2 _pos)
    {
        gameObject.SetActive(true);
        judgeImage.sprite = judges[(int)_type];
        rect.sizeDelta = judgeScales[(int)_type] * 0.5f;

        // 초기화
        canvasGroup.alpha = 0f;
        rect.localScale = Vector3.one;
        rect.anchoredPosition = _pos - new Vector2(0, 100f);

        Sequence seq = DOTween.Sequence();

        seq.Append(canvasGroup.DOFade(1f, 0.2f));
        seq.Join(rect.DOAnchorPos(_pos, 0.3f));
        seq.Join(rect.DOScale(1.2f, 0.15f).From(0.8f));

        seq.AppendInterval(0.4f);

        seq.Append(rect.DOAnchorPos(_pos + new Vector2(0, 50f), 0.3f));
        seq.Join(canvasGroup.DOFade(0f, 0.3f));

        seq.OnComplete(() =>
        {
            JudgeManager.Instance.JudgementUIReturn(this);
        });
    }
}