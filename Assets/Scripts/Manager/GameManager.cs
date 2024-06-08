using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject _gameOverPanel;

    [SerializeField]
    private GameObject _gameClearPanel;

    [SerializeField]
    private GameObject _pausePanel;

    [SerializeField]
    private Image _pauseButtonImage;

    [SerializeField]
    private Sprite playImage;

    [SerializeField]
    private Sprite pauseImage;

    public bool IsGamePlaying;

    public TextMeshProUGUI timeText;
    private float _playTime;
    public TextMeshProUGUI clearTimeText;
    private bool _isTimeOver;

    private bool _isPaused = false;


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
        _pausePanel.SetActive(false);
        _playTime = 0f;

    }

    void Update()
    {
        if (IsGamePlaying && !_isPaused)
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

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0 : 1;
        UpdatePauseUI();
    }

    public bool IsPaused
    {
        get { return _isPaused; }
        set
        {
            _isPaused = value;
            Time.timeScale = _isPaused ? 0 : 1;
            UpdatePauseUI();
        }
    }

    private void UpdatePauseUI()
    {
        _pauseButtonImage.sprite = _isPaused ? playImage : pauseImage;
        _pausePanel.SetActive(_isPaused);
    }

}
