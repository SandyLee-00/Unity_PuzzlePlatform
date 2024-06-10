using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class DataManager : Singleton<DataManager>
{
    public UserData saveData;
    public GameObject _player;
    private PlayerHeartStamina heartStamina;
    public UIInventory _inventory;

    private string savePath = Path.Combine(Application.dataPath, "Data.json");    //저장경로

    public event Action OnDataLoad;

    public EquippableItemData[] equipItems; //데이터를 로드할때

    private void Awake()
    {
        if (_componentInstance == null)
            _componentInstance = DataManager.Instance;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)Define.Scene.Play)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            heartStamina = _player.GetComponent<PlayerHeartStamina>();
        }

        saveData.Inventory.Clear();

        CheckLoadData();
    }

    private void CheckLoadData()
    {
        if (GameManager.Instance.IsLoadData)
        {
            Debug.Log("데이터 로드");
            LoadData();
        }
        else
        {
            heartStamina.SetHeart(heartStamina.MaxHeart);
            heartStamina.SetStamina(heartStamina.MaxStamina);
        }
    }

    private void SavePlayerProperties()
    {
        //플레이어
        saveData.Position = _player.transform.position;
        saveData.Rotation = _player.transform.rotation;
        saveData.Heart = heartStamina.CurrentHeart;
        saveData.Stamina = heartStamina.CurrentStamina;

        //인벤토리
        saveData.Inventory.Clear();
        foreach (EquippableItemData data in _inventory.GetItems())
        {
            if(data != null)
                saveData.Inventory.Add(new UserItemData(data.itemName, (int)data.type));
        }
    }

    private void LoadPlayerProperties()
    {
        //플레이어
        _player.transform.position = saveData.Position;
        _player.transform.rotation = saveData.Rotation;

        heartStamina.SetHeart(saveData.Heart);
        heartStamina.SetStamina(saveData.Stamina);

        Debug.Log($"Heart : {heartStamina.CurrentHeart}, Stamina : {heartStamina.CurrentStamina}");

        //인벤토리
        foreach(UserItemData data in saveData.Inventory)
        {
            if(data.Name == "고추")
            {
                _inventory.AddItem(equipItems[1]);
            }
            else if(data.Name == "버섯")
            {
                _inventory.AddItem(equipItems[0]);
            }
        }
    }

    public void SaveData()
    {
        SavePlayerProperties();
        var json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadData()
    {
        try
        {
            var json = File.ReadAllText(savePath);

            saveData = JsonUtility.FromJson<UserData>(json);
            LoadPlayerProperties();
        } 
        catch
        {
            heartStamina.SetHeart(heartStamina.MaxHeart);
            heartStamina.SetStamina(heartStamina.MaxStamina);
        }

        OnDataLoad?.Invoke();
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
    public List<UserItemData> Inventory;
}

[System.Serializable]
public class UserItemData
{
    public string Name;
    public int ItemType;    //0:Jump, 1:Speed

    public UserItemData(string name, int itemType)
    {  
        Name = name;
        ItemType = itemType;
    }
}