using UnityEngine;
using System.IO;
using System.Collections.Generic;

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

        //saveData에 저장된 데이터를 플레이어에 추가
        /*
         * player.position = saveData.Position;
         * player.rotation = saveData.Rotation;
         * player.inventory = items;
         */
    }
}

[System.Serializable]
public class UserData
{
    //위치
    [Header("Player Transform")]
    public Vector3 Position;
    public Quaternion Rotation;

    //플레이어 속성
    [Header("Player Status")]
    public int Heart;
    public float Stamina;

    //인벤토리
    [Header("Player Inventory")]
    public List<ItemData> items;
}