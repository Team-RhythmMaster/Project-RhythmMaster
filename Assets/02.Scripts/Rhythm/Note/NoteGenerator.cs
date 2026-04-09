using UnityEngine;
using Utils.ClassUtility;

// 노트 생성 및 타이밍 제어
public class NoteGenerator : MonoBehaviour
{
    public SongDataSO songData;

    public float speed { get { return noteSpeed; } }
    private float noteSpeed = 3.5f;

    private float spawnAheadTime; // 노트가 판정선까지 이동하는 데 걸리는 시간
    private int spawnIndex = 0;   // 다음에 생성할 노트 인덱스
    private float rightEdge;      // 화면 오른쪽 끝 위치
    private float spawnX;         // 노트 시작 위치

    public bool isSpawning = false; // 노트 생성 여부

    private void Update()
    {
        if (!isSpawning) 
            return;

        while (spawnIndex < songData.notes.Count)
        {
            // 도착 시간 - 이동 시간 = 생성 시간
            if (songData.notes[spawnIndex].time <= AudioManager.Instance.songTime + spawnAheadTime)
            {
                Spawn(songData.notes[spawnIndex]);
                spawnIndex++;
            }
            else
                break;
        }
    }

    public void Init()
    {
        songData.notes.Sort((a, b) => a.time.CompareTo(b.time));

        rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        spawnX = rightEdge + 3.0f;
        // 거리 / 속도 = 시간 (+1.0f → 여유 시간/렉 방지/프레임 드랍 방지)
        spawnAheadTime = ((spawnX - NoteManager.hitLineX) / speed) + 1.0f;

        // 초기 생성 → 게임 시작 시 이미 보여야 할 노트들 미리 생성
        while (spawnIndex < songData.notes.Count)
        {
            if (songData.notes[spawnIndex].time <= AudioManager.Instance.songTime + spawnAheadTime)
            {
                Spawn(songData.notes[spawnIndex]);
                spawnIndex++;
            }
            else
                break;
        }
    }

    // 노트 화면에 생성
    private void Spawn(NoteData _data)
    {
        float y = NoteManager.Instance.GetLaneY(_data.lane);
        NoteObject note = NoteManager.Instance.GetNote(_data);

        note.transform.position = new Vector3(spawnX, y, 0);
        note.Init(_data, speed);
    }

    // 노트 리스트에 추가
    public void AddNote(NoteData _note)
    {
        //notes.Add(_note);
        //notes.Sort((a, b) => a.time.CompareTo(b.time));
    }
}