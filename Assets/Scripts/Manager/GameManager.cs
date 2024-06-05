using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public TextMeshProUGUI timeText;
    private float _playTime;
    private bool _isTimeOver;


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
        _playTime = 0f;
    }

    void Update()
    {
        _playTime += Time.deltaTime;
        UpdatePlayTimeText();
    }

    public void GameOver()
    {
        IsGamePlaying = false;
        _isTimeOver = true;
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void UpdatePlayTimeText()
    {
        int minutes = Mathf.FloorToInt(_playTime / 60F);
        int seconds = Mathf.FloorToInt(_playTime % 60F);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
