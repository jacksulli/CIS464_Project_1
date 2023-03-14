using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMine : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<BoatController>())
        {
            BoatController theBoat = other.GetComponent<BoatController>();
            theBoat.Die();
            Destroy(this.gameObject);
        }
    }

    public void Die()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
