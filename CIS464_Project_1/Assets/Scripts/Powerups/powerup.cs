using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerup : MonoBehaviour
{

    public float multiplier = 0.8f;
    public GameObject pickupEffect;
    

    void OnTriggerEnter (Collider other) {

        if(other.CompareTag("Player")) {
            Pickup(other);
        }
    }

    void Pickup(Collider player) {

        //spawn effect
        Instantiate(pickupEffect, transform.position, transform.rotation);

        //get player components

        BoatController boat = player.GetComponent<BoatController>(); 

        //pick a random effect to be applied
        int randomNum = Random.Range(0,4);

        //apply effect to player
        if(randomNum == 0){
            player.transform.localScale *= 0.9f;
            boat.movementSpeed *= 1.2f;
            boat.maxVelocity *= 1.2f;
            Debug.Log("Size Down!");
        }else if(randomNum == 1){
            boat.movementSpeed *= 1.4f;
            boat.maxVelocity *= 1.4f;
            Debug.Log("Speed Up!");
        }else if(randomNum == 2){
            boat.rotationSpeed *= 1.2f;
            boat.maxVelocity *= 1.2f;
            Debug.Log("Turning Up!");
        }else if(randomNum == 3){
            int ranfomNum2 = Random.Range(0,10);
            if(ranfomNum2 == 9){
                boat.ToggleInvincibility();
            }else{
            boat.movementSpeed *= 1.4f;
            boat.maxVelocity *= 1.4f;
            Debug.Log("Speed Up!");
            }
        }else if(randomNum == 4){
            boat.coolDownTime = 0.5f;
            Debug.Log("Faster Depth Charges!");
        }
        
        
        //delete object
        Destroy(gameObject);
    }
}
