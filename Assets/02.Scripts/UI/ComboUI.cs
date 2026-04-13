using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private RectTransform rect;
    private Text comboText;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        comboText = GetComponentsInChildren<Text>().Skip(1).FirstOrDefault();
    }

    public void UpdateCombo(int _combo)
    {
        // 기존 틔윈 전부 제거
        rect.DOKill();
        canvasGroup.DOKill();

        if (_combo <= 0)
        {
            PlayComboBreakEffect();
            return;
        }

        canvasGroup.alpha = 1.0f;
        rect.localScale = Vector3.one;
        comboText.text = _combo.ToString();

        // 커졌다가 돌아오는 애니메이션 (타격감)
        rect.DOScale(1.3f, 0.1f)
            .SetLoops(2, LoopType.Yoyo) // 왕복 (커졌다가 다시 작아짐)
            .SetEase(Ease.OutCubic);    // 부드럽게 감속
    }

    // 콤보 끊길 시 효과
    private void PlayComboBreakEffect()
    {
        rect.localScale = Vector3.one;
        Sequence seq = DOTween.Sequence();

        // 살짝 크게 튀기기 (임팩트)
        seq.Append(rect.DOScale(1.5f, 0.1f)
            .SetEase(Ease.OutBack));

        // 동시에 작아지면서 사라짐
        seq.Append(rect.DOScale(0.5f, 0.2f)
            .SetEase(Ease.InCubic));

        // 투명도 감소 및 흔들림 효과
        seq.Join(canvasGroup.DOFade(0f, 0.2f));
        seq.Join(rect.DOShakeAnchorPos(0.2f, 10f));

        // 종료 시 콤보 텍스트 초기화 및 UI 비활성화
        seq.OnComplete(() =>
        {
            comboText.text = "0";
            canvasGroup.alpha = 0.0f;
        });
    }
}