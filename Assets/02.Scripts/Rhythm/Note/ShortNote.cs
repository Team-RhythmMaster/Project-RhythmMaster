using UnityEngine;

// 일반 노트
public class ShortNote : NoteObject
{
    public override void TryHit()
    {
        float currentTime = AudioManager.Instance.songTime + offset;

        float diff = Mathf.Abs(noteTime - currentTime);

        if (diff <= perfect)
        {
            JudgeManager.Instance.Judge("Perfect");
            Hit();
        }
        else if (diff <= great)
        {
            JudgeManager.Instance.Judge("Great");
            Hit();
        }
        else if (diff <= good)
        {
            JudgeManager.Instance.Judge("Good");
            Hit();
        }
        else if (diff <= bad)
        {
            JudgeManager.Instance.Judge("Bad");
            Hit();
        }
        else
        {
            JudgeManager.Instance.Judge("Miss");
        }
    }
}