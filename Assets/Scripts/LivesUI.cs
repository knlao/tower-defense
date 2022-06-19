using System;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public Text livesUI;

    private void Update()
    {
        livesUI.text = "" + PlayerStats.Lives;
    }
}