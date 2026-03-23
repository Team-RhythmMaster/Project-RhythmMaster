using UnityEngine;

namespace Utils.GameDefinitions
{
    // 게임 상태
    public enum GameState
    {
        Game,
        Edit,
        MainMenu,
        GameOver
    }

    // 노래 난이도
    public enum Difficulty 
    { 
        Easy, 
        Normal, 
        Hard 
    }

    // 노래 재생 상태
    public enum MusicState
    {
        Playing,   // 재생 상태
        Paused,    // 일시 정지 상태
        Unpaused,  // 일시 정지 상태 해제
        Stop       // 정지 상태
    }

    // Note 종류
    public enum NoteType
    {
        Short,
        Long
    }

    // Note 판정 종류
    public enum JudgeType
    {
        Perfect,
        Great,
        Good,
        Bad,
        Miss
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