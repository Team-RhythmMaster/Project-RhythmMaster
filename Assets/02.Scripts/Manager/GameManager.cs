using UnityEngine;
using Utils.EnumType;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {  get { return instance; } }

    public SceneType sceneType = SceneType.Intro;
    public GameState currentState;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    // √ ±‚»≠
    private void Init()
    {
        Application.targetFrameRate = 65;
        Screen.SetResolution(1920, 1080, true);
    }

    private void OnSceneLoad(Scene _scene, LoadSceneMode _mode)
    {
        if (_scene.name == "01.TitleScene")
        {
            sceneType = SceneType.Title;
        }
        else if (_scene.name == "02.MainScene")
        {
            sceneType = SceneType.Main;
        }
        else if (_scene.name == "03.RhythmScene")
        {
            sceneType = SceneType.Rhythm;
        }
        else if (_scene.name == "04.NurtureScene")
        {
            sceneType = SceneType.Nurture;
        }
    }
}