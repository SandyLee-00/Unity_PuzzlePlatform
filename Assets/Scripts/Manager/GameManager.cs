using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private GameObject _gameOverPanel;
    private GameObject _gameClearPanel;
    private GameObject _pausePanel;
    private Image _pauseButtonImage;
    private Sprite _playImage;
    private Sprite _pauseImage;
    private float _playTime;
    private bool _isPaused = false;
    private Canvas _canvas;

    public bool IsGamePlaying;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI clearTimeText;

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
        // 현재 씬이 Title 이면 OnSceneLoaded 함수 종료
        if(SceneManager.GetSceneByBuildIndex(0) == scene)
        {
            return;
        }

        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        Transform APanelTransform = _canvas.transform.Find("GameOverPanel");
        _gameOverPanel = APanelTransform.gameObject;

        Transform BPanelTransform = _canvas.transform.Find("GameClearPanel");
        _gameClearPanel = BPanelTransform.gameObject;

        Transform CPanelTransform = _canvas.transform.Find("PausePanel");
        _pausePanel = CPanelTransform.gameObject;


        timeText = GameObject.Find("TimeText").GetComponent<TextMeshProUGUI>();

        Transform DTransform = BPanelTransform.Find("ClearTimeBG");
        Transform ETransform = DTransform.Find("ClearTimeText");
        clearTimeText = ETransform.gameObject.GetComponent<TextMeshProUGUI>();

        _pauseButtonImage = _canvas.transform.Find("PauseButton").GetComponent<Image>();

        _playImage = Resources.Load<Sprite>("Sprites/play");
        _pauseImage = Resources.Load<Sprite>("Sprites/pause");
        _pauseButtonImage.sprite = _pauseImage;

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
        _gameOverPanel?.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameClear()
    {
        IsGamePlaying = false;
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
        _pauseButtonImage.sprite = _isPaused ? _playImage : _pauseImage;
        _pausePanel?.SetActive(_isPaused);
    }
}
