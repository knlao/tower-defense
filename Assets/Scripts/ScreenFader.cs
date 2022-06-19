using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public GameObject panel;
    public Color color;
    public AnimationCurve curve;

    private Image img;

    private void Awake()
    {
        img = panel.GetComponent<Image>();
    }

    public void FadeFrom()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    private IEnumerator FadeIn()
    {
        panel.SetActive(true);
        
        float t = 1;

        while (t > 0)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(color.r, color.g, color.b, a);
            yield return 0;
        }

        panel.SetActive(false);
    }

    private IEnumerator FadeOut(string scene)
    {
        panel.SetActive(true);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(color.r, color.g, color.b, a);
            yield return 0;
        }

        SceneManager.LoadScene(scene);
    }
}
