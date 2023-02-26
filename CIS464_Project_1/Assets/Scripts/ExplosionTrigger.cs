using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BoatController>())
        {
            BoatController theBoat = other.GetComponent<BoatController>();
            theBoat.Die();
            Destroy(this.gameObject);
        }
    }
}
