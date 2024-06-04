using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    private Button _button;

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
        }
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }
}
