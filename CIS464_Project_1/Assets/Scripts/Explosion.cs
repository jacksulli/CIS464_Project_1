using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is meant to be attached to a particle effect, and the only purpose is to destroy it after a few seconds
public class Explosion : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(Lifetime());
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
