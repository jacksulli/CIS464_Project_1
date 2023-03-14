using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//DO NOT USE, UNDER CONSTRUCTION
public class HomingTorpedo : MonoBehaviour
{
    public Transform goal;
    [SerializeField] private GameObject explosionEffect;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(LifeCycle());
    }

    private void Update()
    {
        agent.SetDestination(goal.position);
    }

    // Update is called once per frame
    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag != "Ocean" && other.gameObject.tag != "Enemy")
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

    public void SetTarget(Transform _target)
    {
        goal = _target;
    }
}