using UnityEngine;
using Utils.EnumType;

// 일반 노트
public class ShortNote : NoteObject
{
    public override void TryHit()
    {
        float currentTime = AudioManager.Instance.songTime + offset;
        float diff = Mathf.Abs(GetTime() - currentTime);

        if (diff <= JudgeManager.bad)
        {
            isHit = true;

            if (diff <= JudgeManager.perfect) 
                JudgeManager.Instance.Judge(JudgeType.Perfect);
            else if (diff <= JudgeManager.great) 
                JudgeManager.Instance.Judge(JudgeType.Great);
            else if (diff <= JudgeManager.good) 
                JudgeManager.Instance.Judge(JudgeType.Good);
            else 
                JudgeManager.Instance.Judge(JudgeType.Bad);

            Remove();
        }
    }
}