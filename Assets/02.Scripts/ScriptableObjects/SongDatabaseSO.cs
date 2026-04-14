using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SongDatabase", menuName = "Rhythm/SongDatabase")]
public class SongDatabaseSO : ScriptableObject
{
    public List<SongDataSO> songs;
}