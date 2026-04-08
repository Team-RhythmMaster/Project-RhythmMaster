using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Utils.EnumType;

public class FeverManager : MonoBehaviour
{
    private static FeverManager instance;
    public static FeverManager Instance { get { return instance; } }

    private FeverState state = FeverState.Normal;

    private Slider gaugeSlider;
    private Text gaugeText;

    private SpriteRenderer[] bg;
    public Sprite[] normalBG;
    public Sprite[] feverBG;

    private CloudMovement[] clouds;
    private StarMovement[] stars;

    [Header("Gauge")]
    private float gauge = 0f;      // 현재 게이지
    private float maxGauge = 100f; // 피버 발동 기준값

    [Header("Fever")]
    private float feverDuration = 15f; // 피버 지속 시간
    private float feverTimer = 0f;     // 남은 시간 (카운트다운용)

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
        bg = GameObject.Find("BG").GetComponentsInChildren<SpriteRenderer>().Take(2).ToArray();

        clouds = GameObject.Find("Cloud").GetComponentsInChildren<CloudMovement>();
        stars = GameObject.Find("Star").GetComponentsInChildren<StarMovement>(true);
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
        CloudActive(false);
        StarActive(true);

        bg[0].sprite = feverBG[0];
        bg[1].sprite = feverBG[1];

        state = FeverState.Fever;
        scoreMultiplier = feverMultiplier;
        feverTimer = feverDuration;
        gauge = 0;

        gaugeSlider.value = (float)(gauge / maxGauge);
    }

    // 피버 종료
    void EndFever()
    {
        CloudActive(true);
        StarActive(false);

        bg[0].sprite = normalBG[0];
        bg[1].sprite = normalBG[1];

        state = FeverState.Normal;
        scoreMultiplier = normalMultiplier;
    }

    // 현재 배율 반환
    public int GetMultiplier()
    {
        return scoreMultiplier;
    }

    public void CloudActive(bool _isActive)
    {
        for(int i = 0; i < clouds.Length; i++)
        {
            if (_isActive)
                clouds[i].RandomReset();
            clouds[i].gameObject.SetActive(_isActive);
        }
    }

    public void StarActive(bool _isActive)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(_isActive);
        }
    }
}
