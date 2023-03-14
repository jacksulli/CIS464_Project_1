using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "LivesManagerScriptableObject", menuName = "ScriptableObjects/LivesManager")]
public class PlayerLivesSO : ScriptableObject
{

    public int value = 10;

    [System.NonSerialized]
    public UnityEvent<int> livesChangeEvent;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if(livesChangeEvent == null)
        {
            livesChangeEvent = new UnityEvent<int>();
        }
    }

    public void DecreaseLives(int _amount)
    {
        value -= _amount;
        livesChangeEvent.Invoke(value);
    }

    public void IncreaseLives(int _amount)
    {
        value += _amount;
        livesChangeEvent.Invoke(value);
    }
}
