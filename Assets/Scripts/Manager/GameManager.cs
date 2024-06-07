using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject _gameOverPanel;

    [SerializeField]
    private GameObject _gameClearPanel;

    public bool IsGamePlaying;

    public TextMeshProUGUI timeText;
    private float _playTime;
    public TextMeshProUGUI clearTimeText;
    private bool _isTimeOver;


    private void Awake()
    {
        if (_componentInstance == null)
        {
            _componentInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_componentInstance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        IsGamePlaying = true;
        _gameOverPanel.SetActive(false);
        _gameClearPanel.SetActive(false);
        _playTime = 0f;
    }

    void Update()
    {
        if (IsGamePlaying)
        {
            _playTime += Time.deltaTime;
            UpdatePlayTimeText(timeText);
        }
    }

    public void GameOver()
    {
        IsGamePlaying = false;
        _isTimeOver = true;
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameClear()
    {
        IsGamePlaying = false;
        _isTimeOver = true;
        _gameClearPanel.SetActive(true);
        UpdatePlayTimeText(clearTimeText);
        Time.timeScale = 0;
    }

    private void UpdatePlayTimeText(TextMeshProUGUI text)
    {
        int minutes = Mathf.FloorToInt(_playTime / 60F);
        int seconds = Mathf.FloorToInt(_playTime % 60F);
        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


}
