using UnityEngine;
using Utils.ClassUtility;

// Note 기본 동작
public abstract class NoteObject : MonoBehaviour
{
    protected NoteGenerator noteGenerator;

    // 기본 데이터
    public Note note;                // 원본 데이터
    public float speed;              // 이동 속도
    protected float offset = -0.03f; // 싱크 보정값

    // 판정 범위
    protected float perfect = 0.1f;
    protected float great = 0.2f;
    protected float good = 0.3f;
    protected float bad = 0.4f;
    protected float miss = 0.5f;

    protected float diff = 0.0f;

    // 상태
    protected bool isHit = false;

    protected virtual void Awake()
    {
        noteGenerator = FindFirstObjectByType<NoteGenerator>();
        speed = noteGenerator.speed;
    }

    protected virtual void Update()
    {
        float currentTime = AudioManager.Instance.songTime + offset;
        float x = noteGenerator.hitLine.x + (note.time - currentTime) * speed; // 핵심 공식
        transform.position = new Vector3(x, NoteManager.Instance.laneY[note.lane], 0);

        CheckMiss(currentTime);
    }

    public virtual void Init(float _noteTime, int _lane, float _speed)
    {
        note.time = _noteTime;
        speed = _speed;

        note.lane = _lane;
        isHit = false;

        NoteManager.Instance.Register(this);
    }

    public virtual void Init(Note data, float speed)
    {
        note = data;
        this.speed = speed;

        isHit = false;

        NoteManager.Instance.Register(this);
    }

    public abstract void TryHit();

    protected virtual void CheckMiss(float currentTime)
    {
        if (!isHit && currentTime - note.time > miss)
        {
            JudgeManager.Instance.Judge("Miss");
            Remove();
        }
    }

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