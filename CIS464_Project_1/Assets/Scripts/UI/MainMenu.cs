using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public PlayerLivesSO livesManager; //Gets a reference to the PlayerLives scriptable object in the game files
    public PlayerStats playerStats; //Gets a reference to the PlayerStats scriptable object in the game files
    public GameObject optionsMenu; //Gets a reference to the options menu UI gameobject
    public GameObject buttonsHolder; //Gets a reference to the UI gameobject that holds the buttons
    
    void Start()
    {
        Cursor.visible = true; //Make the cursor visible, in case the player is coming from in-game
        Cursor.lockState = CursorLockMode.None; //Unlock the cursor
    }

    //Starts the game from Level 1. This is triggered by the player clicking the Start button, which has a reference to this script
    public void StartGame()
    {
        AudioManager.Instance.PlaySound("Click"); //Play the click sound
        livesManager.value = 3; //Set the base level of lives that the player gets, this should be controlled by a variable
        playerStats.ResetStats(); //Reset the players stats for the score
        playerStats.StartTimeCounter(); //Start the timer to count how long it takes the player to complete the game

        SceneManager.LoadScene(1); //Load level 1
    }

    //Quits the game
    public void Quit()
    {
        AudioManager.Instance.PlaySound("Click");
        Application.Quit(); //Quit the game
    }

    //Open the options menu in the main menu
    public void OpenOptionsMenu()
    {
        AudioManager.Instance.PlaySound("Click");
        optionsMenu.SetActive(true); //Turn on the options menu
        buttonsHolder.SetActive(false); //Turn off the inital main menu buttons
    }

    //Close the options menu
    public void CloseOptionsMenu()
    {
        AudioManager.Instance.PlaySound("Click");
        optionsMenu.SetActive(false); //Turn off the options menu
        buttonsHolder.SetActive(true); //Turn back on the inital main menu buttons
    }
}
