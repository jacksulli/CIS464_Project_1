//This script controls the torpedo object that is shot from the enemy submarines
//It includes the logic to destroy the torpedo when it hits something, and destroying the object it hits
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect; //Reference to the explosion effect gameobject for when the torpedo is destroyed
    void Start()
    {
        //Start the torpedoes lifecycle which just destroys it after 5 seconds
        //Just in case the torpedo doesn't hit anything and travels off the map
        StartCoroutine(LifeCycle()); 
    }

    // Waits five seconds then destroys the torpedo
    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the torpedo hits something besides the ground or the enemy submarines
        if(other.gameObject.tag != "Ocean" && other.gameObject.tag != "Enemy")
        {
            //If the torpedo hits a player
            if (other.gameObject.tag == "Player")
            {
                BoatController thePlayer = other.gameObject.GetComponent<BoatController>(); //Get a reference to the boat's script
                thePlayer.Die(); //Kill the player
                Destroy(this.gameObject); 
            }
            else if (other.gameObject.tag == "Mine") //If the torpedo hits a mine
            {
                SeaMine seaMine = other.gameObject.GetComponent<SeaMine>(); //Get a reference to the mine's script
                AudioManager.Instance.PlaySound("TorpedoExplosion"); //Play the torpedo death sound
                seaMine.Die(); //Destroy the mine. In the future make this make the mine blow up
                Destroy(this.gameObject);

            }
            else if(other.gameObject.GetComponent<DepthCharge>()) //If the torpedo hits a falling depth charge
            {
                DepthCharge theCharge = other.gameObject.GetComponent<DepthCharge>(); //Get a reference to the depth charge that was hit
                theCharge.Die(); //Destroy the depth charge
                AudioManager.Instance.PlaySound("TorpedoExplosion");
                Instantiate(explosionEffect, transform.position, transform.rotation); //Instantiate the torpedo death explosion at the impact point
                Destroy(this.gameObject);
            }
            else //If the torpedo hits anything else
            {
                AudioManager.Instance.PlaySound("TorpedoExplosion");
                Instantiate(explosionEffect, transform.position, transform.rotation); //Instantiate the torpedo death explosion at the impact point
                Destroy(this.gameObject);
            }
        }
        
        
        

    }
}
