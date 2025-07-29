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
        // ����ڱ༭�������У�ʹ��UnityEditor.EditorApplication.isPlaying = false;
        // ���ڹ�������Ϸ�У�ֱ���˳�Ӧ�ó���
        UnityEditor.EditorApplication.isPlaying = false; // ���ڱ༭������Ч
#else
        Application.Quit();
#endif

    }
}
