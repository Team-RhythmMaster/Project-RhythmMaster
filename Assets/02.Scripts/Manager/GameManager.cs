using UnityEngine;
using Utils.ClassUtility;
using Utils.GameDefinitions;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {  get { return instance; } }

    public GameState state = GameState.Game;

    public Dictionary<string, Sheet> sheets = new Dictionary<string, Sheet>();
    public string title;
    Coroutine coPlaying;

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

    private void Init()
    {
        Application.targetFrameRate = 65;
        Screen.SetResolution(1920, 1080, true);
    }
}