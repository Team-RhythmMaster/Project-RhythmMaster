using TMPro;
using UnityEngine;
using DG.Tweening;

public class ComboUI : MonoBehaviour
{
    private static ComboUI instance;
    public static ComboUI Instance {  get { return instance; } }

    public TMP_Text text;
    public RectTransform rect;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateCombo(int _combo)
    {
        text.text = _combo.ToString();

        rect.DOKill();
        rect.localScale = Vector3.one;

        rect.DOScale(1.3f, 0.1f)
            .SetLoops(2, LoopType.Yoyo);
    }
}