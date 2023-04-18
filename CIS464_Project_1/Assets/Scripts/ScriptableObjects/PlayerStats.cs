/*This script contains assorted info about the player that needs to be separate from the game.
 * The main function of this scriptable object is to store information related to the player's score, and time the player.
 * It also has information about if the player is in debug mode or not, and how many lives the player should start with.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Score Info")]
    public int enemiesKilled = 10;
    public int playerDeaths = 10;
    public int currentLevel = 0;
    public float startTime;
    public float endTime;
    public float time;
    public int finalLevelID;

    [Header("Player Info")]
    public bool inDebugMode = false;

    [System.NonSerialized]
    public UnityEvent levelChangeEvent;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (levelChangeEvent == null)
        {
            levelChangeEvent = new UnityEvent();
        }
    }
    public void IncreaseLevel()
    {
        currentLevel += 1;
    }

    public void DecreaseLevel()
    {
        currentLevel -= 1;
    }

    public void SetLevel(int _level)
    {
        currentLevel = _level;
    }

    public void StartTimeCounter()
    {
        startTime = Time.time;
    }

    public void EndTimeCounter()
    {
        endTime = Time.time;
        time = endTime - startTime;
    }

    public void ResetStats()
    {
        enemiesKilled = 0;
        playerDeaths = 0;
        currentLevel = 1;
        startTime = 0;
        endTime = 0;
    }

    public void ToggleDebug()
    {
        if(inDebugMode)
        {
            inDebugMode = false;
        }
        else
        {
            inDebugMode = true;
        }
    }
}
