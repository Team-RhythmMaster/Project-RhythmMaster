using UnityEngine;
using DG.Tweening;
using Utils.EnumType;
using UnityEngine.UI;

// 판정 텍스트 View
public class JudgmentUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private RectTransform rect;
    private Text judgeText;

    private float moveDistance = 100f;
    private float moveDuration = 0.3f;
    private float stayDuration = 0.4f;
    private float fadeDuration = 0.4f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        judgeText = GetComponent<Text>();
    }

    public void Play(JudgeType _type, Vector2 _pos)
    {
        gameObject.SetActive(true);

        judgeText.text = _type.ToString();
        judgeText.color = GetColor(_type);

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
            JudgeManager.Instance.Return(this);
        });
    }

    private Color GetColor(JudgeType _type)
    {
        switch (_type)
        {
            case JudgeType.Perfect:
                return Color.yellow;
            case JudgeType.Great:
                return Color.green;
            case JudgeType.Good:
                return Color.blue;
            case JudgeType.Bad:
                return Color.gray;
            default:
                return Color.red;
        }
    }
}