using UnityEngine;
using System.Linq;
using DG.Tweening;
using Utils.EnumType;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance {  get { return instance; } }

    private Transform[] hitLines;

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
        hitLines = GameObject.Find("HitLine").GetComponentsInChildren<Transform>().Skip(1).ToArray();
    }

    private void Update()
    {
        if (GameManager.Instance.sceneType == SceneType.Rhythm)
        {
            CheckInput(0, KeyCode.F);
            CheckInput(1, KeyCode.J);
        }
    }

    private void CheckInput(int _lane, KeyCode _key)
    {
        // 키 눌렀을 때 판정 시도
        if (Input.GetKeyDown(_key))
        {
            NoteManager.Instance.TryHit(_lane);

            hitLines[_lane].DOKill(); // 기존 트윈 제거
            hitLines[_lane].localScale = Vector3.one; // 스케일 초기화
            hitLines[_lane].transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 5, 0.5f);
        }

        // 롱노트 유지 여부 전달
        NoteManager.Instance.Hold(_lane, Input.GetKey(_key));
    }
}