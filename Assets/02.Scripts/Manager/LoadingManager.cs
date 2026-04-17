using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using Utils.EnumType;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    private static LoadingManager instance;
    public static LoadingManager Instance { get { return instance; } }

    public PlayerController player;
    public SkeletonAnimation skeletonAnimation;

    public NoteManager noteManager;
    public RhythmPartManager rhythmManager;

    public GameObject loadingUI;
    public Image fadeImage;

    private float fadeDuration = 0.5f;
    private float minLoadingTime = 1.0f;

    private bool isLoading = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // 곡 종료 시 호출
    public void OnSongEnd()
    {
        if (isLoading)
            return;

        GameManager.Instance.currentState = GameState.Ending;
        StartCoroutine(LoadingRoutine());
    }

    // 게임 시작
    public void StartGame()
    {
        GameManager.Instance.currentState = GameState.Playing;

        //AudioManager.Instance.StartTiming();
        //audioSource.time = 0f;
        //audioSource.Play();
    }

    // 전체 초기화
    private void ResetGame()
    {
        // 오디오 초기화
        //audioSource.Stop();
        //audioSource.time = 0f;

        // 리듬 타이밍 초기화
        //rhythmManager.ResetTiming();
        // 점수 초기화
        //scoreManager.Reset();
        // 노트 초기화
        noteManager.ResetNotes();
        // 캐릭터 초기화
        player.ResetState();
    }

    // 로딩 루틴
    private IEnumerator LoadingRoutine()
    {
        isLoading = true;
        GameManager.Instance.currentState = GameState.Loading;

        // 로딩 UI ON
        loadingUI.SetActive(true);
        yield return StartCoroutine(Fade(0f, 1f));

        float startTime = Time.time;

        // 전체 초기화
        ResetGame();

        // 최소 로딩 시간 보장
        float elapsed = Time.time - startTime;
        if (elapsed < minLoadingTime)
            yield return new WaitForSeconds(minLoadingTime - elapsed);

        // 페이드 아웃
        yield return StartCoroutine(Fade(1f, 0f));

        loadingUI.SetActive(false);

        // 다시 시작
        StartGame();
        isLoading = false;
    }

    // 페이드 연출
    private IEnumerator Fade(float _startAlpha, float _endAlpha)
    {
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(_startAlpha, _endAlpha, t / fadeDuration);

            fadeImage.color = new Color(color.r, color.g, color.b, alpha);

            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, _endAlpha);
    }
}