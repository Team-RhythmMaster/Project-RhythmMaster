using UnityEngine;
using Utils.EnumType;

// 롱 노트
public class LongNote : NoteObject
{
    private HoldState state = HoldState.Idle;
    private LineRenderer lineRenderer;
    private GameObject head;

    private bool isKeyHeld = false;

    protected override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
        head = transform.GetChild(0).gameObject;

        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    protected override void Update()
    {
        base.Update();
        UpdateLine();
        float currentTime = AudioManager.Instance.songTime + offset;

        if (state != HoldState.Holding)
            return;

        // 누르다 떼면 실패
        if (!isKeyHeld && currentTime < note.endTime)
        {
            Fail();
            return;
        }

        // 끝까지 유지 성공
        if (currentTime >= note.endTime)
        {
            Complete();
        }
    }

    private void UpdateLine()
    {
        float currentTime = AudioManager.Instance.songTime + offset;
        float startTime = note.time;

        // 잡고 있으면 시작점이 현재 시간으로 따라옴
        if (state == HoldState.Holding)
            startTime = currentTime;

        float startX = noteGenerator.hitLine.x + (startTime - currentTime) * speed;
        float endX = noteGenerator.hitLine.x + (note.endTime - currentTime) * speed;
        float y = NoteManager.Instance.laneY[note.lane];

        lineRenderer.SetPosition(0, new Vector3(startX, y, 0));
        lineRenderer.SetPosition(1, new Vector3(endX, y, 0));
    }

    public void SetHoldInput(bool holding)
    {
        isKeyHeld = holding;
    }

    public override void TryHit()
    {
        float currentTime = AudioManager.Instance.songTime + offset;
        diff = Mathf.Abs(note.time - currentTime);
        head.gameObject.SetActive(false);

        if (diff <= bad)
        {
            isHit = true;
            state = HoldState.Holding;
            NoteManager.Instance.SetActiveLongNote(note.lane, this);
        }
    }

    void Complete()
    {
        JudgeManager.Instance.Judge("Perfect"); // 간단 처리
        NoteManager.Instance.ClearActiveLongNote(note.lane);
        state = HoldState.Completed;
        Remove();
    }

    void Fail()
    {
        JudgeManager.Instance.Judge("Miss");
        NoteManager.Instance.ClearActiveLongNote(note.lane);
        state = HoldState.Failed;
        Remove();
    }
}