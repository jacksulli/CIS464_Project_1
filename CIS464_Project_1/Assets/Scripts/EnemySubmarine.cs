using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySubmarine : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float minWaitTime = 1f;
    [SerializeField] private float maxWaitTime = 15f;

    [SerializeField] GameObject periscope;
    [SerializeField] Rigidbody torpedo;
    [SerializeField] GameObject homingTorpedo;

    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject debris;


    [SerializeField] private Transform waterRippleObject;
    ParticleSystem.EmissionModule waterRipple;

    [SerializeField] private EnemiesLeftSO enemiesLeft;

    private float torpedoSpeed;

    private Rigidbody target;


    //AI INFO -----------------------
    private NavMeshAgent agent;
    public float range;

    public Transform centrePoint;

    [SerializeField] private EnemyType enemyType;

    void Start()
    {
        enemiesLeft.IncreaseLives(1);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemyType.subSpeed;
        torpedoSpeed = enemyType.torpedoSpeed;


        waterRipple = waterRippleObject.GetComponent<ParticleSystem>().emission;
        target = player.GetComponent<Rigidbody>();
        StartCoroutine(StartMatch());

    }

    private void Update()
    {
        if(enemyType.canMove)
        {
            MoveAgent();
        }    
        
    }

    //Agent Info from https://www.youtube.com/watch?v=dYs0WRzzoRc
    private void MoveAgent()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                agent.SetDestination(point);
            }
        }

    }

    private IEnumerator StartMatch()
    {
        RaisePeriscope();
        yield return new WaitForSeconds(2f);
        LowerPeriscope();
        yield return new WaitForSeconds(1f);
        StartCoroutine(Attack());
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    private IEnumerator Attack()
    {
        
        while (true)
        {
            float timeToWait = Random.Range(minWaitTime, maxWaitTime);
            //Debug.Log(timeToWait);
            yield return new WaitForSeconds(timeToWait);
            RaisePeriscope();
            yield return new WaitForSeconds(2f);
            LowerPeriscope();

            for(int i = 0; i < enemyType.shotCount; i++)
            {
                Shoot();
                yield return new WaitForSeconds(0.3f);
            }
            
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
        if(enemyType.homingTorpedo)
        {
            var instance = Instantiate(homingTorpedo, transform.position, Quaternion.identity);
            instance.GetComponent<HomingTorpedo>().SetTarget(player.GetChild(0).transform);
        }
        else
        {
            var instance = Instantiate(torpedo, transform.position, rotation: Quaternion.identity);
            if (InterceptionDirection(a: target.transform.position, b: transform.position, vA: target.velocity, torpedoSpeed, result: out var direction))
            {
                instance.velocity = direction * torpedoSpeed;
            }
            else
            {
                instance.velocity = (target.transform.position - transform.position).normalized * torpedoSpeed;
            }
        }
        
     
    }

    
    public void Die()
    {
        enemiesLeft.DecreaseLives(1);
        //Spawn Explosion Effect
        Instantiate(explosionEffect, transform.position, transform.rotation);
        AudioManager.Instance.PlaySound("KilledEnemy");
        Instantiate(debris, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);

        Destroy(this.gameObject);
    }

    //Predictive Aiming ----------------------------------------------------

    public bool InterceptionDirection(Vector3 a, Vector3 b, Vector3 vA, float sB, out Vector3 result)
    {
        //Direction from b to a
        var aToB = b - a;
        var dC = aToB.magnitude;
        var alpha = Vector3.Angle(aToB, vA) * Mathf.Deg2Rad;
        var sA = vA.magnitude;
        var r = sA / sB;

        if(MyMath.SolveQuadratic(a: 1 - r*r, b: 2*r*dC*Mathf.Cos(alpha), c: -(dC*dC), out var root1, out var root2) == 0)
        {
            result = Vector3.zero;
            return false;
        }

        var dA = Mathf.Max(a: root1, b: root2);
        var t = dA / sB;
        var c = a + vA * t;
        result = (c - b).normalized;
        return true;

    }


}

//Methods found from https://www.youtube.com/watch?v=2zVwug_agr0
public class MyMath
{
    public static int SolveQuadratic(float a, float b, float c, out float root1, out float root2)
    {
        var discriminant = b * b - 4 * a * c;
        if(discriminant < 0)
        {
            root1 = Mathf.Infinity;
            root2 = -root1;
            return 0;
        }
        else
        {
            root1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
            root2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);
            return discriminant > 0 ? 2 : 1;
        }
    }
}
