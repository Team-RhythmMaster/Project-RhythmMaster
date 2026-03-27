using UnityEngine;
using Utils.EnumType;

// 일반 노트
public class ShortNote : NoteObject
{
    public override void TryHit()
    {
        float currentTime = AudioManager.Instance.songTime + offset;
        float diff = Mathf.Abs(GetTime() - currentTime);

        if (diff <= bad)
        {
            isHit = true;

            if (diff <= perfect) 
                JudgeManager.Instance.Judge(JudgeType.Perfect);
            else if (diff <= great) 
                JudgeManager.Instance.Judge(JudgeType.Great);
            else if (diff <= good) 
                JudgeManager.Instance.Judge(JudgeType.Good);
            else 
                JudgeManager.Instance.Judge(JudgeType.Bad);

            Remove();
        }
    }
}