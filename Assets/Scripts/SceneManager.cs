using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void GoToScene(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void OnEasyClicked()
    {
        GameManager.Instance.Difficulty = 25;
        UnityEngine.SceneManagement.SceneManager.LoadScene("game");
    }

    public void OnNormalClicked()
    {
        GameManager.Instance.Difficulty = 15;
        UnityEngine.SceneManagement.SceneManager.LoadScene("game");
    }

    public void OnHardClicked()
    {
        GameManager.Instance.Difficulty = 5;
        UnityEngine.SceneManagement.SceneManager.LoadScene("game");
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}