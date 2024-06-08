using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public UserData saveData;

    const string savePath = "/Data.txt";    //저장경로

    public void SaveData()
    {
        var json = JsonUtility.ToJson(saveData);

        File.WriteAllText(Application.persistentDataPath + savePath, json);
    }

    public void LoadData()
    {
        var json = File.ReadAllText(Application.persistentDataPath + savePath);

        saveData = JsonUtility.FromJson<UserData>(json);
    }
}

[System.Serializable]
public class UserData
{
    //위치
    [Header("Player Transform")]
    public Vector3 Position;
    public Quaternion Rotation;

    //인벤토리
}