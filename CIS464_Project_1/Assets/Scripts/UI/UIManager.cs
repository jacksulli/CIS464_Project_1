using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI livesLeftText;

    [SerializeField]
    private TextMeshProUGUI enemyLeftText;

    [SerializeField]
    private PlayerLivesSO livesManager;

    [SerializeField]
    private EnemiesLeftSO enemiesManager;

    public GameObject lossMenuObject;

    public FloatVariable currentLevel;

    public GameObject roundStart;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI enemiesText;

    private void Start()
    {
        livesLeftText.text = livesManager.value.ToString(); //Set the lives text to be based on the scriptable object livesManager
        enemyLeftText.text = enemiesManager.value.ToString(); //Set the enemy left text to be based on the scriptable object livesManager

        levelText.text = "Level: " + SceneManager.GetActiveScene().buildIndex.ToString(); //Create level text based on the build index. Ideally there would be a scriptable object storing this

        StartCoroutine(StartSequence());
    }

    private void Update()
    {
        
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
