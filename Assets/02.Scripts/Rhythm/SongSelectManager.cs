using UnityEngine;

public class SongSelectManager : MonoBehaviour
{
    private static SongSelectManager instance;
    public static SongSelectManager Instance { get { return instance; } }

    public SongDatabaseSO database;
    public SongDataSO CurrentSong => database.songs[currentIndex];
    private int currentIndex = 0;

    private delegate void OnSongChanged(SongDataSO song);
    private event OnSongChanged onSongChanged;

    private void Start()
    {
        NotifyChange();
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void NextSong()
    {
        currentIndex = (currentIndex + 1) % database.songs.Count;
        NotifyChange();
    }

    public void PrevSong()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = database.songs.Count - 1;

        NotifyChange();
    }

    // 외부에서 노래 변경 이벤트 구독
    private void NotifyChange()
    {
        onSongChanged?.Invoke(CurrentSong);
    }
}