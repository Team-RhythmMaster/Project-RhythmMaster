using System;
using Utils.EnumType;
using System.Collections.Generic;

namespace Utils.ClassUtility
{
    [Serializable]
    public class SongDataList
    {
        public SongData[] Songs;
    }

    [Serializable]
    public class SongData
    {
        public int id;          // 곡 고유 ID
        public int bpm;         // BPM (Beats Per Minute)
        public string title;    // 곡 제목
        public string artist;   // 아티스트 이름
        public int difficulty;  // 난이도 (0: Easy, 1: Normal, 2: Hard)

        public NoteData[] notes; // 노트 정보 배열
        public string audioClip; // 오디오 파일 경로
        public string songSprite;// 이미지 파일 경로
    }

    [Serializable]
    public class NoteData
    {
        public int lane;      // 레인 위치
        public float speed;   // 이동 속도
        public float time;    // 판정선 도착시간
        public float endTime; // longNote 끝시간
        public bool IsLong => endTime > time; // longNote 여부 확인
    }

    // Runtime Data (실행 데이터 → 저장 X)
    [Serializable]
    public class ScoreData
    {
        public int score;
        public int combo;
        public int maxCombo;

        public int perfect;
        public int great;
        public int good;
        public int bad;
        public int miss;
    }

    // persistent Data (저장 데이터 → 저장 O)
    [Serializable]
    public class SaveData
    {
        public List<SongRecord> songRecords;
    }

    // 곡별 기록 데이터
    [Serializable]
    public class SongRecord
    {
        public int songID;
        public RankType rank;
        public int maxScore;
        public int maxCombo;
        public bool isUnlocked;
    }
}