using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI livesLeftText;

    [SerializeField] private TextMeshProUGUI enemyLeftText;

    [SerializeField]  private PlayerLivesSO livesManager;

    [SerializeField]  private EnemiesLeftSO enemiesManager;

    [SerializeField] private PlayerStats playerStats;

    public GameObject lossMenuObject;

    public FloatVariable currentLevel;

    public GameObject roundStart;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI enemiesText;

    //Escape Menu Info:
    private bool escapeMenuOpen = false;
    [SerializeField] private GameObject escapeMenu;

    private void Start()
    {
        livesLeftText.text = livesManager.value.ToString(); //Set the lives text to be based on the scriptable object livesManager
        enemyLeftText.text = enemiesManager.value.ToString(); //Set the enemy left text to be based on the scriptable object livesManager

        levelText.text = "Level: " + SceneManager.GetActiveScene().buildIndex.ToString(); //Create level text based on the build index. Ideally there would be a scriptable object storing this

        StartCoroutine(StartSequence());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(escapeMenuOpen)
            {
                TurnOffEscapeMenu();
            }
            else
            {
                TurnOnEscapeMenu();
            }
        }
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        //Set the UI to be a listener to the events in the livesManager and enemiesManager so the UI will change whenever the values of those objects change
        livesManager.livesChangeEvent.AddListener(ChangeLivesValue);
        enemiesManager.enemiesLeftEvent.AddListener(ChangeEnemyLeftValue);
    }

    // Update is called once per frame
    void OnDisable()
    {
        livesManager.livesChangeEvent.RemoveListener(ChangeLivesValue);
        enemiesManager.enemiesLeftEvent.RemoveListener(ChangeEnemyLeftValue);
    }

    public void TurnOnEscapeMenu()
    {
        escapeMenuOpen = true;
        escapeMenu.SetActive(true);
        Cursor.visible = true; //Turn on mouse cursor
        Cursor.lockState = CursorLockMode.None; //Unlock mouse Cursor
    }

    public void TurnOffEscapeMenu()
    {
        escapeMenuOpen = false;
        escapeMenu.SetActive(false);
        Cursor.visible = false; //Turn off mouse cursor
        Cursor.lockState = CursorLockMode.Locked; //Lock mouse Cursor
    }

    public void ChangeLivesValue(int amount)
    {
        livesLeftText.text = amount.ToString();
    }

    public void ChangeEnemyLeftValue(int amount)
    {
        enemyLeftText.text = amount.ToString();
        enemiesText.text = "Enemies: " + amount.ToString();
    }

    public IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(2f);
        roundStart.SetActive(false);
    }
}
