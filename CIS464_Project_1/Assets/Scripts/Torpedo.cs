using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.velocity = transform.forward * speed;
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Ocean")
        {
            if (collision.gameObject.tag == "Player")
            {
                BoatController player = collision.gameObject.GetComponent<BoatController>();
                player.Die();
            }

            Debug.Log("Torpedo Destroyed");
            Destroy(this.gameObject);
        }
    }
}
