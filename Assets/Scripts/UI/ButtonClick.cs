using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    private Button _button;

    public GameObject canvas;

    void Start()
    {
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        switch (gameObject.name)
        {
            case "BackButton" or "ExitButton":
                SceneManager.LoadScene("IntroScene");
                break;
            case "PlayButton" or "RetryButton":
                SceneManager.LoadScene("PlayScene");
                break;
            case "ContinueButton":
                GameManager.Instance.IsPaused = false; // Pause 상태 해제
                gameObject.transform.parent.gameObject.SetActive(false);
                break;
            case "CloseButton":
                GameManager.Instance.IsPaused = false; // Pause 상태 해제
                gameObject.transform.parent.gameObject.SetActive(false);
                break;
            case "CreditButton":
                Transform creditPanelTransform = canvas.transform.Find("CreditPanel");
                creditPanelTransform.gameObject.SetActive(true);
                break;
            case "SettingButton":
                Transform settingPanelTransform = canvas.transform.Find("SettingPanel");
                settingPanelTransform.gameObject.SetActive(true);
                break;
        }
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    
}
