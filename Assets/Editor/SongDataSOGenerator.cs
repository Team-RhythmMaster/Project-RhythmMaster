using System.IO;
using UnityEditor;
using UnityEngine;
using Utils.EnumType;
using Utils.ClassUtility;
using System.Collections.Generic;

public class SongDataSOGenerator
{
    private const string dataPath = "Assets/Resources/Data/JSON/SongData.json";
    private const string songDataSaveFolder = "Assets/08.ScriptableObjects/Songs";
    private const string songDatabaseSaveFolder = "Assets/08.ScriptableObjects/Database/songDatabase.asset";

    [MenuItem("Tools/Generate SongDataSO")]
    public static void Generate()
    {
        string data = File.ReadAllText(dataPath);
        SongDataList dataList = JsonUtility.FromJson<SongDataList>(data);

        ClearFolder(songDataSaveFolder);
        List<SongDataSO> songDataList = new List<SongDataSO>();

        foreach (var _data in dataList.Songs)
        {
            SongDataSO so = ScriptableObject.CreateInstance<SongDataSO>();

            so.id = _data.id;
            so.bpm = _data.bpm;
            so.title = _data.title;
            so.artist = _data.artist;
            so.difficulty = (Difficulty)_data.difficulty;

            so.audioClip = FindAsset<AudioClip>(_data.audioClip, "06.Sounds");
            so.songSprite = FindAsset<Sprite>(_data.songSprite, "04.Images");
            so.notes = new List<NoteData>();

            foreach (var n in _data.notes)
            {
                NoteData note = new NoteData
                {
                    lane = n.lane,
                    time = n.time,
                    endTime = n.endTime
                };

                so.notes.Add(note);
            }

            string savePath = $"{songDataSaveFolder}/{so.title}.asset";
            AssetDatabase.CreateAsset(so, savePath);
            songDataList.Add(so);
        }

        CreateOrUpdateDatabase(songDataList);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("SongData Created");
    }

    // DatabaseSO 생성
    static void CreateOrUpdateDatabase(List<SongDataSO> _list)
    {
        SongDatabaseSO db = AssetDatabase.LoadAssetAtPath<SongDatabaseSO>(songDatabaseSaveFolder);

        if (db == null)
        {
            db = ScriptableObject.CreateInstance<SongDatabaseSO>();
            AssetDatabase.CreateAsset(db, songDatabaseSaveFolder);
        }

        db.songs = _list;
        EditorUtility.SetDirty(db);
    }

    // 폴더 초기화 기존 데이터 삭제 → 중복 방지
    static void ClearFolder(string _folderPath)
    {
        if (!AssetDatabase.IsValidFolder(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
            AssetDatabase.Refresh();
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:SongDataSO", new[] { _folderPath });

        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AssetDatabase.DeleteAsset(path);
        }
    }

    // Asset 찾기
    static T FindAsset<T>(string _fileName, string _folderFilter) where T : Object
    {
        string[] guids = AssetDatabase.FindAssets($"{_fileName} t:{typeof(T).Name}");

        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            // 특정 폴더만 허용
            if (path.Contains(_folderFilter))
            {
                return AssetDatabase.LoadAssetAtPath<T>(path);
            }
        }

        Debug.LogError($"{typeof(T).Name} not found: {_fileName}");
        return null;
    }
}
