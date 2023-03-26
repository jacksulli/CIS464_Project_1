using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public PlayerLivesSO livesManager;
    public EnemiesLeftSO enemiesManager;
    public PlayerStats playerStats;

    public GameObject lossMenu;
    public GameObject winText;
    public GameObject winImage;
    [SerializeField] private int finalLevelID = 7;    
    

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
        playerStats.currentLevel += 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        playerStats.currentLevel = 1;
        AudioManager.Instance.StopMusic();
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        playerStats.currentLevel = 1;
        livesManager.value = 5;
        SceneManager.LoadScene(1);
    }

    public void PreviousLevel()
    {
        playerStats.currentLevel -= 1;
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

    public void QuitGame()
    {
        Application.Quit();
    }


    private IEnumerator WinLevel()
    {
        

        if(SceneManager.GetActiveScene().buildIndex == finalLevelID)
        {
            winImage.SetActive(true);
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMusic("VictoryTrack");
            yield return new WaitForSeconds(20f);
            AudioManager.Instance.StopMusic();
            MainMenu();
        }
        else
        {
            winText.SetActive(true);
            yield return new WaitForSeconds(4f);
            NextLevel();
        }
        
    }
    public IEnumerator ResetCycle()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
