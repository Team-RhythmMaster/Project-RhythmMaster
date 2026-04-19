namespace Utils.EnumType
{
    // 게임씬 타입
    public enum SceneType
    {
        Intro,
        Title,
        Main,
        Rhythm,
        Nurture
    }

    // 게임 현재 상태
    public enum GameState
    {
        Intro,
        Playing,
        Ending,
        Loading
    }

    // 노래 재생 상태
    public enum MusicState
    {
        Playing,   // 재생 상태
        Paused,    // 일시 정지 상태
        Unpaused,  // 일시 정지 상태 해제
        Stop       // 정지 상태
    }

    // 노래 난이도
    public enum Difficulty 
    { 
        Easy, 
        Normal, 
        Hard 
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

    // 피버 상태
    public enum FeverState
    {
        Normal,
        Fever
    }

    // 노래 랭크
    public enum RankType
    {
        S,
        A,
        B,
        C,
        D,
        F
    }
}