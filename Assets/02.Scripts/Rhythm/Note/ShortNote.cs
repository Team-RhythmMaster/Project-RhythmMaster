using UnityEngine;
using Utils.EnumType;
using System.Collections;

public class ShortNote : NoteObject
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void TryHit()
    {
        diff = Mathf.Abs(data.time - currentTime);

        if (diff <= ScoreManager.bad)
        {
            isHit = true;

            if (diff <= ScoreManager.perfect)
                ScoreManager.Instance.Judgment(JudgeType.Perfect, this);
            else if (diff <= ScoreManager.great)
                ScoreManager.Instance.Judgment(JudgeType.Great, this);
            else if (diff <= ScoreManager.good)
                ScoreManager.Instance.Judgment(JudgeType.Good, this);
            else
                ScoreManager.Instance.Judgment(JudgeType.Bad, this);

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