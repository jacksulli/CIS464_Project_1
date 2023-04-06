//This script controls the game score UI elements
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

    //List of modifiers to adjust the score for calculating the final score
    [SerializeField] private float deathsScoreModifier = 1f;
    [SerializeField] private float killsScoreModifier = 1f;
    [SerializeField] private float timeScoreModifier = 1f;
    [SerializeField] private float levelScoreModifier = 1f;
    

    public void Start()
    {
        
        enemiesKilled = playerStats.enemiesKilled; //Get the total amount of enemies killed from the player stats SO
        playerDeaths = playerStats.playerDeaths; //Get the total amount of player deaths
        time = playerStats.time; //Get the time it took the player to finish the game
        curLevel = playerStats.currentLevel; //Get the current level

        //Set the text in the UI
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

    //Create a final score based on the indivudal elements
    private float CreateScore()
    {
        float score = (enemiesKilled * killsScoreModifier) + (curLevel * levelScoreModifier) + (1/time * timeScoreModifier) - (playerDeaths * deathsScoreModifier);
        return score;
    }
}
