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

        deathsScoreText.text = playerDeaths.ToString();
        killsScoreText.text = enemiesKilled.ToString();
        timeScoreText.text = time.ToString();
        levelScoreText.text = curLevel.ToString();


        totalScoreText.text = CreateScore().ToString();
    }

    private float CreateScore()
    {
        float score = (enemiesKilled * killsScoreModifier) + (curLevel * levelScoreModifier) + (1/time * timeScoreModifier) - (playerDeaths * deathsScoreModifier);
        return score;
    }
}
