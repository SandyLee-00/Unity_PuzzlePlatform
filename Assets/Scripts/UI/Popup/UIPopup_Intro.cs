using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPopup_Intro : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.Play(Define.Sound.Bgm, "TitleBGM");
    }

    public void StartPlayScene(int mode)
    {
        SceneManager.LoadScene((int)Define.Scene.Play);
        GameManager.Instance.IsGamePlaying = true;
        if (mode == 2)
            GameManager.Instance.IsLoadData = true;
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
