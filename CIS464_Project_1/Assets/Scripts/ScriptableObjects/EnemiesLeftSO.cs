//This scriptable object determines the enemies left per level.
//This number is reset each time a new level is started
//Each enemy submarine induvidually increases the number when the spawn on level start
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EnemiesManagerSO", menuName = "ScriptableObjects/EnemiesManager")]
public class EnemiesLeftSO : ScriptableObject
{
    public int value = 1; //Sets a base value
    public int enemiesPerLevel; //This value is for the score sheet. Once the player finishes a level, this value is added to the total enemies killed

    [System.NonSerialized]
    public UnityEvent<int> enemiesLeftEvent; //Events used to decouple code from UI.
    
    private void OnEnable()
    {
        //Set the enemiesLeftEvent
        if (enemiesLeftEvent == null)
        {
            enemiesLeftEvent = new UnityEvent<int>();
        }
    }

    //Decreases the amount of enemies left. The name of this function is wrong
    public void DecreaseLives(int _amount)
    {
        value -= _amount;
        enemiesLeftEvent.Invoke(value); //Event tells UI to update the number on the screen
    }

    //Increases the amount of enemies left. The name of this function is wrong
    public void IncreaseLives(int _amount)
    {
        value += _amount;
        enemiesLeftEvent.Invoke(value);
    }
}
