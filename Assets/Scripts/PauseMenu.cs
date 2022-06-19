using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject ui;
    public bool isPaused;

    private void Start()
    {
        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);
        isPaused = ui.activeSelf;

        Time.timeScale = ui.activeSelf ? 0 : 1;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        FindObjectOfType<ScreenFader>().FadeTo("MainScene");
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        FindObjectOfType<ScreenFader>().FadeTo("Menu");
    }
}