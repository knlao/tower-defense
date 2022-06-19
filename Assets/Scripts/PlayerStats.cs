using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    /*
     * Player Statistics Script
     * Store player information
     */

    [Header("Money")]
    public int startMoney = 400;
    public static int Money;

    [Header("Lives")]
    public int startLives = 20;
    public static int Lives;

    [Header("Waves")]
    public static int Waves;

    private void Start()
    {
        Money = startMoney;
        Lives = startLives;
        Waves = 0;
    }
}
