using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int enemiesKilled = 10;
    public int playerDeaths = 10;
    public int time = 0;
    public int currentLevel = 0;

    float startTime;
    float endTime;

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

    public float EndTimeCounter()
    {
        endTime = Time.time;
        return endTime - startTime;
    }
}
