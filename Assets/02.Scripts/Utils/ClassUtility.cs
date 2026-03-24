using System;
using UnityEngine;
using Utils.EnumType;
using System.Collections.Generic;

namespace Utils.ClassUtility
{
    // Score 정보
    public struct ScoreData
    {
        public int great;
        public int good;
        public int miss;
        public int fastMiss; // 빨리 입력해서 미스
        public int longMiss; // 롱노트 완성 실패, miss 카운트는 하지 않음

        public string[] judgeText;
        public Color[] judgeColor;
        public JudgeType judge;
        public int combo;
        public int score
        {
            get
            {
                return (great * 500) + (good * 200);
            }
            set
            {
                score = value;
            }
        }
    }

    [Serializable]
    public class Note
    {
        public NoteType type; // 노트 타입 (0: Short, 1: Long)
        public float time;    // 노트 도착 시간
        public float tail;    // 롱노트 끝 시간
        public int line;      // 레인 index
    }

    [Serializable]
    public class Sheet
    {
        // Description
        public string title;
        public string artist;

        // Audio
        public int bpm;
        public int offset;
        public int[] signature;

        // Note
        public List<Note> notes = new List<Note>();


        public AudioClip clip;
        public Sprite img;

        public float BarPerSec { get; private set; }
        public float BeatPerSec { get; private set; }

        public int BarPerMilliSec { get; private set; }
        public int BeatPerMilliSec { get; private set; }

        public void Init()
        {
            BarPerMilliSec = (int)(signature[0] / (bpm / 60f) * 1000);
            BeatPerMilliSec = BarPerMilliSec / 64;

            BarPerSec = BarPerMilliSec * 0.001f;
            BeatPerSec = BarPerMilliSec / 64f;
        }
    }
}