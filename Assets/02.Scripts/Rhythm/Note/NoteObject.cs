using UnityEngine;
using Utils.EnumType;
using Utils.ClassUtility;

public abstract class NoteObject : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    public Sprite[] noteSprites;
    public Sprite[] noteHitSprites;
    protected int spriteIndex = 0;

    protected NoteData data;
    protected float yPos = 0.0f;       // 노트 y 위치 (lane 위치)
    protected float diff = 0.0f;       // longNote 시작점과 끝점 시간 차이
    protected float offset = 0.0f;     // 싱크 보정값
    protected float currentTime = 0.0f;// 현재 노트 시간 (판정 시점)

    protected bool isHit = false;
    private bool isFirstFrame = true;

    public int GetLane() => data.lane;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if (isFirstFrame)
        {
            // 프레임 위치 유지
            isFirstFrame = false;
            return;
        }

        currentTime = AudioManager.Instance.songTime + offset;
        // 핵심 공식 노트 위치 계산 : 노트 위치 = 판정선 + (남은 시간 × 속도)
        float x = NoteManager.hitLineX + ((data.time - currentTime) * data.speed);
        transform.position = new Vector3(x, yPos, 0);

        CheckMiss(currentTime);
    }

    // 초기화
    public void Init(NoteData _data, float _speed)
    {
        isHit = false;
        data = _data;
        data.speed = _speed;
        yPos = NoteManager.Instance.laneY[data.lane];

        spriteIndex = data.IsLong ? 3 : Random.Range(0, noteSprites.Length - 1);
        spriteRenderer.sprite = noteSprites[spriteIndex];

        NoteManager.Instance.Register(this);
    }

    // 판정 시도
    public abstract void TryHit();

    protected virtual void CheckMiss(float _currentTime)
    {
        // 현재 시간 - 노트 시간 > Miss 판정 허용 오차
        if (!isHit && _currentTime - data.time > ScoreManager.miss)
        {
            ScoreManager.Instance.Judgment(JudgeType.Miss, this);
            Remove();
        }
    }

    // 노트 제거 및 반환
    protected void Remove()
    {
        NoteManager.Instance.Unregister(this);
        NoteManager.Instance.ReturnNote(this);
    }
}