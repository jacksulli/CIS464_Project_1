//This script controls the level changing (i.e. winning the level, losing the level)
//This script also hardcodes in the level ID of the final level
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public PlayerLivesSO livesManager; //Reference to the Player Lives scriptable object
    public EnemiesLeftSO enemiesManager; //Reference to the Enemies Left scriptable object
    public PlayerStats playerStats; 

    //These three should be moved to the UI manager
    public GameObject lossMenu; //Reference to the loss menu UI
    public GameObject winText; //Reference to the You Win! text that displays after a match
    public GameObject winImage; //Reference to the Winning image
    [SerializeField] private GameObject scoreSheet;

    //Reference to the Build ID of the last level of the game. Right now it's level 7
    //This causes the game to end after the completion of this level
    [SerializeField] private int finalLevelID = 7;    
    
    public int livesToAdd = 1; //The amount of lives to add to the player after winning each level

    private void Awake()
    {
        //If somehow the level starts with the player having no lives or less 
        if(livesManager.value <= 0)
        {
            livesManager.value = 0; //Reset the lives counter
            livesManager.IncreaseLives(1); //Increase the lives by 1
        }

        enemiesManager.value = 0; //Reset the enemies manager value, since the enemies manager controls the enemies per-level
    }

    //Theoretically this should be in the UI Manager
    public void Loss()
    {
        lossMenu.SetActive(true); //Turn on the loss menu
        playerStats.EndTimeCounter(); //End the timer counting the player's time in-game
        AudioManager.Instance.StopMusic(); //Turn off all music
        Cursor.visible = true; //Turn off mouse cursor
        Cursor.lockState = CursorLockMode.None; //Lock the cursor movement
    }

    //Go to the next level, can be triggered by enemies dropping to 0 or using debug menu
    public void NextLevel()
    {
        livesManager.IncreaseLives(livesToAdd); //Increase the player lives
        playerStats.currentLevel += 1; //Increase the current level number reference
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Move to the next level
    }
    
    //Takes the player to the main menu
    public void MainMenu()
    {
        playerStats.currentLevel = 1; //Sets the current level to 1, since this value only matters in game. The only way the player can enter the game is in level 1
        AudioManager.Instance.StopMusic(); //Stop the music
        SceneManager.LoadScene(0); //Load the main menu scene
    }

    //Restart the game from level 1
    public void Restart()
    {
        playerStats.currentLevel = 1; //Set the current level reference variable to 1
        livesManager.value = 3; //Set the lives back to the default lives value
        SceneManager.LoadScene(1); //Load level 1
    }

    //Go to the previous level, only can be triggered from cheats menu
    public void PreviousLevel()
    {
        playerStats.currentLevel -= 1; //Subtract 1 from the reference to
        livesManager.value = 3; //Reset the lives I guess
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); //Go to previous scene
    }

    //Resets the current level
    public void ResetCurrentLevel()
    {
        StartCoroutine(ResetCycle()); //Calls a coroutine, this can probably be removed
    }
    void OnEnable()
    {
        livesManager.livesChangeEvent.AddListener(CheckForLoss); //When the livesChangeEvent from the lives manager, triggers, CheckForLoss runs
        enemiesManager.enemiesLeftEvent.AddListener(CheckForWin); //When the livesChangeEvent from the enemies manager SO triggers, CheckForWin runs
    }

    //When this script is disabled
    void OnDisable()
    {
        livesManager.livesChangeEvent.RemoveListener(CheckForLoss); //Remove this script as a listener for the livesChangeEvent
        enemiesManager.enemiesLeftEvent.RemoveListener(CheckForWin); //Remove this script as a listener for the enemiesLeftEvent
    }

    public void CheckForLoss(int _amount)
    {
        if (livesManager.value <= 0)
        {
            Loss();
        }
    }

    //Checks to see if the player won the current level
    public void CheckForWin(int _amount)
    {
        if(enemiesManager.value <= 0) //Check if the number of enemies left is 0
        {
            StartCoroutine(WinLevel()); //Start the winlevel coroutine
        }
    }

    //Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }

    //Win level coroutine
    private IEnumerator WinLevel()
    {
        playerStats.enemiesKilled += enemiesManager.enemiesPerLevel; //Add the amount of enemies killed to the scoreboard
        enemiesManager.enemiesPerLevel = 0; //Reset the enemies per level value (this is under construction)
        playerStats.EndTimeCounter(); //End the timer counting how long the player has been on the game
        if(SceneManager.GetActiveScene().buildIndex == finalLevelID) //If this level is the final level, win the entire game
        {
            //This should also theoretically be on the UI manager
            winImage.SetActive(true); //Turn on the game win image
            scoreSheet.SetActive(true); //Turn on the score panel
            AudioManager.Instance.StopMusic(); //Turn off music
            AudioManager.Instance.PlayMusic("VictoryTrack"); //play the victory music
            yield return new WaitForSeconds(20f); //Wait 20 seconds
            AudioManager.Instance.StopMusic(); 
            MainMenu(); //Kick the player to the main menu
        }
        else //Else just win the current level
        {
            winText.SetActive(true); //Turn on the generic win text
            yield return new WaitForSeconds(4f);
            NextLevel(); //Go to the next level
        }
        
    }

    //Waits two seconds then resets the level, this is probably not necessary
    public IEnumerator ResetCycle()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
