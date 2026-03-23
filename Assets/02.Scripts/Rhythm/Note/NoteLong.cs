using UnityEngine;

public class NoteLong : NoteObject
{
    public Transform head;
    public Transform tail;
    public LineRenderer line;

    protected override void SetVisualPosition(float y)
    {
        float currentTime = conductor.songPosition;

        float headY = (note.time - currentTime) * GetSpeed();
        float tailY = (note.tail - currentTime) * GetSpeed();

        head.position = new Vector3(head.position.x, headY, 0);
        tail.position = new Vector3(tail.position.x, tailY, 0);

        transform.position = head.position;

        Vector3 linePos = tail.position - head.position;
        linePos.x = 0;
        linePos.z = 0;

        line.SetPosition(1, linePos);
    }
}