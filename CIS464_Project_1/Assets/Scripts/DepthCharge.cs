using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCharge : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField] private GameObject explosion;

    [SerializeField] private FloatVariable depthChargeUses;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LifeCycle());
=======
    [SerializeField] private DepthChargeBlast explosion; //Reference to the explosion effect GameObject

    [SerializeField] private FloatVariable depthChargeUses; //Reference to the scriptable object that determines how many depth charge uses are left

    void Start()
    {
        StartCoroutine(LifeCycle()); //Starts the life cycle of the depth charge gameobject
>>>>>>> Jack
    }

    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(0.4f); //In theory this seconds would depend on the upward force value
<<<<<<< HEAD
        AudioManager.Instance.PlaySound("Splash");
        yield return new WaitForSeconds(3f);
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z), transform.rotation);
        Die();
=======
        AudioManager.Instance.PlaySound("Splash"); //Plays splash sound
        yield return new WaitForSeconds(3f);
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z), transform.rotation); //Spawns the explosion on the depth charges x and z values, and on the 0 y value
        Die(); //Run death cycle function
>>>>>>> Jack
        
    }

    public void Die()
    {
<<<<<<< HEAD
        depthChargeUses.value += 1;
        Destroy(this.gameObject);
=======
        depthChargeUses.value += 1; //When the depth charge dies, add one back to the number of depth charges available to use
        Destroy(this.gameObject); 
>>>>>>> Jack
    }
}

    
