using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
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
    public void IncreasePlayerSpeed()
    {
        playerBoat.IncreaseSpeed();
    }

    public void DecreasePlayerSpeed()
    {
        playerBoat.DecreaseSpeed();
    }

    public void ToggleInvincibility()
    {
        playerBoat.ToggleInvincibility();

    }

    public void IncreaseLives()
    {
        livesManager.IncreaseLives(1);
    }

    public void DecreaseLives()
    {
        livesManager.DecreaseLives(1);
    }

    public void WinGame()
    {
        levelManager.NextLevel();
    }

    public void Restart()
    {
        levelManager.Restart();
    }

    public void PreviousLevel()
    {
        levelManager.PreviousLevel();
    }
}
