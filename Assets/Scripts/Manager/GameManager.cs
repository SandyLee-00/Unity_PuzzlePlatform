using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private Canvas canvas;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        Transform APanelTransform = canvas.transform.Find("GameOverPanel");
        _gameOverPanel = APanelTransform.gameObject;

        Transform BPanelTransform = canvas.transform.Find("GameClearPanel");
        _gameClearPanel = BPanelTransform.gameObject;

        Transform CPanelTransform = canvas.transform.Find("PausePanel");
        _pausePanel = CPanelTransform.gameObject;

        _pauseButtonImage = GameObject.Find("PauseButton").GetComponent<Image>();

        timeText = GameObject.Find("TimeText").GetComponent<TextMeshProUGUI>();

        Transform DTransform = BPanelTransform.Find("ClearTimeBG");
        Transform ETransform = DTransform.Find("ClearTimeText");
        clearTimeText = ETransform.gameObject.GetComponent<TextMeshProUGUI>();

        // 초기화
        InitializeGame();
    }

    private void InitializeGame()
    {
        IsGamePlaying = true;
        _gameOverPanel?.SetActive(false);
        _gameClearPanel?.SetActive(false);
        _pausePanel?.SetActive(false);
        _playTime = 0f;
        Time.timeScale = 1;
        _isPaused = false;
        _isTimeOver = false;
    }

    void Start()
    {
        InitializeGame();
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
        _gameOverPanel?.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameClear()
    {
        IsGamePlaying = false;
        _isTimeOver = true;
        _gameClearPanel?.SetActive(true);
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
        _pausePanel?.SetActive(_isPaused);
    }
}
