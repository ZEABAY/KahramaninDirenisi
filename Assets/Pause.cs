using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private bool paused = false;
    public GameObject pauseScreen;
    public FirstPersonController fpsc;

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("Pause");
            fpsc.enabled = paused;
            paused = !paused;
            SetCursorState(paused);
            PauseGame();
        }
    }
    public void SetCursorState(bool newState)
    {
        Cursor.visible = newState;
        Cursor.lockState = newState ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(paused);
        Time.timeScale = paused ? 0 : 1; // Duraklatma veya devam ettirme durumuna göre ayarla
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
