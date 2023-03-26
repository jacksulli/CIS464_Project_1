using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int enemiesKilled = 10;
    public int playerDeaths = 10;
    public int currentLevel = 0;

    public float startTime;
    public float endTime;

    public float time;

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
}
