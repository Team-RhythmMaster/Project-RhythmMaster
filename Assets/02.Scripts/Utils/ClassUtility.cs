using System;
using UnityEngine;
using Utils.EnumType;
using System.Collections.Generic;

namespace Utils.ClassUtility
{
    [Serializable]
    public class NoteData
    {
        public bool IsLong => endTime > time; // longNote 여부 확인
        public int lane;      // 레인 위치
        public float speed;   // 이동 속도
        public float time;    // 판정선 도착시간
        public float endTime; // longNote 끝시간
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
        public List<NoteData> notes = new List<NoteData>();


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
}