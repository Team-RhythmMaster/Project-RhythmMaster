using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "Rhythm/SongData")]
public class SongData : ScriptableObject
{
    public AudioClip clip;
    public float bpm;
}