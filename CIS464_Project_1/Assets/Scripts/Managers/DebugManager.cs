using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
<<<<<<< HEAD
    public BoatController playerBoat;
    public PlayerLivesSO livesManager;
    public LevelManager levelManager;
    public GameObject debugManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Debug Mode");
            if (debugManager.activeSelf)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                debugManager.SetActive(false);
            }
            else
            {
                playerBoat.ExitSonarMode();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                debugManager.SetActive(true);
            }
        }
    }

    // Start is called before the first frame update
=======
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
>>>>>>> Jack
    public void IncreasePlayerSpeed()
    {
        playerBoat.IncreaseSpeed();
    }

<<<<<<< HEAD
=======
    //Decrease player speed by a fixed amount
>>>>>>> Jack
    public void DecreasePlayerSpeed()
    {
        playerBoat.DecreaseSpeed();
    }

<<<<<<< HEAD
=======
    //Toggle player invincibility
>>>>>>> Jack
    public void ToggleInvincibility()
    {
        playerBoat.ToggleInvincibility();

    }

<<<<<<< HEAD
=======
    //Increase player lives by 1
>>>>>>> Jack
    public void IncreaseLives()
    {
        livesManager.IncreaseLives(1);
    }

<<<<<<< HEAD
=======
    //Decrease player lives by 1
>>>>>>> Jack
    public void DecreaseLives()
    {
        livesManager.DecreaseLives(1);
    }

<<<<<<< HEAD
=======
    //Automatically win level
>>>>>>> Jack
    public void WinGame()
    {
        levelManager.NextLevel();
    }

<<<<<<< HEAD
=======
    //Restart level
>>>>>>> Jack
    public void Restart()
    {
        levelManager.Restart();
    }

<<<<<<< HEAD
=======
    //Go to previous level
>>>>>>> Jack
    public void PreviousLevel()
    {
        levelManager.PreviousLevel();
    }
}
