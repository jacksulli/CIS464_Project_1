using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSheet : MonoBehaviour
{
    //Reference to the player stats scriptable object
    [SerializeField] private PlayerStats playerStats;

    //Reference to all the values that go into calculating the score
    int playerDeaths;
    int enemiesKilled;
    float time;
    int curLevel;

    //Reference to the text in the scoreboard menu UI
    [SerializeField] private TextMeshProUGUI deathsScoreText;
    [SerializeField] private TextMeshProUGUI killsScoreText;
    [SerializeField] private TextMeshProUGUI timeScoreText;
    [SerializeField] private TextMeshProUGUI levelScoreText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    //List of modifiers to adjust the score
    [SerializeField] private float deathsScoreModifier = 1f;
    [SerializeField] private float killsScoreModifier = 1f;
    [SerializeField] private float timeScoreModifier = 1f;
    [SerializeField] private float levelScoreModifier = 1f;
    

    public void Start()
    {
        
        enemiesKilled = playerStats.enemiesKilled;
        playerDeaths = playerStats.playerDeaths;
        time = playerStats.time;
        curLevel = playerStats.currentLevel;

        deathsScoreText.text = "Deaths: " + playerDeaths.ToString();
        killsScoreText.text = "Kills: " + enemiesKilled.ToString();
        timeScoreText.text = "Time: " + time.ToString();
        levelScoreText.text = "Level: " + curLevel.ToString();

        if (!playerStats.inDebugMode)
        {
            totalScoreText.text = "Total Score: " + CreateScore().ToString();
        }
        else
        {
            totalScoreText.text = "No Score (Cheats Enabled)";
        }

    }

    private float CreateScore()
    {
        float score = (enemiesKilled * killsScoreModifier) + (curLevel * levelScoreModifier) + (1/time * timeScoreModifier) - (playerDeaths * deathsScoreModifier);
        return score;
    }
}
