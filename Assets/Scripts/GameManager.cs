using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    
    public static bool GameIsOver;

    private void Start()
    {
        Time.timeScale = 1;
        GameIsOver = false;
        FindObjectOfType<ScreenFader>().FadeFrom();
    }
    
    private void Update()
    {
        if (!FindObjectOfType<PauseMenu>().isPaused)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                Time.timeScale = 5;
            else
                Time.timeScale = 1;
        }
        
        if (Input.GetKeyDown(KeyCode.R))
            FindObjectOfType<ScreenFader>().FadeTo("MainScene");

        if (Input.GetKeyDown(KeyCode.E))
            EndGame();
        
        if (GameIsOver) return;
        if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        GameIsOver = true;
        gameOverUI.SetActive(true);
    }
}
