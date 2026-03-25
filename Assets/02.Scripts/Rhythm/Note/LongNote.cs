using UnityEngine;

// 롱 노트
public class LongNote : NoteObject
{
    public float endTime;

    private bool isHolding = false;
    private bool holdInput = false;

    public Transform body; // 길이 늘어나는 부분

    public void Init(float time, int lane, float speed, float endTime)
    {
        base.Init(time, lane, speed);

        this.endTime = endTime;

        isHolding = false;
        holdInput = false;

        UpdateVisual();
    }

    // 길이 계산
    private void UpdateVisual()
    {
        float duration = endTime - noteTime;
        float length = duration * speed;

        // Y축 기준으로 늘림
        body.localScale = new Vector3(length, 0.5f, 1);
        // 위치 보정 (중앙 기준)
        body.localPosition = new Vector3(length / 2f, 0, 0);
    }

    public override void TryHit()
    {
        float currentTime = AudioManager.Instance.songTime;

        float diff = Mathf.Abs(noteTime - currentTime);

        if (diff <= 0.1f)
        {
            isHolding = true;
            JudgeManager.Instance.Judge("Perfect");
        }
    }

    public void SetHoldInput(bool holding)
    {
        holdInput = holding;
    }

    protected override void Update()
    {
        base.Update();

        float currentTime = AudioManager.Instance.songTime;

        if (isHolding)
        {
            if (holdInput)
            {
                if (currentTime >= endTime)
                {
                    Complete();
                }
            }
            else
            {
                Fail();
            }
        }
    }

    void Complete()
    {
        JudgeManager.Instance.Judge("Perfect");
        Remove();
    }

    void Fail()
    {
        JudgeManager.Instance.Judge("Miss");
        Remove();
    }
}