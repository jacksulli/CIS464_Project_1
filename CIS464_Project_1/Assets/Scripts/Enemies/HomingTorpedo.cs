using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//DO NOT USE
//UNDER CONSTRUCTION
public class HomingTorpedo : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    Transform target;
    private float rotationSpeed = 0.1f;
    private float torpedoSpeed = 0.1f;
    private Rigidbody rb;
    private bool trackingPlayer = true;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LifeCycle());
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(trackingPlayer)
        {
            LookForPlayer();
        }
        else
        {
            Spin();
        }

        Move();

    }

    private void LookForPlayer()
    {
        if(target != null)
        {
            Vector3 targetDirection = (target.position - transform.position).normalized;

            targetDirection.y = 0;

            float singleStep = rotationSpeed * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 1f);

            transform.rotation = Quaternion.LookRotation(newDirection); 
        }
        
    }

    private void Spin()
    {
        Vector3 targetDirection = (new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f)) - transform.position).normalized;

        targetDirection.y = 0;

        float singleStep = rotationSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 1f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void Move()
    {
        rb.AddForce(transform.forward * torpedoSpeed);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, torpedoSpeed);
    }

    // Update is called once per frame
    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(10f);
        Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the torpedo hits something besides the ground or the enemy submarines
        if (other.gameObject.tag != "Ocean" && other.gameObject.tag != "Enemy")
        {
            //If the torpedo hits a player
            if (other.gameObject.tag == "Player")
            {
                BoatController thePlayer = other.gameObject.GetComponent<BoatController>(); //Get a reference to the boat's script
                thePlayer.Die(); //Kill the player
                Destroy(this.gameObject);
            }
            else if (other.gameObject.tag == "Mine") //If the torpedo hits a mine
            {
                SeaMine seaMine = other.gameObject.GetComponent<SeaMine>(); //Get a reference to the mine's script
                AudioManager.Instance.PlaySound("TorpedoExplosion"); //Play the torpedo death sound
                seaMine.Die(); //Destroy the mine. In the future make this make the mine blow up
                Destroy(this.gameObject);

            }
            else if (other.gameObject.GetComponent<DepthCharge>()) //If the torpedo hits a falling depth charge
            {
                DepthCharge theCharge = other.gameObject.GetComponent<DepthCharge>(); //Get a reference to the depth charge that was hit
                theCharge.Die(); //Destroy the depth charge
                AudioManager.Instance.PlaySound("TorpedoExplosion");
                Instantiate(explosionEffect, transform.position, transform.rotation); //Instantiate the torpedo death explosion at the impact point
                Destroy(this.gameObject);
            }
            else if (other.gameObject.tag == "Smoke") //If the torpedo hits a mine
            {
                trackingPlayer = false;
                rotationSpeed = 6f;
            }
            else //If the torpedo hits anything else
            {
                AudioManager.Instance.PlaySound("TorpedoExplosion");
                Instantiate(explosionEffect, transform.position, transform.rotation); //Instantiate the torpedo death explosion at the impact point
                Destroy(this.gameObject);
            }
        }
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetRotationSpeed(float _rotationSpeed)
    {
        rotationSpeed = _rotationSpeed;
    }

    public void SetSpeed(float _speed)
    {
        torpedoSpeed = _speed;
    }

    public void Die()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation); //Instantiate the torpedo death explosion at the impact point
        Destroy(this.gameObject);
    }
}