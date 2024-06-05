using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public GameObject pausePanel;
    public Sprite playImage;
    public Sprite pauseImage;

    private Image _pauseButtonImage;
    private bool isPaused = false;
    private Button _button;


    void Start()
    {
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(TogglePause);

        _pauseButtonImage = GetComponent<Image>();
        _pauseButtonImage.sprite = pauseImage;
        pausePanel.SetActive(false);
    }

    public void TogglePause()
    {
        Debug.Log("클릭");
        if (isPaused)
        {
            
            Time.timeScale = 1;
            isPaused = false;
            _pauseButtonImage.sprite = pauseImage;
            pausePanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
            _pauseButtonImage.sprite = playImage;
            pausePanel.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(TogglePause);
    }
}
