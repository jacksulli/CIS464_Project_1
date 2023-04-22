using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScreen : MonoBehaviour
{
    //WakeParticle System Settings
    ParticleSystem.EmissionModule smokeParticle; //Reference to the wake objects
    [SerializeField] private float wakeMultiplier; //Multiplier for number of particles created
    [SerializeField] private float wakeBase; //Base number of particles
    private void Start()
    {
        smokeParticle = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
        StartCoroutine(LifeCycle());
    }

    
    IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(10f);
        smokeParticle.rateOverTime = 0;
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
