using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public PlayerLivesSO livesManager;
    public PlayerStats playerStats;
    public GameObject optionsMenu;
    public GameObject buttonsHolder;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    public void StartGame()
    {
        AudioManager.Instance.PlaySound("Click");
        livesManager.value = 3;
        playerStats.ResetStats();
        playerStats.StartTimeCounter();

        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        AudioManager.Instance.PlaySound("Click");
        Application.Quit();
    }

    public void OpenOptionsMenu()
    {
        AudioManager.Instance.PlaySound("Click");
        optionsMenu.SetActive(true);
        buttonsHolder.SetActive(false);
    }

    public void CloseOptionsMenu()
    {
        AudioManager.Instance.PlaySound("Click");
        optionsMenu.SetActive(false);
        buttonsHolder.SetActive(true);
    }
}
