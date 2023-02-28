using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthChargeBlast : MonoBehaviour
{
    [SerializeField] private float blastRadius = 1f;
    // Update is called once per frame
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
