using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScene_Intro : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.Play(Define.Sound.Bgm, "TitleBGM");
    }

    public void StartPlayScene()
    {
        SceneManager.LoadScene((int)Define.Scene.Play);
        GameManager.Instance.IsGamePlaying = true;
    }

    public void OnExitButton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
