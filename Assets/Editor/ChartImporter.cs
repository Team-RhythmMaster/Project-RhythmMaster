using System.IO;
using UnityEngine;
using UnityEditor;
using Utils.ClassUtility;

public class ChartImporter
{
    //[MenuItem("Tools/Import Chart JSON")]
    //public static void ImportJson()
    //{
    //    string path = EditorUtility.OpenFilePanel("Select Chart JSON", "", "json");

    //    if (string.IsNullOrEmpty(path)) return;

    //    string json = File.ReadAllText(path);
    //    ChartJson chart = JsonUtility.FromJson<ChartJson>(json);

    //    CreateSongData(chart);
    //}

    //static void CreateSongData(ChartJson chart)
    //{
    //    SongData song = ScriptableObject.CreateInstance<SongData>();

    //    song.title = chart.title;
    //    song.artist = chart.artist;
    //    song.bpm = chart.bpm;
    //    song.offset = chart.offset;

    //    foreach (var n in chart.notes)
    //    {
    //        NoteData note = new NoteData
    //        {
    //            beat = n.beat,
    //            lane = n.lane,
    //            type = ParseType(n.type)
    //        };

    //        song.notes.Add(note);
    //    }

    //    string savePath = $"Assets/Songs/{song.title}.asset";

    //    AssetDatabase.CreateAsset(song, savePath);
    //    AssetDatabase.SaveAssets();

    //    Debug.Log("SongData Created: " + savePath);
    //}

    //static NoteType ParseType(string type)
    //{
    //    switch (type)
    //    {
    //        case "Tap": return NoteType.Tap;
    //        case "Hold": return NoteType.Hold;
    //        default: return NoteType.Tap;
    //    }
    //}
}
