using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySubmarine : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float minWaitTime = 1f;
    [SerializeField] private float maxWaitTime = 10f;

    [SerializeField] GameObject periscope;

    [SerializeField] private Transform waterRippleObject;
    ParticleSystem.EmissionModule waterRipple;

    [SerializeField] private EnemiesLeftSO enemiesLeft;
    // Start is called before the first frame update
    void Start()
    {
        enemiesLeft.IncreaseLives(1);
        waterRipple = waterRippleObject.GetComponent<ParticleSystem>().emission;
        StartCoroutine(Attack());
    }

    private void Update()
    {
        periscope.transform.LookAt(player, Vector3.up);
    }

    private IEnumerator Attack()
    {
        
        while (true)
        {
            float timeToWait = Random.Range(minWaitTime, maxWaitTime);
            Debug.Log(timeToWait);
            yield return new WaitForSeconds(timeToWait);
            RaisePeriscope();
            yield return new WaitForSeconds(2f);
            LowerPeriscope();
            Shoot();
        }
       
    }

    private void RaisePeriscope()
    {
        waterRipple.rateOverTime = 2f;
        periscope.SetActive(true);
        AudioManager.Instance.PlaySound("Drop1");
    }
    private void LowerPeriscope()
    {
        waterRipple.rateOverTime = 0f;
        periscope.SetActive(false);
        AudioManager.Instance.PlaySound("Drop2");
    }

    private void Shoot()
    {
        Debug.Log("Firing Torpedo!");
    }

    
    public void Die()
    {
        enemiesLeft.DecreaseLives(1);
        //Spawn Explosion Effect
        Debug.Log("Submarine Destroyed!");
        Destroy(this.gameObject);
    }

    private void PlayerImpactPoint()
    {
    
    }

}
