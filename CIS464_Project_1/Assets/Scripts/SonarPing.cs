//This script controlls an invisible sphere that is instantiated during a sonar ping
//This sphere spawns a red dot on any enemy submarines within the sphere
//Originally the sphere would be smaller, and the player would be able to control where the "ping" was
//To simplify this, the sphere just covers the entire map, so a sonar ping just reveals all submarines on the map
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarPing : MonoBehaviour
{
    [SerializeField] private float sonarRadius = 2f; //Refernece to the sonar radius. This should be large enough to cover the map
    [SerializeField] private GameObject enemyDetected; //Gameobject that is instantiated to represent where a submarine is
    
    void Start()
    {
        
        StartCoroutine(LifeCycle()); //Start the LifeCycle coroutine

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sonarRadius); //Create an overlap sphere centered on the center of the map
        foreach (var hitCollider in hitColliders) //Go through every collider in the sphere
        {
            if (hitCollider.gameObject.tag == "Enemy") //If the collider has the "Enemy" tag it is an enemy submarine
            {
                EnemySubmarine theEnemy = hitCollider.gameObject.GetComponent<EnemySubmarine>(); //Get the script of the submarine

                //Spawn the enemyDetected object as a child of this gameobject. This is a red dot
                //This means that when this gameobject is destroyed, the red dots will be destroyed too
                Instantiate(enemyDetected, theEnemy.transform.position, theEnemy.transform.rotation, this.transform); 
            }
        }
    }

    //Destroy this gameobject after two seconds. This will destroy the red dots too
    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    //Shows the overlap sphere in the inspector when gizmos are enabled
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, sonarRadius);
    }
}
