using UnityEngine;

// 음악 분석 및 Note 생성 시스템
public class NoteGenerator : MonoBehaviour
{
    // 기본 설정
    public GameObject notePrefab;
    public Vector3 spawnPos = new Vector3(10, 0, 0); // 노트 생성 위치 (화면 오른쪽 밖)

    public float bpm = 120f;      // 곡의 BPM
    public int totalBeats = 100;  // 총 몇 개의 박자를 생성할지
    public int laneCount = 2;     // 레인 개수 (0 ~ laneCount-1)
    public float speed = 3f;      // 노트 이동 속도 (판정선까지 도달하는 시간 계산에 사용)

    // 내부 계산 변수
    private float beatInterval;   // 한 박자 시간 (초)
    private float spawnAheadTime; // 노트를 미리 생성할 시간(화면 밖 → 판정선까지 이동 시간)
    private float rightEdge;      // 카메라 기준 화면 오른쪽 끝 좌표
    private float songStartTime;  // 노트 기준 시작 시간
    private int currentIndex = 0; // 현재 몇 번째 노트를 생성했는지

    private void Start()
    {
        rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        songStartTime = AudioManager.Instance.songTime;
        spawnAheadTime = rightEdge / speed;
        beatInterval = 60f / bpm;
    }

    private void Update()
    {
        float currentTime = AudioManager.Instance.songTime;

        // 노트 생성 루프
        while (currentIndex < totalBeats)
        {
            // 이 노트가 도착해야 할 시간
            float noteTime = songStartTime + currentIndex * beatInterval;

            // 생성 타이밍 조건
            if (noteTime - currentTime <= spawnAheadTime)
            {
                SpawnNote(noteTime);
                currentIndex++;
            }
            else
            {
                // 아직 생성할 타이밍이 아님
                break;
            }
        }
    }

    private void SpawnNote(float noteTime)
    {
        GameObject obj = Instantiate(notePrefab, spawnPos, Quaternion.identity);
        NoteObject note = obj.GetComponent<NoteObject>();

        // 노트 데이터 설정 (이 노트가 도착해야 하는 시간)
        note.noteTime = noteTime;
        note.lane = Random.Range(0, laneCount);
        note.speed = speed;
    }
}