using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils.EnumType;

public class FeverManager : MonoBehaviour
{
    private static FeverManager instance;
    public static FeverManager Instance { get { return instance; } }

    private FeverState state = FeverState.Normal;

    private Slider gaugeSlider;
    public Text gaugeText;
    private GameObject flame;

    [Header("Gauge")]
    private float gauge = 0f;      // 현재 게이지
    private float maxGauge = 100f; // 피버 발동 기준값

    [Header("Fever")]
    private float feverDuration = 15f; // 피버 지속 시간
    private float feverTimer = 0f;   // 남은 시간 (카운트다운용)

    [Header("Multiplier")]
    private int scoreMultiplier = 1;  // 현재 적용 중인 배율 (노말: 1, 피버: 2)
    private int normalMultiplier = 1; // 노말 상태일 때 적용되는 배율
    private int feverMultiplier = 2;  // 피버 상태일 때 적용되는 배율

    private void Awake()
    {
        if(instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        Init();
    }

    private void Init()
    {
        gaugeSlider = GameObject.Find("FeverSlider").GetComponent<Slider>();
        gaugeText = GameObject.Find("GaugeText").GetComponent<Text>();
        flame = gaugeSlider.transform.GetChild(4).gameObject;
    }

    private void Update()
    {
        if (state == FeverState.Fever)
        {
            feverTimer -= Time.deltaTime;

            if (feverTimer <= 0f)
                EndFever();
        }
        gaugeText.text = string.Format("{0}/{1}", (int)gauge, (int)maxGauge);
    }

    // 게이지 증가/감소
    public void AddGauge(float amount)
    {
        if (state == FeverState.Fever)
            return;

        gauge += amount;
        gauge = Mathf.Clamp(gauge, 0, maxGauge);
        gaugeSlider.value = (float)(gauge / maxGauge);

        if (gauge >= maxGauge)
            StartFever();
    }

    // 피버 시작
    void StartFever()
    {
        state = FeverState.Fever;
        scoreMultiplier = feverMultiplier;
        feverTimer = feverDuration;
        gauge = 0;

        Sequence seq = DOTween.Sequence();
        seq.Append(flame.transform.DOScale(1.5f, 0.2f).SetEase(Ease.OutBack));
        seq.Append(flame.transform.DOScale(1.0f, 0.2f).SetEase(Ease.InOutSine));
        seq.SetLoops(-1);

        flame.SetActive(true);
        gaugeSlider.value = (float)(gauge / maxGauge);
    }

    // 피버 종료
    void EndFever()
    {
        state = FeverState.Normal;
        scoreMultiplier = normalMultiplier;

        flame.transform.DOKill();
        flame.transform.localScale = Vector3.one;

        flame.SetActive(false);
    }

    // 현재 배율 반환
    public int GetMultiplier()
    {
        return scoreMultiplier;
    }
}
