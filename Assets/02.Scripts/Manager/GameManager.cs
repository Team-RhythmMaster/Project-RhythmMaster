using UnityEngine;
using Utils.EnumType;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {  get { return instance; } }

    public SceneType sceneType = SceneType.Intro;
    public GameState state = GameState.Game;

    private NoteGenerator noteGenerator;

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
        noteGenerator.Init();
        AudioManager.Instance.Play();

        yield return new WaitForSeconds(AudioManager.Instance.noteTravelTime);

        noteGenerator.isSpawning = true;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 초기화
    private void Init()
    {
        Application.targetFrameRate = 65;
        Screen.SetResolution(1920, 1080, true);
        noteGenerator = FindAnyObjectByType<NoteGenerator>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "01.TitleScene")
        {
            sceneType = SceneType.Title;
        }
        else if (scene.name == "02.MainScene")
        {
            sceneType = SceneType.Main;
        }
        else if (scene.name == "03.RhythmScene")
        {
            sceneType = SceneType.Rhythm;
        }
        else if (scene.name == "04.NurtureScene")
        {
            sceneType = SceneType.Nurture;
        }
    }
}