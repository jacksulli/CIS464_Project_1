using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMine : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField] private GameObject explosionEffect;
=======
<<<<<<< Updated upstream
=======
    [SerializeField] private GameObject explosionEffect; //Reference to the explosion effect gameobject
>>>>>>> Stashed changes
>>>>>>> Jack

    //Whenever an object collides with this
    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<BoatController>()) //If the object has a BoatController script attached
        {
            BoatController theBoat = other.GetComponent<BoatController>(); //Get reference to that objects boatcontroller
            theBoat.Die(); //Kill the player boat
            Destroy(this.gameObject); //Destroy the mine
        }
    }
<<<<<<< HEAD

=======
<<<<<<< Updated upstream
=======

    //This function is actually never used, but is left here in case we want to switch control of the explosion to this script,
    //instead of the boat controller script
>>>>>>> Jack
    public void Die()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
<<<<<<< HEAD
=======
>>>>>>> Stashed changes
>>>>>>> Jack
}
