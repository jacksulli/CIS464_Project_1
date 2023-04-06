//This script just controls the lifetime of an explosion particle effect
//This script should be attached to an explosion particle effect prefab
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Lifetime());
    }

    //Destroy the object after 2 seconds
    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
