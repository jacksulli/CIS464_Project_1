using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarPing : MonoBehaviour
{
    [SerializeField] private float sonarRadius = 2f;
    [SerializeField] private GameObject enemyDetected;
    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(LifeCycle());

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sonarRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Enemy")
            {
                EnemySubmarine theEnemy = hitCollider.gameObject.GetComponent<EnemySubmarine>();
                Instantiate(enemyDetected, theEnemy.transform.position, theEnemy.transform.rotation, this.transform);
            }
        }
    }

    // Update is called once per frame
    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, sonarRadius);
    }
}
