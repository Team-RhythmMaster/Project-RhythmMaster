using UnityEngine;
using Utils.EnumType;

public class LongNote : NoteObject
{
    private LineRenderer lineRenderer;
    private JudgeType judge;

    private bool isHolding = false;  // 판정을 시작했는지 여부
    private bool isKeyHold = false;  // 현재 키를 누르고 있는지 여부

    protected override void Start()
    {
        base.Start();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    protected override void Update()
    {
        base.Update();
        UpdateLine(currentTime);

        if (!isHolding) 
            return;

        if (!isKeyHold && currentTime < data.endTime)
        {
            // 누르다 떼면 실패
            Fail();
        }
        else if (isKeyHold && currentTime >= data.endTime)
        {
            // 끝까지 유지하면 성공
            Complete();
        }
    }

    // 판정 시작 시점과 끝 시점에 따라 선의 위치를 업데이트 → 롱노트 시각화 표현
    private void UpdateLine(float _currentTime)
    {
        float startTime = isHolding ? _currentTime : data.time;
        float startX = NoteManager.hitLineX + ((startTime - _currentTime) * data.speed);
        float endX = NoteManager.hitLineX + ((data.endTime - _currentTime) * data.speed);

        lineRenderer.SetPosition(0, new Vector3(startX, yPos, 0));
        lineRenderer.SetPosition(1, new Vector3(endX, yPos, 0));
    }

    public override void TryHit()
    {
        diff = Mathf.Abs(data.time - currentTime);

        if (diff <= JudgeManager.bad)
        {
            isHit = true;
            isHolding = true;
            spriteRenderer.color = new Color(1, 1, 1, 0.0f);
            spriteRenderer.sprite = noteHitSprites[spriteIndex];
            NoteManager.Instance.SetActiveLongNote(data.lane, this);

            if (diff <= JudgeManager.perfect)
                judge = JudgeType.Perfect;
            else if (diff <= JudgeManager.great)
                judge = JudgeType.Great;
            else if (diff <= JudgeManager.good)
                judge = JudgeType.Good;
            else
                judge = JudgeType.Bad;
        }
    }

    void Complete()
    {
        JudgeManager.Instance.Judgment(judge, this);
        NoteManager.Instance.ClearActiveLongNote(data.lane);
        Remove();
    }

    void Fail()
    {
        JudgeManager.Instance.Judgment(JudgeType.Miss, this);
        NoteManager.Instance.ClearActiveLongNote(data.lane);
        Remove();
    }

    // NoteManager에서 매 프레임마다 현재 누르고 있는 키 상태를 업데이트
    public void SetHold(bool _holding)
    {
        isKeyHold = _holding;
    }
}