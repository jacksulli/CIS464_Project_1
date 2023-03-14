using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestructable : MonoBehaviour
{
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
