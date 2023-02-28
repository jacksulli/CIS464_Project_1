using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatController : MonoBehaviour
{
    //Private variables
    private Rigidbody rb;
    private Vector2 playerInput;
    public LevelManager levelManager;
    public GameObject explosion;
    private Camera cam;

    [SerializeField] private GameObject sonarPingObject;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 13f; //Movement speed (Force)
    [SerializeField] private float rotationSpeed = 2f; //Rotation speed
    [SerializeField] private float maxVelocity = 1f;


    [Header("Boat Wake")]
    //WakeParticle System Settings
    ParticleSystem.EmissionModule wakeParticle, frontWakeParticle;
    [SerializeField] private float wakeMultiplier;
    [SerializeField] private float wakeBase;


    //-------Depth Charge------------------------------
    [Header("Depth Charge Settings")]
    //Depth Charge Game Object
    [SerializeField] private GameObject depthCharge;
    //Position the depth charge is dropped at
    [SerializeField] private Transform dropPosition;
    //Force values for the dropped depth charge
    [SerializeField] private float dropBackwardForce;
    [SerializeField] private float dropUpwardForce;
    //Cooldown
    public float coolDownTime = 2f; //This should be the time it takes a depth charge to explode
    private float nextFireTime;


    [Header("Other")]
    [SerializeField] private GameObject mesh; //Reference to the mesh of the boat object
    [SerializeField] private PlayerLivesSO livesManager; //Reference to the player lives

    private bool inSonarMode = false;
    private bool canDie = true;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>(); //Get reference to the boat's rigidbody
        Cursor.visible = false; //Turn off mouse cursor
        Cursor.lockState = CursorLockMode.Locked;

        wakeParticle = transform.GetChild(2).GetComponent<ParticleSystem>().emission;
        frontWakeParticle = transform.GetChild(3).GetComponent<ParticleSystem>().emission; //Change this to search by name
    }

    void Update()
    {
        //If Spacebar is pressed, and the cooldown has passed, drop a depth charge
        if(Time.time > nextFireTime)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                DropDepthCharge();
                nextFireTime = Time.time + coolDownTime;
            }
            
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(inSonarMode)
            {
                inSonarMode = false;
                ExitSonarMode();
            }
            else
            {
                inSonarMode = true;
                EnterSonarMode();
            }
            //Enter sonar mode

        }

        if(inSonarMode)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 worldSpacePosition = cam.ScreenToWorldPoint(mousePos);
                Debug.Log(worldSpacePosition);

                Instantiate(sonarPingObject, new Vector3(worldSpacePosition.x, 0, worldSpacePosition.z), transform.rotation);
                //Play temp sound
                AudioManager.Instance.PlaySound("Sonar");
            }
        }

        Move();
    }


    // Update is called once per frame

    void FixedUpdate()
    {

        //Vector3 movementVector = new Vector3(playerInput.x, 0, playerInput.y) ;
        if (playerInput.x != 0 || playerInput.y != 0)
        {
            rb.AddForce(transform.forward * movementSpeed);
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

        wakeParticle.rateOverTime = wakeBase * wakeMultiplier * rb.velocity.magnitude;
        frontWakeParticle.rateOverTime = wakeBase * wakeMultiplier * rb.velocity.magnitude;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Ocean")
        {
            AudioManager.Instance.PlaySound("Collision");
        }
    }

    private void Move()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        Vector3 targetDirection = new Vector3(playerInput.x, 0, playerInput.y);

        // The step size is equal to speed times frame time.
        float singleStep = rotationSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

  
    private void DropDepthCharge()
    {
        //Debug.Log("Dropping Charge!");

        GameObject droppedItem = Instantiate(depthCharge, dropPosition.position, transform.rotation);
        Rigidbody rig = droppedItem.GetComponent<Rigidbody>();

        rig.AddForce(-transform.forward * dropBackwardForce, ForceMode.Impulse);
        rig.AddForce(transform.up * dropUpwardForce, ForceMode.Impulse);

        StartCoroutine(DepthChargeSound());
        
    }

    public void Die()
    {
        if(canDie)
        {
            //Subtract one from the lives counter
            livesManager.DecreaseLives(1);
            Debug.Log("Boom!");
            AudioManager.Instance.PlaySound("Explosion");
            //Set the mesh invisible to mimc being destroyed
            mesh.SetActive(false);
            rb.isKinematic = true;

            Instantiate(explosion, transform.position, transform.rotation);


            //If the lives are greater than 0, restart the level
            //If the lives are 0, end game
            if (livesManager.value > 0)
            {
                StartCoroutine(ResetLevel());
            }
            else
            {
                Debug.Log("You Lose!");
                //Generate loss UI
                levelManager.Loss();
            }
        }
        
    }

    private void EnterSonarMode()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitSonarMode()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public IEnumerator ResetLevel()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator DepthChargeSound()
    {
        yield return new WaitForSeconds(0.4f); //In theory this seconds would depend on the upward force value
        AudioManager.Instance.PlaySound("Splash");
    }

    //Debug Info -----------------------------------------------------

    public void IncreaseSpeed()
    {
        movementSpeed += 1;
    }

    public void DecreaseSpeed()
    {
        movementSpeed -= 1;
    }

    public void ToggleInvincibility()
    {
        if(canDie)
        {
            canDie = false;
            Debug.Log("You are now invincible!");
        }
        else
        {
            if (canDie)
            {
                canDie = true;
                Debug.Log("You are now vincible!");
            }
        }
        
    }


}
