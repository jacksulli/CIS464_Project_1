using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //[SerializeField]
    //private Slider slider;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private PlayerLivesSO livesManager;

    private void Start()
    {
        text.text = livesManager.value.ToString();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        livesManager.livesChangeEvent.AddListener(ChangeLivesValue);
    }

    // Update is called once per frame
    void OnDisable()
    {
        livesManager.livesChangeEvent.RemoveListener(ChangeLivesValue);
    }

    public void ChangeLivesValue(int amount)
    {
        text.text = amount.ToString();
    }
}
