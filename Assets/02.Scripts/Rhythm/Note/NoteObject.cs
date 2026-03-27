using UnityEngine;
using Utils.EnumType;
using Utils.ClassUtility;

// Note 기본 동작
public abstract class NoteObject : MonoBehaviour
{
    protected NoteGenerator noteGenerator;

    // 기본 데이터
    private NoteData data;
    protected float offset = -0.2f; // 싱크 보정값
    protected float diff = 0.0f;     // longNote 시작점과 끝점 시간 차이

    // 판정 범위
    protected const float perfect = 0.05f;
    protected const float great = 0.15f;
    protected const float good = 0.2f;
    protected const float bad = 0.25f;
    protected const float miss = 0.5f;

    protected bool isHit = false;
    private bool firstFrame = true;

    public int GetLane() => data.lane;
    public float GetSpeed() => data.speed;
    public float GetTime() => data.time;
    public float GetEndTime() => data.endTime;

    protected virtual void Awake()
    {

    }

    protected virtual void Update()
    {
        if (firstFrame)
        {
            firstFrame = false;
            return; // 프레임 위치 유지
        }

        float currentTime = AudioManager.Instance.songTime + offset;
        // 핵심 공식 노트 위치 계산 : 노트 위치 = 판정선 + (남은 시간 × 속도)
        float x = NoteManager.hitLineX + (data.time - currentTime) * data.speed;
        transform.position = new Vector3(x, NoteManager.Instance.laneY[data.lane], 0);

        CheckMiss(currentTime);
    }

    public void Init(NoteData _data, float _speed, NoteGenerator _generator)
    {
        noteGenerator = _generator;
        data = _data;
        data.speed = _speed;
        isHit = false;
        NoteManager.Instance.Register(this);
    }

    // 판정 시도
    public abstract void TryHit();

    protected virtual void CheckMiss(float _currentTime)
    {
        if (!isHit && _currentTime - data.time > miss)
        {
            JudgeManager.Instance.Judge(JudgeType.Miss);
            Remove();
        }
    }

    // 노트 제거 및 반환
    protected void Remove()
    {
        NoteManager.Instance.Unregister(this);
        NoteManager.Instance.Return(this);
    }
}