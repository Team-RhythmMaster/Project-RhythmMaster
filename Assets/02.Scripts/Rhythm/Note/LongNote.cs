using UnityEngine;
using System.Linq;
using Utils.EnumType;

public class LongNote : NoteObject
{
    private SpriteRenderer tailRenderer;
    private JudgeType judge;

    private bool isHolding = false;  // 판정을 시작했는지 여부
    private bool isKeyHold = false;  // 현재 키를 누르고 있는지 여부

    protected override void Awake()
    {
        base.Awake();
        tailRenderer = GetComponentsInChildren<SpriteRenderer>().Skip(1).FirstOrDefault();
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
        float endX = NoteManager.hitLineX + ((data.endTime - _currentTime) * data.speed);

        if (!isHolding)
        {
            // 일반 상태 (이동 중)
            float startX = NoteManager.hitLineX + ((data.time - _currentTime) * data.speed);
            float length = endX - startX;
            transform.position = new Vector3(startX, transform.position.y, 0);
            SetTail(length);
        }
        else
        {
            // 판정 시작 후 (머리 고정)
            float length = endX - NoteManager.hitLineX;
            transform.position = new Vector3(NoteManager.hitLineX, transform.position.y, 0);
            SetTail(length);
        }
    }

    private void SetTail(float length)
    {
        if (length < 0) length = 0;

        Vector2 size = tailRenderer.size;
        size.x = length;
        tailRenderer.size = size;
    }

    public override void TryHit()
    {
        diff = Mathf.Abs(data.time - currentTime);

        if (diff <= ScoreManager.bad)
        {
            isHit = true;
            isHolding = true;

            spriteRenderer.sprite = noteHitSprites[3];
            tailRenderer.sprite = noteHitSprites[4];
            NoteManager.Instance.SetActiveLongNote(data.lane, this);

            if (diff <= ScoreManager.perfect)
                judge = JudgeType.Perfect;
            else if (diff <= ScoreManager.great)
                judge = JudgeType.Great;
            else if (diff <= ScoreManager.good)
                judge = JudgeType.Good;
            else
                judge = JudgeType.Bad;
        }
    }

    void Complete()
    {
        ScoreManager.Instance.Judgment(judge, this);
        NoteManager.Instance.ClearActiveLongNote(data.lane);
        Remove();
    }

    void Fail()
    {
        ScoreManager.Instance.Judgment(JudgeType.Miss, this);
        NoteManager.Instance.ClearActiveLongNote(data.lane);
        Remove();
    }

    // NoteManager에서 매 프레임마다 현재 누르고 있는 키 상태를 업데이트
    public void SetHold(bool _holding)
    {
        isKeyHold = _holding;
    }
}