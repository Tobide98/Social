using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public int highestScore;
    public int highestCombo;
    public float longestTime;
    public int totalTimesPlayed;


    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI totalTimesPlayedText;


    // Start is called before the first frame update
    void Start()
    {
        GetPlayerData();
        highscoreText.text = "HIGHSCORE: " + highestScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetPlayerData()
    {
        resetData();
        if (PlayerPrefs.HasKey("highestScore")) highestScore = PlayerPrefs.GetInt("highestScore");
        if (PlayerPrefs.HasKey("highestCombo")) highestCombo = PlayerPrefs.GetInt("highestCombo");
        if (PlayerPrefs.HasKey("longestTime")) longestTime = PlayerPrefs.GetFloat("longestTime");
        if (PlayerPrefs.HasKey("totalPlayed")) totalTimesPlayed = PlayerPrefs.GetInt("totalPlayed");
    }

    public void SavePlayerProfile()
    {
        PlayerPrefs.SetInt("highestScore", highestScore);
        highscoreText.text = "HIGHSCORE: " + highestScore.ToString();
    }

    public void resetData()
    {
        highestCombo = 0;
        highestScore = 0;
        longestTime = 0;
        totalTimesPlayed = 0;
    }

    public void ClearPlayerData()
    {
        resetData();
        SavePlayerProfile();
    }

    public void ClearGameplayData()
    {
        GetPlayerData();
    }
}
