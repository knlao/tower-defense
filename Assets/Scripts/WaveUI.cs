using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    public Text wavesUI;

    private void Update()
    {
        wavesUI.text = "" + PlayerStats.Waves;
    }
}
