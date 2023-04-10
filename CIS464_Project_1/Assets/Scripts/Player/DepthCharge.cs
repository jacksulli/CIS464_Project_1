//This script controls the depth charge that's dropped by the destroyer
//It makes a splash sound when it hits a water, and a few seconds later blows up
//When it blows up, it destroys any nearby enemy submarines
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCharge : MonoBehaviour
{
    [SerializeField] private GameObject explosion; //Reference to the depth charge explosion object (not effect)

    [SerializeField] private FloatVariable depthChargeUses; //Get a reference to the float variable that corresponds to depth charge uses
    
    void Start()
    {
        StartCoroutine(LifeCycle());
    }

    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(0.4f); //In theory this seconds would depend on the upward force value
        AudioManager.Instance.PlaySound("Splash"); //Play the splash sound around the time the depth charge hits the water
        yield return new WaitForSeconds(1.5f);
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z), transform.rotation); //Instantiate the depth charge explosion on the x,z position
        Die();
        
    }

    public void Die()
    {
        depthChargeUses.value += 1;
        Destroy(this.gameObject);
    }
}

    
