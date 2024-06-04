using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private GameObject GameOverPanel;

    public bool IsGamePlaying;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        IsGamePlaying = true;
        GameOverPanel.SetActive(false);
    }

    void Update()
    {

    }

    public void GameOver()
    {
        IsGamePlaying = false;
        GameOverPanel.SetActive(true);
    }

}
