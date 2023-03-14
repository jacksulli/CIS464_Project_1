using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script affects non-enemy and non-player objects that can be destroyed
public class ObstacleDestructable : MonoBehaviour
{
    //Whenever something collides with this object, if the collision is with a torpedo, then this object Dies
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Torpedo")
        {
            Die();
        }
    }
    private void Die()
    {
        AudioManager.Instance.PlaySound("ExplosionDebris");
        Destroy(this.gameObject);
    }
}
