using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//This script controls the enemy AI
//Many variables in this script are dependant on the EnemyType scriptable object
//This allows us to create different enemy types, and have this script take the variables from the scriptable object that we created
public class EnemySubmarine : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float minWaitTime = 1f; //Wait time between attacks
    [SerializeField] private float maxWaitTime = 15f;

    [SerializeField] GameObject periscope; //Reference to periscope game object
    [SerializeField] Rigidbody torpedo; //Reference to the torpedo when it is fired
    [SerializeField] GameObject homingTorpedo; 

    [SerializeField] GameObject explosionEffect; //Get reference to the explosion effect
    [SerializeField] GameObject debris; //Reference to debris gameobject


    [SerializeField] private Transform waterRippleObject;
    ParticleSystem.EmissionModule waterRipple; //Reference to the emission module of the waterRipple to easily control the rate

    [SerializeField] private EnemiesLeftSO enemiesLeft; //Reference to the enemies left scriptable object

    private float torpedoSpeed; //torpedo speed, determined by enemy type scriptable object

    private Rigidbody target;



    //AI INFO -----------------------
    private NavMeshAgent agent; //Reference to the navmesh agent
    public float range; //Range that the AI can create points to go to

    public Transform centerPoint; //Center point of the AI

    [SerializeField] private EnemyType enemyType; //Reference to the enemy type scriptable object that determines most of the variables in the script

    void Start()
    {
        enemiesLeft.IncreaseLives(1); //On game start, each enemy submarine will increase the enemy counter by 1
        agent = GetComponent<NavMeshAgent>(); //Get a reference to the object's navMeshAgent
        agent.speed = enemyType.subSpeed; //Set enemy's speed based on the enemy type
        torpedoSpeed = enemyType.torpedoSpeed; //Set torpedo speed based on enemy type


        waterRipple = waterRippleObject.GetComponent<ParticleSystem>().emission; //Get a reference to the water ripple particle effect
        target = player.GetComponent<Rigidbody>(); //Get a reference to the player
        StartCoroutine(StartMatch()); //Start the beginning of match sequence

    }

    private void Update()
    {
        //If this enemy type can move then move it
        if(enemyType.canMove && agent.enabled == true)
        {
            MoveAgent();
        }    
        
    }

    //Agent Info from https://www.youtube.com/watch?v=dYs0WRzzoRc
    private void MoveAgent()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) //If the navmeshagent has reached its destination
        {
            Vector3 point; //Create vector 
            if (RandomPoint(centerPoint.position, range, out point)) //Create a random point within a certain radius (range), and get that point
            {
                agent.SetDestination(point); //Set the AI destination to that poimt
            }
        }

    }

    //Start sequence for the level. Submarine is frozen for 3 seconds, and then the Attack cycle starts
    private IEnumerator StartMatch()
    {
        RaisePeriscope();
        yield return new WaitForSeconds(2f);
        LowerPeriscope();
        yield return new WaitForSeconds(1f);
        StartCoroutine(Attack());
    }

    //This creates a random point for the submarine to travel to. If the point is created, the function returns true.
    //If the function can't create a valid point for the AI to travel to, it returns false
    //This is taken directly from the unity documentation for navmesh agents
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

    //This coroutine controls the submarine's attack cycle.
    //The Coroutine runs a while loop until the submarine is destroyed
    private IEnumerator Attack()
    {
        
        while (true)
        {
            float timeToWait = Random.Range(minWaitTime, maxWaitTime); //Wait a random amount of time within a boundary in between each submarine attack
            yield return new WaitForSeconds(timeToWait);
            agent.enabled = false; //Turn off navmeshagent so moving submarines will stop during their attack cycle
            RaisePeriscope(); //Raise periscope
            yield return new WaitForSeconds(2f);
            LowerPeriscope(); //Lower periscope
            yield return new WaitForSeconds(1f);

            //Submarine shoots a total of 3 seconds after raising the periscope
            for (int i = 0; i < enemyType.shotCount; i++) //Shoot as many torpedoes as the enemy type allows
            {
                Shoot();
                yield return new WaitForSeconds(0.3f); //Wait 0.3 seconds in between each shot
            }

            agent.enabled = true; //Turn on the navmeshagent
        }
       
    }

    private void RaisePeriscope()
    {
        waterRipple.rateOverTime = 2f; //Turn on the ripple effect by increasing the particle emissions over time
        periscope.SetActive(true); //Turn on the periscope gameobject
        AudioManager.Instance.PlaySound("Drop1");
    }
    private void LowerPeriscope()
    {
        waterRipple.rateOverTime = 0f; //Turn off water ripple
        periscope.SetActive(false); //Turn off the periscope gameobject
        AudioManager.Instance.PlaySound("Drop2"); //Play the drop sound
    }

    private void Shoot()
    {
        //Shoots a homing torpedo if the enemy type allows this
        //STILL UNDER CONSTRUCTION!!!, the homing does not work that well
        if(enemyType.homingTorpedo)
        {
            GameObject instance = Instantiate(homingTorpedo, transform.position, Quaternion.identity);
            instance.GetComponent<HomingTorpedo>().SetTarget(player.GetChild(0).transform);
        }
        else
        {
            var instance = Instantiate(torpedo, transform.position, rotation: Quaternion.identity);  //Instantiate the torpedo object, reset rotation

            //If it can find a path to intercept the player, fire in that direction
            if (InterceptionDirection(a: target.transform.position, b: transform.position, vA: target.velocity, torpedoSpeed, result: out var direction))
            {
                instance.velocity = direction * torpedoSpeed; //Fire the torpedo in the direction calculated by InterceptionDirection function
            }
            else
            {
                //Just fire at the player's current position
                //Set the velocity direction of the torpedo. Since we normalize it, its magnitude will be whatever the variable torpedoSpeed is
                instance.velocity = (target.transform.position - transform.position).normalized * torpedoSpeed; 
            }
        }
        
     
    }

    
    public void Die()
    {
        enemiesLeft.DecreaseLives(1); //When the enemy dies, decrease the number of lives left

        Instantiate(explosionEffect, transform.position, transform.rotation); //Spawn the explosion effect
        AudioManager.Instance.PlaySound("KilledEnemy");
        Instantiate(debris, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation); //Instantiate some debris on the water

        Destroy(this.gameObject);
    }

    //Predictive Aiming ----------------------------------------------------
    //Methods found from https://www.youtube.com/watch?v=2zVwug_agr0

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
