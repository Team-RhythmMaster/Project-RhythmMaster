using UnityEngine;
using Utils.EnumType;

// 롱 노트
public class LongNote : NoteObject
{
    private LineRenderer lineRenderer;
    private GameObject head;

    private bool isHolding = false;
    private bool isKeyHold = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        head = transform.GetChild(0).gameObject;

        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    protected override void Update()
    {
        base.Update();

        float currentTime = AudioManager.Instance.songTime + offset;
        UpdateLine(currentTime);

        if (!isHolding) 
            return;

        // 누르다 떼면 실패
        if (!isKeyHold && currentTime < GetEndTime())
        {
            Fail();
            return;
        }
        // 끝까지 유지 성공
        if (currentTime >= GetEndTime())
        {
            Complete();
        }
    }

    private void UpdateLine(float _currentTime)
    {
        float startTime = isHolding ? _currentTime : GetTime();

        float startX = NoteManager.hitLineX + (startTime - _currentTime) * GetSpeed();
        float endX = NoteManager.hitLineX + (GetEndTime() - _currentTime) * GetSpeed();

        float y = NoteManager.Instance.GetLaneY(GetLane());

        lineRenderer.SetPosition(0, new Vector3(startX, y, 0));
        lineRenderer.SetPosition(1, new Vector3(endX, y, 0));
    }

    public void SetHold(bool _holding)
    {
        isKeyHold = _holding;
    }

    public override void TryHit()
    {
        float currentTime = AudioManager.Instance.songTime + offset;
        diff = Mathf.Abs(GetTime() - currentTime);
        head.gameObject.SetActive(false);

        if (diff <= JudgeManager.bad)
        {
            isHolding = true;
            isHit = true;

            NoteManager.Instance.SetActiveLongNote(GetLane(), this);
        }
    }

    void Complete()
    {
        JudgeManager.Instance.Judge(JudgeType.Perfect);
        NoteManager.Instance.ClearActiveLongNote(GetLane());
        Remove();
    }

    void Fail()
    {
        JudgeManager.Instance.Judge(JudgeType.Miss);
        NoteManager.Instance.ClearActiveLongNote(GetLane());
        Remove();
    }
}