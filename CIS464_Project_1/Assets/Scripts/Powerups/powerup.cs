using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerup : MonoBehaviour
{

    public float multiplier = 1.2f;
    public GameObject pickupEffect;

    void OnTriggerEnter (Collider other) {

        if(other.CompareTag("Player")) {
            Pickup(other);
        }
    }

    void Pickup(Collider player) {

        //spawn effect
        Instantiate(pickupEffect, transform.position, transform.rotation);

        //apply effect to player
        player.transform.localScale *= multiplier;

        
        //delete object
        Destroy(gameObject);
    }
}
