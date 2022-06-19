using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator cameraAnim;

    private void Start()
    {
        FindObjectOfType<ScreenFader>().FadeFrom();
    }

    public void Play()
    {
        cameraAnim.SetBool("isSelect", true);
    }

    public void More()
    {
        cameraAnim.SetBool("isMore", true);
    }

    public void Back()
    {
        cameraAnim.SetBool("isMore", false);
        cameraAnim.SetBool("isSelect", false);
    }

    public void Controls()
    {
        cameraAnim.SetBool("isControls", true);
    }

    public void Credits()
    {
        cameraAnim.SetBool("isCredits", true);
    }

    public void Back2()
    {
        cameraAnim.SetBool("isControls", false);
        cameraAnim.SetBool("isCredits", false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Easy()
    {
        FindObjectOfType<Difficulty>().ChooseDifficulty(0);
        FindObjectOfType<ScreenFader>().FadeTo("MainScene");
    }

    public void Medium()
    {
        FindObjectOfType<Difficulty>().ChooseDifficulty(1);
        FindObjectOfType<ScreenFader>().FadeTo("MainScene");
    }

    public void Hard()
    {
        FindObjectOfType<Difficulty>().ChooseDifficulty(2);
        FindObjectOfType<ScreenFader>().FadeTo("MainScene");
    }
}