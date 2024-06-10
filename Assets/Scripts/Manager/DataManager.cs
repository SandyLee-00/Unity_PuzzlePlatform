using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    public UserData saveData;
    private PlayerHeartStamina heartStamina;

    private string savePath = Path.Combine(Application.dataPath, "Data.json");    //저장경로

    private void Awake()
    {
        heartStamina = GetComponent<PlayerHeartStamina>();
    }

    private void Start()
    {
        //데이터가 없으면 기본세팅
        //있으면 json로드
        try
        {
            LoadData();
        }
        catch
        {
            heartStamina.SettingBasic();
        }
        finally
        {
            Debug.Log(heartStamina.CurrentHeart);
            Debug.Log(heartStamina.CurrentStamina);
        }
    }

    private void SetPlayerProperties()
    {
        //플레이어
        saveData.Position = transform.position;
        saveData.Rotation = transform.rotation;
        saveData.Heart = heartStamina.CurrentHeart;
        saveData.Stamina = heartStamina.CurrentStamina;

        //인벤토리

    }

    private void GetPlayerProperties()
    {
        //플레이어
        transform.position = saveData.Position;
        transform.rotation = saveData.Rotation;
        heartStamina.LoadFromSaveData(saveData.Heart, saveData.Stamina);
    }

    [ContextMenu("To JsonData")]
    public void SaveData()
    {
        SetPlayerProperties();
        var json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(savePath, json);
    }

    public void LoadData()
    {
        var json = File.ReadAllText(savePath);

        saveData = JsonUtility.FromJson<UserData>(json);

        GetPlayerProperties();
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
}