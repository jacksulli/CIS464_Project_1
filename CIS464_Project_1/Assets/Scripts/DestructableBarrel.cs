using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableBarrel : MonoBehaviour
{
    [SerializeField] GameObject powerUp;
    private void OnTriggerEnter(Collider other)
    {
        //If a torpedo collides with this object
        if (other.gameObject.tag == "Torpedo")
        {
            Die();
        }
    }

    private void Die()
    {
        AudioManager.Instance.PlaySound("ExplosionDebris"); //Play explosion sound

        Instantiate(powerUp, transform.position, transform.rotation);

        Destroy(this.gameObject); //Destroy this gameobject
    }
}
