//This script controls objects that can be destroyed by the torpedoes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestructable : MonoBehaviour
{
    [SerializeField] GameObject powerUp;
    private void OnTriggerEnter(Collider other)
    {
        //If a torpedo collides with this object
        if(other.gameObject.tag == "Torpedo")
        {
            Die();
        }
    }

    private void Die()
    {
        AudioManager.Instance.PlaySound("ExplosionDebris"); //Play explosion sound

        float randomNum = Random.Range(0f, 1f);

        // Check if the random number is less than 0.3 (30% chance)
        if (randomNum < 0.8f)
        {
            Instantiate(powerUp, transform.position, transform.rotation);
        }

        Destroy(this.gameObject); //Destroy this gameobject
    }
}
