using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EnemiesManagerSO", menuName = "ScriptableObjects/EnemiesManager")]
public class EnemiesLeftSO : ScriptableObject
{
    public int value = 1;

    [System.NonSerialized]
    public UnityEvent<int> enemiesLeftEvent; //Events used to decouple code from UI.
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (enemiesLeftEvent == null)
        {
            enemiesLeftEvent = new UnityEvent<int>();
        }
    }

    public void DecreaseLives(int _amount)
    {
        value -= _amount;
        enemiesLeftEvent.Invoke(value); //Event tells UI to update the number on the screen
    }

    public void IncreaseLives(int _amount)
    {
        value += _amount;
        enemiesLeftEvent.Invoke(value);
    }
}
