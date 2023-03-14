using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is a simple overlap sphere that checks for any enemy submarines inside of it.
//The sphere only checks for submarines at the start frame, then dissapears.
public class DepthChargeBlast : MonoBehaviour
{
    [SerializeField] private float blastRadius = 1f; //Variable for the radius of the depth charge blast (or the overlap sphere)

    private void Start()
    {
        AudioManager.Instance.PlaySound("WaterExplosion");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Enemy")
            {
                EnemySubmarine theEnemy = hitCollider.gameObject.GetComponent<EnemySubmarine>();
                theEnemy.Die();
            }
        }

        StartCoroutine(Die());

    }

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, blastRadius);
    }
}
