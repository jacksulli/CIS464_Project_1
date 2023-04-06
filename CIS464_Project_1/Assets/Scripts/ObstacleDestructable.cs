//This script controls objects that can be destroyed by the torpedoes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestructable : MonoBehaviour
{
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
        Destroy(this.gameObject); //Destroy this gameobject
    }
}
