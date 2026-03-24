using UnityEngine;

// 모든 노트의 기본 동작을 담당
public abstract class NoteObject : MonoBehaviour
{
    public int lane;              // 어떤 레인에 속하는지 (0=왼쪽, 1=오른쪽)
    public float noteTime;        // 이 노트를 눌러야 하는 시간 (초)
    public float speed = 3f;      // 이동 속도 (시각적인 요소)
    public float offset = -1.0f;  // 음악과 노트 위치 싱크 조정 (초)
    public float hitLineX = -2.0f;// 판정선 위치

    // 판정 기준 시간
    public float perfect = 0.05f;
    public float great = 0.1f;
    public float good = 0.15f;
    public float bad = 0.2f;
    public float miss = 0.25f;

    private bool isHit = false;

    void Start()
    {
        // 노트 생성 시 자동 등록
        NoteManager.Instance.Register(this);
    }

    void Update()
    {
        float currentTime = AudioManager.Instance.songTime + offset;

        // 시간 기반 위치 계산
        float x = hitLineX + (noteTime - currentTime) * speed;
        // lane별로 y 위치 다르게 (위/아래)
        transform.position = new Vector3(x, lane * 1.5f, 0);

        CheckMiss(currentTime);
    }

    // Miss 자동 처리
    void CheckMiss(float currentTime)
    {
        if (!isHit && currentTime - noteTime > miss)
        {
            Debug.Log("Miss");
            Remove();
        }
    }

    // 입력 판정
    public void TryHit()
    {
        float currentTime = AudioManager.Instance.songTime;
        float diff = Mathf.Abs(noteTime - currentTime);

        if (diff <= perfect)
        {
            Debug.Log("Perfect");
            Hit();
        }
        else if (diff <= great)
        {
            Debug.Log("Great");
            Hit();
        }
        else if (diff <= good)
        {
            Debug.Log("Good");
            Hit();
        }
        else if(diff <= bad)
        {
            Debug.Log("Bad");
            Hit();
        }
        else
        {
            Debug.Log("Miss");
        }
    }

    void Hit()
    {
        isHit = true;
        Remove();
    }

    void Remove()
    {
        // 리스트에서 제거
        NoteManager.Instance.Unregister(this);
        Destroy(gameObject);
    }
}