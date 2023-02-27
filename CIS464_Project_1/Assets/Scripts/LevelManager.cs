using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public PlayerLivesSO livesManager;
    public EnemiesLeftSO enemiesManager;

    public GameObject lossMenu;

    public int livesToAdd;

    private void Awake()
    {
        if(livesManager.value <= 0)
        {
            livesManager.value = 0;
            livesManager.IncreaseLives(1);
        }

        enemiesManager.value = 0;
    }
    public void Loss()
    {
        lossMenu.SetActive(true);
        Cursor.visible = true; //Turn off mouse cursor
        Cursor.lockState = CursorLockMode.None;
    }

    public void NextLevel()
    {
        livesManager.IncreaseLives(livesToAdd);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        livesManager.value = 5;
        SceneManager.LoadScene(1);
    }

    public void PreviousLevel()
    {
        livesManager.value = 5;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void OnEnable()
    {
        livesManager.livesChangeEvent.AddListener(CheckForLoss);
        enemiesManager.enemiesLeftEvent.AddListener(CheckForWin);
    }

    // Update is called once per frame
    void OnDisable()
    {
        livesManager.livesChangeEvent.RemoveListener(CheckForLoss);
        enemiesManager.enemiesLeftEvent.RemoveListener(CheckForWin);
    }

    public void CheckForLoss(int _amount)
    {
        if (livesManager.value <= 0)
        {
            Debug.Log("You Lost!");
            Loss();
        }
    }

    public void CheckForWin(int _amount)
    {
        if(enemiesManager.value <= 0)
        {
            Debug.Log("You Win!");
            NextLevel();
        }
    }


}
