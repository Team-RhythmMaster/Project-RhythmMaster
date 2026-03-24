using UnityEngine;

namespace Utils.EnumType
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
}