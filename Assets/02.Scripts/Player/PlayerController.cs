using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Slider hpSlider;
    private Text hpGuageText;

    private float currentHP = 100f;

    private void Awake()
    {
        hpSlider = GameObject.Find("HPSlider").GetComponent<Slider>();
        hpGuageText = GameObject.Find("HPGaugeText").GetComponent<Text>();
    }

    public void OnDamage(float _damage)
    {
        currentHP -= _damage;
        currentHP = Mathf.Clamp(currentHP, 0f, 100f);

        hpSlider.value = (float)(currentHP / 100f);
        hpGuageText.text = string.Format("{0} / 100", (int)currentHP);

        if (currentHP <= 0f)
        {
            // Game Over Ã³¸®
             Debug.Log("Game Over!");
        }
    }
}
