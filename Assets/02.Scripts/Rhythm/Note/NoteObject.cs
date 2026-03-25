using UnityEngine;
using UnityEngine.Rendering.Universal;
using Utils.ClassUtility;

// Note 기본 동작
public abstract class NoteObject : MonoBehaviour
{
    protected NoteGenerator noteGenerator;

    // 기본 데이터
    public Note note;               // 원본 데이터
    public float noteTime;          // 판정 시간
    public float speed = 3f;        // 이동 속도
    protected float offset = 0.05f; // 싱크 보정

    // 판정 범위
    protected float perfect = 0.1f;
    protected float great = 0.2f;
    protected float good = 0.3f;
    protected float bad = 0.4f;
    protected float miss = 0.5f;

    // 상태
    protected bool isHit = false;

    protected virtual void Awake()
    {
        noteGenerator = FindFirstObjectByType<NoteGenerator>();
    }

    protected virtual void Update()
    {
        float currentTime = AudioManager.Instance.songTime + offset;
        float x = noteGenerator.hitLine.x + (noteTime - currentTime) * speed; // 핵심 공식
        transform.position = new Vector3(x, note.lane * 1.5f, 0);

        CheckMiss(currentTime);
    }

    public virtual void Init(float _noteTime, int _lane, float _speed)
    {
        noteTime = _noteTime;
        speed = _speed;

        note.lane = _lane;
        isHit = false;

        NoteManager.Instance.Register(this);
    }

    // 입력 (자식이 구현)
    public abstract void TryHit();

    // Miss 처리
    protected virtual void CheckMiss(float currentTime)
    {
        if (!isHit && currentTime - noteTime > miss)
        {
            JudgeManager.Instance.Judge("Miss");
            Remove();
        }
    }

    // 제거
    protected void Hit()
    {
        isHit = true;
        Remove();
    }

    protected void Remove()
    {
        NoteManager.Instance.Unregister(this);
        NoteManager.Instance.Return(this);
    }
}