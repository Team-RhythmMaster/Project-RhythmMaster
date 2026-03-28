using UnityEngine;
using Utils.EnumType;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {  get { return instance; } }

    private NoteGenerator noteGenerator;
    public GameState state = GameState.Game;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Init();
    }

    private IEnumerator Start()
    {
        // 모든 초기화 끝날 때까지 한 프레임 대기
        yield return null;
        noteGenerator.Init();
        AudioManager.Instance.Play();
    }

    private void Init()
    {
        Application.targetFrameRate = 65;
        Screen.SetResolution(1920, 1080, true);

        noteGenerator = FindAnyObjectByType<NoteGenerator>();
    }
}