using UnityEngine;
using DG.Tweening;
using Utils.EnumType;
using System.Collections.Generic;

public class JudgeManager : MonoBehaviour
{
    private static JudgeManager instance;
    public static JudgeManager Instance { get { return instance; } }

    // UI 오브젝트풀
    public JudgmentUI judgePrefab;
    private Transform parentTransform;
    private Queue<JudgmentUI> judgePool = new Queue<JudgmentUI>();

    private ComboUI comboUI;
    private HitEffect hitEffect;
    private AudioSource audioSource;
    public AudioClip[] punchSFX;

    // lane별 판정 UI 생성 위치
    private Vector2[] lanePositions = { new Vector2(-480.0f, 290.0f), new Vector2(-480.0f, -35.0f) };

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
        parentTransform = GameObject.Find("JudgePool").transform;
        comboUI = FindAnyObjectByType<ComboUI>();
        hitEffect = FindAnyObjectByType<HitEffect>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFeedback(JudgeType _type, int _lane)
    {
        // 타격 이펙트
        //hitEffect.Play(result.position, result.type);
        ShowJudge(_type, _lane, ScoreManager.Instance.scoreData.score);
        comboUI.UpdateCombo(ScoreManager.Instance.scoreData.combo);

        PlaySound(_type, _lane);
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

    // 화면 효과
    private void PlayScreenEffect(JudgeType _type)
    {
        Camera.main.transform.DOKill();
        Camera.main.transform.position = new Vector3(0.0f, 0.0f, -10.0f);

        if (_type == JudgeType.Perfect)
            Camera.main.transform.DOShakePosition(0.1f, 0.2f);
    }

    // 판정 UI 생성 및 콤보 UI 업데이트
    public void ShowJudge(JudgeType _data, int _lane, int _score)
    {
        JudgmentUIGet().Play(_data, lanePositions[_lane]);
        ScoreManager.Instance.SetScore(_score.ToString());
    }

    // 판정 UI 오브젝트 풀링 사용
    public JudgmentUI JudgmentUIGet()
    {
        if (judgePool.Count > 0)
        {
            var obj = judgePool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        return Instantiate(judgePrefab, parentTransform);
    }

    // 판정 UI 오브젝트 풀링 반환
    public void JudgementUIReturn(JudgmentUI _obj)
    {
        _obj.gameObject.SetActive(false);
        judgePool.Enqueue(_obj);
    }
}