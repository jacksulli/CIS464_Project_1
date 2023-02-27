using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private void Start()
    {
        livesLeftText.text = livesManager.value.ToString();
        enemyLeftText.text = enemiesManager.value.ToString();
    }

    private void Update()
    {
        
    }
    // Start is called before the first frame update
    void OnEnable()
    {
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
    }

}
