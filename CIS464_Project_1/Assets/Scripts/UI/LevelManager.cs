using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public PlayerLivesSO livesManager;

    public GameObject lossMenu;

    public int livesToAdd;

    private void Awake()
    {
        if(livesManager.value <= 0)
        {
            livesManager.value = 0;
            livesManager.IncreaseLives(1);
        }
    }
    public void Loss()
    {
        lossMenu.SetActive(true);
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


}
