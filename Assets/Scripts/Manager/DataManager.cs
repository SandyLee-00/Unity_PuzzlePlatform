using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    public UserData saveData;
    private PlayerHeartStamina heartStamina;

    private void Start()
    {
        heartStamina = GetComponent<PlayerHeartStamina>();
    }

    private string savePath = Path.Combine("/Assets/SaveData/", "Data.json");    //저장경로

    private void SetPlayerProperties()
    {
        //플레이어
        saveData.Position = transform.position;
        saveData.Rotation = transform.rotation;
        saveData.Heart = heartStamina.CurrentHeart;
        saveData.Stamina = heartStamina.CurrentStamina;

        //인벤토리

    }

    [ContextMenu("To JsonData")]
    public void SaveData()
    {
        SetPlayerProperties();
        var json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(Application.dataPath + savePath, json);
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