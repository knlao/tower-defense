using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text wavesText;

    private void OnEnable()
    {
        wavesText.text = "" + PlayerStats.Waves;
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
