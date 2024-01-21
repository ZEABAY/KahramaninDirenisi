using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI Score;

    public void Setup(int score)
    {
        SetCursorState(true);
        gameObject.SetActive(true);
        Score.text = $"{score} Enemies Killed";
        Time.timeScale = 0;
    }


    public void SetCursorState(bool newState)
    {
        Cursor.visible = newState;
        Cursor.lockState = newState ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("PlayGround");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitButton()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
