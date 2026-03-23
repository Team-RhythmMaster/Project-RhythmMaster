using TMPro;
using UnityEngine;

public class UIText : MonoBehaviour
{
    private TextMeshProUGUI text;
    public UIObject uiObject;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string _text)
    {
        text.text = _text;
    }

    public void SetColor(Color color)
    {
        text.color = color;
    }

    public void ChangeText()
    {
        UIManager.Instance.find.Invoke(uiObject.Name);
    }
}
