using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public PlayerLivesSO livesManager;
    public EnemiesLeftSO enemiesManager;
    public FloatVariable currentLevel;

    public GameObject lossMenu;
    public GameObject winText;
    

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
        currentLevel.value += 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        currentLevel.value = 1;
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

    public void ResetCurrentLevel()
    {
        StartCoroutine(ResetCycle());
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
            Loss();
        }
    }

    public void CheckForWin(int _amount)
    {
        if(enemiesManager.value <= 0)
        {
            StartCoroutine(WinLevel());
        }
    }

    
    private IEnumerator WinLevel()
    {
        winText.SetActive(true);
        yield return new WaitForSeconds(4f);

        if(currentLevel.value == 5)
        {
            MainMenu();
        }
        else
        {
            NextLevel();
        }
        
    }
    public IEnumerator ResetCycle()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
