using UnityEngine;
using DG.Tweening;

public class HitEffect : MonoBehaviour
{
    public RectTransform rect;
    public CanvasGroup cg;

    public void Play(Vector2 _pos)
    {
        rect.anchoredPosition = _pos;
        cg.alpha = 1f;

        Sequence seq = DOTween.Sequence();

        seq.Append(rect.DOScale(1.5f, 0.2f));
        seq.Join(cg.DOFade(0f, 0.2f));

        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
