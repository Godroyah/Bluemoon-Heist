using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject playMenu;
    public GameObject reticule;
    public GameObject healthText;
    public GameObject healthSlider;

    private bool paused;

    private void Start()
    {
        paused = false;
    }

    void Update()
    {
        if(paused)
        {
            if (Input.GetButton("Quit"))
            {
                Application.Quit();
            }
            else if (Input.GetButton("MainMenu"))
            {
                SceneManager.LoadScene("Main_Menu");
            }
        }

        Debug.Log(Cursor.lockState);
        if (Input.GetButtonDown("Cancel"))
        {
            if (paused)
            {
                Resume();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            else
            {
                Pause();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        reticule.SetActive(true);
        healthText.SetActive(true);
        healthSlider.SetActive(true);
        Time.timeScale = 1f;
        paused = false;
    }

    public void Pause()
    {
        reticule.SetActive(false);
        healthText.SetActive(false);
        healthSlider.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void ResumeGame()
    {
        Resume();
    }

    public void HowToPlay()
    {
        pauseMenu.SetActive(false);
        playMenu.SetActive(true);
    }

    public void Back()
    {
        playMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

}
