using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScene_Intro : MonoBehaviour
{
    public void StartPlayScene()
    {
        SceneManager.LoadScene(1);
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
