//This script is the blast from a depth charge.
//It destroys any enemy submarines within the blast radius

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthChargeBlast : MonoBehaviour
{
    [SerializeField] private float blastRadius = 1f; //The size of the depth charge explosion
    
    private void Start()
    {
        AudioManager.Instance.PlaySound("WaterExplosion");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRadius); //Spawn an overlap sphere for the depth charge explosion
        foreach (var hitCollider in hitColliders) //For every collider within the sphere
        {
            if (hitCollider.gameObject.tag == "Enemy") //If an enemy submarine is within the blast radius
            {
                EnemySubmarine theEnemy = hitCollider.gameObject.GetComponent<EnemySubmarine>(); //Get a reference to the submarine
                theEnemy.Die(); //Kill the submarine
            }
        }

        StartCoroutine(Die()); //Turn off this explosion object

    }

    //Wait for two seconds then destroy this object
    public IEnumerator Die()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    //Shows the radius of the depth charge explosion in the inspector if gizmos are enabled
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, blastRadius);
    }
}
