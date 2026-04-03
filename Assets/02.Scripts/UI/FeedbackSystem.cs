using UnityEngine;
using DG.Tweening;
using Utils.EnumType;

public class FeedbackSystem : MonoBehaviour
{
    private static FeedbackSystem instance;
    public static FeedbackSystem Instance { get { return instance; } }

    private ComboUI comboUI;
    private HitEffect hitEffect;
    private AudioSource audioSource;

    public AudioClip[] punchSFX;

    private void Awake()
    {
        if(instance != null && instance != this)
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
        comboUI = FindAnyObjectByType<ComboUI>();
        hitEffect = FindAnyObjectByType<HitEffect>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFeedback(JudgeType _type, int _lane)
    {
        // 판정 UI
        JudgeManager.Instance.ShowJudge(_type, _lane, JudgeManager.Instance.score);
        // 콤보 UI
        comboUI.UpdateCombo(JudgeManager.Instance.combo);
        // 타격 이펙트
        //hitEffect.Play(result.position, result.type);

        // 사운드
        PlaySound(_type, _lane);
        // 화면 효과
        PlayScreenEffect(_type);
    }

    private void PlaySound(JudgeType _type, int _lane)
    {
        switch (_type)
        {
            case JudgeType.Perfect:
                audioSource.PlayOneShot(punchSFX[_lane]);
                break;
            case JudgeType.Great:
                audioSource.PlayOneShot(punchSFX[_lane]);
                break;
            case JudgeType.Good:
                audioSource.PlayOneShot(punchSFX[_lane]);
                break;
            case JudgeType.Bad:
                audioSource.PlayOneShot(punchSFX[_lane]);
                break;
            case JudgeType.Miss:
                audioSource.PlayOneShot(punchSFX[2]);
                break;
        }
    }

    private void PlayScreenEffect(JudgeType _type)
    {
        Camera.main.transform.DOKill();
        Camera.main.transform.position = new Vector3(0.0f, 0.0f, -10.0f);

        if (_type == JudgeType.Perfect)
            Camera.main.transform.DOShakePosition(0.1f, 0.2f);
    }
}