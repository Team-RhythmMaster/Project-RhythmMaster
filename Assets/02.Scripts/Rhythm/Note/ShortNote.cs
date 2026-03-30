using UnityEngine;
using Utils.EnumType;
using System.Collections;

public class ShortNote : NoteObject
{
    protected override void Start()
    {
        base.Start();
    }

    public override void TryHit()
    {
        diff = Mathf.Abs(data.time - currentTime);

        if (diff <= JudgeManager.bad)
        {
            isHit = true;

            if (diff <= JudgeManager.perfect)
                JudgeManager.Instance.Judgment(JudgeType.Perfect, this);
            else if (diff <= JudgeManager.great)
                JudgeManager.Instance.Judgment(JudgeType.Great, this);
            else if (diff <= JudgeManager.good)
                JudgeManager.Instance.Judgment(JudgeType.Good, this);
            else
                JudgeManager.Instance.Judgment(JudgeType.Bad, this);

            StartCoroutine(OnHit());
        }
    }

    private IEnumerator OnHit()
    {
        spriteRenderer.sprite = noteHitSprites[spriteIndex];
        yield return new WaitForSeconds(0.1f);
        Remove();
    }
}