using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public EnemiesLeftSO enemyManager;
    public LevelManager levelManager;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            enemyManager.DecreaseLives(1);

            if(enemyManager.value <= 0)
            {
                Debug.Log("You Win");
                levelManager.MainMenu();
            }
        }
    }
}
