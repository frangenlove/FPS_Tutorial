using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    string newGameScene = "SampleScene";
    public TMP_Text highScoreUI;


    void Start()
    {
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Top Wave Survived: {highScore}";
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        // 如果在编辑器中运行，使用UnityEditor.EditorApplication.isPlaying = false;
        // 但在构建的游戏中，直接退出应用程序
        UnityEditor.EditorApplication.isPlaying = false; // 仅在编辑器中有效
#else
        Application.Quit();
#endif

    }
}
