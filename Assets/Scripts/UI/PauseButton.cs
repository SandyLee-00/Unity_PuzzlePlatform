using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private Button _button;

    void Start()
    {
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(TogglePause);
    }

    public void TogglePause()
    {
        GameManager.Instance.TogglePause(); // GameManager의 TogglePause 메서드를 호출
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(TogglePause);
    }
}
