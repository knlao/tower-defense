using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelDifficuty
{
    Easy, //0
    Medium, //1
    Hard //2
}

public class Difficulty : MonoBehaviour
{
    public LevelDifficuty selectedDifficulty;

    public void ChooseDifficulty(int difficulty)
    {
        switch(difficulty)
        {
            case 0:
                selectedDifficulty = LevelDifficuty.Easy;
                break;
            case 1:
                selectedDifficulty = LevelDifficuty.Medium;
                break;
            case 2:
                selectedDifficulty = LevelDifficuty.Hard;
                break;
        }
    }
}
