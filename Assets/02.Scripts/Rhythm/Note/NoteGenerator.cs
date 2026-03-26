using UnityEngine;
using Utils.ClassUtility;
using System.Collections.Generic;

// Note 재생
public class NoteGenerator : MonoBehaviour
{
    public List<Note> notes = new List<Note>();

    [Header("설정값")]
    private float noteSpeed = 2.5f;
    public float speed { get { return noteSpeed; } }
    public Vector3 hitLine = new Vector3(-6.5f, 0, 0); // 판정선 위치

    [Header("내부 변수")]
    private int spawnIndex = 0;    // 현재 생성할 노트 인덱스
    private float spawnAheadTime;  // 미리 생성 시간
    private float rightEdge;       // 화면 오른쪽 끝 위치

    private void Start()
    {
        rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        spawnAheadTime = (rightEdge - hitLine.x) / speed; // 노트가 판정선까지 이동하는 데 걸리는 시간
        notes.Sort((a, b) => a.time.CompareTo(b.time));
    }

    private void Update()
    {
        Debug.Log(spawnAheadTime);
        float currentTime = AudioManager.Instance.songTime;

        while (spawnIndex < notes.Count)
        {
            Note data = notes[spawnIndex];

            if (data.time - currentTime <= spawnAheadTime)
            {
                Spawn(data);
                spawnIndex++;
            }
            else 
                break;
        }
    }

    // 노트 생성
    void Spawn(Note data)
    {
        float spawnX = rightEdge;
        Vector3 pos = new Vector3(spawnX, NoteManager.Instance.laneY[data.lane], 0);

        if (data.isHold)
        {
            LongNote note = NoteManager.Instance.GetLong();
            note.transform.position = pos;

            note.Init(data, speed);
        }
        else
        {
            ShortNote note = NoteManager.Instance.GetShort();
            note.transform.position = pos;

            note.Init(data, speed);
        }
    }

    public float GetSpawnAheadTime()
    {
        return spawnAheadTime;
    }
}