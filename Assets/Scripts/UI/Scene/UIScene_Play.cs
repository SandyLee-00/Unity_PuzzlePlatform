using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScene_Play : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerMovement = GameObject.FindWithTag(Define.PlayerTag).GetComponent<PlayerMovement>();
        SoundManager.Instance.Play(Define.Sound.Bgm, "PlayBGM");
    }

    public void StartIntroScene()
    {
        SceneManager.LoadScene((int)Define.Scene.Title);
        GameManager.Instance.IsGamePlaying = false;
    }

    public void StartPlayScene()
    {
        SceneManager.LoadScene((int)Define.Scene.Play);
        GameManager.Instance.IsGamePlaying = true;
    }

    public void OnContinueButton()
    {
        GameManager.Instance.IsPaused = false; // Pause 상태 해제
        _playerMovement.ToggleCursor();
    }

    public void OnSaveButton()
    {

    }
}
