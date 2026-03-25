using UnityEngine;
using Utils.EnumType;
using Utils.ClassUtility;
using System.Collections;
using System.Collections.Generic;

// Note 판정
public class Judgement : MonoBehaviour
{
    private readonly int miss = 600;
    private readonly int good = 400;
    private readonly int great = 250;

    private List<Queue<Note>> notes = new List<Queue<Note>>();
    private Queue<Note> note1 = new Queue<Note>();
    private Queue<Note> note2 = new Queue<Note>();
    private Queue<Note> note3 = new Queue<Note>();
    private Queue<Note> note4 = new Queue<Note>();

    private int[] longNoteCheck = new int[4] { 0, 0, 0, 0 };

    private int curruntTime = 0;
    public int judgeTimeFromUserSetting = 0;

    private Coroutine coCheckMiss;
    private Score score;

    public void Init()
    {
        foreach (var note in notes)
        {
            note.Clear();
        }
        notes.Clear();

        //foreach (var note in GameManager.Instance.sheets[GameManager.Instance.title].notes)
        //{
        //    if (note.line == 1)
        //        note1.Enqueue(note);
        //    else if (note.line == 2)
        //        note2.Enqueue(note);
        //    else if (note.line == 3)
        //        note3.Enqueue(note);
        //    else
        //        note4.Enqueue(note);
        //}
        notes.Add(note1);
        notes.Add(note2);
        notes.Add(note3);
        notes.Add(note4);

        if (coCheckMiss != null)
        {
            StopCoroutine(coCheckMiss);
        }
        coCheckMiss = StartCoroutine(IECheckMiss());
    }

    public void Judge(int line)
    {
        if (notes[line].Count <= 0 || !AudioManager.Instance.IsPlaying())
            return;

        Note note = notes[line].Peek();
        float judgeTime = curruntTime - note.time + judgeTimeFromUserSetting;

        if (judgeTime < miss && judgeTime > -miss)
        {
            if (judgeTime < good && judgeTime > -good)
            {
                if (judgeTime < great && judgeTime > -great)
                {
                    score.data.great++;
                    score.data.judge = JudgeType.Great;
                }
                else
                {
                    score.data.good++;
                    score.data.judge = JudgeType.Good;
                }
                score.data.combo++;
            }
            else
            {
                score.data.fastMiss++;
                score.data.judge = JudgeType.Miss;
                score.data.combo = 0;
            }
            score.SetScore();
            //JudgeEffect.Instance.OnEffect(line);

            if (note.type == (int)NoteType.Short)
            {
                notes[line].Dequeue();
            }
            else if (note.type == NoteType.Long)
            {
                longNoteCheck[line] = 1;
            }
        }
    }

    public void CheckLongNote(int line)
    {
        if (notes[line].Count <= 0)
            return;

        Note note = notes[line].Peek();
        if (note.type != NoteType.Long)
            return;

        float judgeTime = curruntTime - note.endTime + judgeTimeFromUserSetting;
        if (judgeTime < good && judgeTime > -good)
        {
            if (judgeTime < great && judgeTime > -great)
            {
                score.data.great++;
                score.data.judge = JudgeType.Great;
                score.data.combo++;
            }
            else
            {
                score.data.longMiss++;
            }
            score.SetScore();
            longNoteCheck[line] = 0;
            notes[line].Dequeue();
        }
    }

    private IEnumerator IECheckMiss()
    {
        while (true)
        {
            curruntTime = (int)AudioManager.Instance.GetMilliSec();

            for (int i = 0; i < notes.Count; i++)
            {
                if (notes[i].Count <= 0)
                    break;
                Note note = notes[i].Peek();
                float judgeTime = note.time - curruntTime + judgeTimeFromUserSetting;

                if (note.type == NoteType.Long)
                {
                    if (longNoteCheck[note.lane - 1] == 0) // Head가 판정처리가 안된 경우
                    {
                        if (judgeTime < -miss)
                        {
                            score.data.miss++;
                            score.data.judge = JudgeType.Miss;
                            score.data.combo = 0;
                            score.SetScore();
                            notes[i].Dequeue();
                        }
                    }
                }
                else
                {
                    if (judgeTime < -miss)
                    {
                        score.data.miss++;
                        score.data.judge = JudgeType.Miss;
                        score.data.combo = 0;
                        score.SetScore();
                        notes[i].Dequeue();
                    }
                }
            }

            yield return null;
        }
    }
}