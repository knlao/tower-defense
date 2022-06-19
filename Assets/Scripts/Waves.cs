using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    public TextAsset easyJSON;
    public TextAsset mediumJSON;
    public TextAsset hardJSON;


    public WaveList easy;
    public WaveList medium;
    public WaveList hard;

    public WaveList myWaves;

    private LevelDifficuty currentDifficulty;

    private void Awake()
    {
        ReadCSV(easyJSON, LevelDifficuty.Easy);
        ReadCSV(mediumJSON, LevelDifficuty.Medium);
        ReadCSV(hardJSON, LevelDifficuty.Hard);
    }

    private void ReadCSV(TextAsset tAsset, LevelDifficuty d)
    {
        string[] data = tAsset.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        int tableSize = data.Length / 7 - 1;
        switch (d)
        {
            case LevelDifficuty.Easy:
                easy.waves = new Wave[tableSize];
                for (int i = 0; i < tableSize; i++)
                {
                    easy.waves[i] = new Wave();
                    easy.waves[i].wave_index = int.Parse(data[7 * (i + 1)]);
                    easy.waves[i].enemies = new int[6];
                    for (int j = 0; j < 6; j++)
                    {
                        easy.waves[i].enemies[j] = int.Parse(data[7 * (i + 1) + 1 + j]);
                    }
                }
                break;
            case LevelDifficuty.Medium:
                medium.waves = new Wave[tableSize];
                for (int i = 0; i < tableSize; i++)
                {
                    medium.waves[i] = new Wave();
                    medium.waves[i].wave_index = int.Parse(data[7 * (i + 1)]);
                    medium.waves[i].enemies = new int[6];
                    for (int j = 0; j < 6; j++)
                    {
                        medium.waves[i].enemies[j] = int.Parse(data[7 * (i + 1) + 1 + j]);
                    }
                }
                break;
            case LevelDifficuty.Hard:
                hard.waves = new Wave[tableSize];
                for (int i = 0; i < tableSize; i++)
                {
                    hard.waves[i] = new Wave();
                    hard.waves[i].wave_index = int.Parse(data[7 * (i + 1)]);
                    hard.waves[i].enemies = new int[6];
                    for (int j = 0; j < 6; j++)
                    {
                        hard.waves[i].enemies[j] = int.Parse(data[7 * (i + 1) + 1 + j]);
                    }
                }
                break;
            default:
                Debug.LogError("Invalid Difficulty");
                break;
        }
    }

    private void Start()
    {
        currentDifficulty = FindObjectOfType<Difficulty>().selectedDifficulty;
        switch (currentDifficulty)
        {
            case LevelDifficuty.Easy:
                myWaves = easy;
                break;
            case LevelDifficuty.Medium:
                myWaves = medium;
                break;
            case LevelDifficuty.Hard:
                myWaves = hard;
                break;
            default:
                Debug.LogError("Invalid Difficulty");
                break;
        }
    }
}

[System.Serializable]
public class Wave
{
    public int wave_index;  
    public int[] enemies;
}

[System.Serializable]
public class WaveList
{
    public Wave[] waves;
}