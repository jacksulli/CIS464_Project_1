using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LifeCycle());
    }

    // Update is called once per frame
    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag != "Ocean" && other.gameObject.tag != "Enemy")
        {
            if (other.gameObject.tag == "Player")
            {
                BoatController thePlayer = other.gameObject.GetComponent<BoatController>();
                thePlayer.Die();
                Destroy(this.gameObject);
            }
            else if (other.gameObject.tag == "Mine")
            {
                SeaMine seaMine = other.gameObject.GetComponent<SeaMine>();
                seaMine.Die();
                Destroy(this.gameObject);

            }
            else
            {
                Instantiate(explosionEffect, transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
        }
        
        
        

    }
}
