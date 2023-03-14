using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public BoatController playerBoat; //Reference to the player
    public PlayerLivesSO livesManager; //Reference to the lives manager scriptable object
    public LevelManager levelManager; //Reference to the LevelManager GameObject
    public GameObject debugManager; //Reference to the debugManager UI gameObject

    private void Update()
    {
        ToggleDebugManager(); //If the player presses tab, turn on/off the debug manager
    }

    void ToggleDebugManager()
    {
        //When the player presses tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Debug Mode");
            //If the debugManager is already active
            if (debugManager.activeSelf)
            {
                Cursor.visible = false; //Turn off cursor
                Cursor.lockState = CursorLockMode.Locked; //Lock cursor position
                debugManager.SetActive(false); //Turn off debug manager
            }
            else
            {
                Cursor.visible = true; //Turn on the cursor
                Cursor.lockState = CursorLockMode.None; //Unlock cursor position
                debugManager.SetActive(true); //Turn on the debug manager
            }
        }
    }
 
    //Increase player speed by fixed amount
    public void IncreasePlayerSpeed()
    {
        playerBoat.IncreaseSpeed();
    }

    //Decrease player speed by a fixed amount
    public void DecreasePlayerSpeed()
    {
        playerBoat.DecreaseSpeed();
    }

    //Toggle player invincibility
    public void ToggleInvincibility()
    {
        playerBoat.ToggleInvincibility();

    }

    //Increase player lives by 1
    public void IncreaseLives()
    {
        livesManager.IncreaseLives(1);
    }

    //Decrease player lives by 1
    public void DecreaseLives()
    {
        livesManager.DecreaseLives(1);
    }

    //Automatically win level
    public void WinGame()
    {
        levelManager.NextLevel();
    }

    //Restart level
    public void Restart()
    {
        levelManager.Restart();
    }

    //Go to previous level
    public void PreviousLevel()
    {
        levelManager.PreviousLevel();
    }
}
