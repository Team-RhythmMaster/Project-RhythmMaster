using UnityEngine;

public class NoteShort : NoteObject
{
    protected override void SetVisualPosition(float _x)
    {
        transform.position = new Vector3(_x, transform.position.y, 0);
    }
}