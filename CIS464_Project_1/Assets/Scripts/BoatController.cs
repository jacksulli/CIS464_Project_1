using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoatController : MonoBehaviour
{
    //Private variables
    private Rigidbody rb; //Reference to player rigidbody
    private Vector2 playerInput; //Vector2 to determine which direction to move player
    public LevelManager levelManager; //Reference to the level's levelmanager
    public GameObject explosion; //Explosion particle effect for when the boat is destroyed
    private Camera cam; //Main camera
    private bool canDie = true; //Controls whether player is invincible or not
    private bool freezeControls = true; //Controls whether controls are frozen or not

    public EnemiesLeftSO enemiesManager; //Reference to the enemies manager scriptable object

    [SerializeField] private GameObject sonarPingObject; //Sonar ping that appears over enemy boats

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 13f; //Movement speed (Force)
    [SerializeField] private float rotationSpeed = 2f; //Rotation speed
    [SerializeField] private float maxVelocity = 1f; //Value to clamp velocity


    [Header("Boat Wake")]
    //WakeParticle System Settings
    ParticleSystem.EmissionModule wakeParticle, frontWakeParticle; //Reference to the wake objects
    [SerializeField] private float wakeMultiplier; //Multiplier for number of particles created
    [SerializeField] private float wakeBase; //Base number of particles


    //-------Depth Charge------------------------------
    [Header("Depth Charge Settings")]
    [SerializeField] private GameObject depthCharge; //Depth charge gameobject that will be dropped
    [SerializeField] private Transform dropPosition; //Position the depth charge will be dropped at
    [SerializeField] private float dropBackwardForce, dropUpwardForce; //Force values for launching depth charge
    [SerializeField] private float coolDownTime = 2f; //Ideally this should be the same time as it takes a depth charge to explode
    private float nextFireTime; //Private variable to determine when the depth charge can be dropped

    //Scriptable object variable, determines how many depth charges are in play
    public FloatVariable depthChargeUses;

    [Header("Other")]
    [SerializeField] private GameObject mesh; //Reference to the mesh of the boat object
    [SerializeField] private PlayerLivesSO livesManager; //Reference to the player lives


    //Sonar info
    private bool inSonarMode = false;
    //Cooldown
    public float sonarCoolDown = 5f; //This should be the time it takes a depth charge to explode
    private float nextSonarTime;  //Private variable to determine when the sonar can be used
    [SerializeField] GameObject sonarUIHolder; //Sonar UI gameobject
    [SerializeField] GameObject sonarActiveUI; //UI Gameobject that says that the sonar is active
    [SerializeField] GameObject sonarRechargeUI; //UI GameObject that says the sonar is recharging





    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main; //Get reference to main camera
        rb = GetComponent<Rigidbody>(); //Get reference to the boat's rigidbody
        Cursor.visible = false; //Turn off mouse cursor
        Cursor.lockState = CursorLockMode.Locked; //Lock mouse Cursor

        //Get reference to wake particle systems
        wakeParticle = transform.GetChild(2).GetComponent<ParticleSystem>().emission;
        frontWakeParticle = transform.GetChild(3).GetComponent<ParticleSystem>().emission; //Ideally should change this to search by name

        //Set the depth charge uses left
        depthChargeUses.value = 3;

        //Start the start sequence that freezes the boat at the beginning of the level
        StartCoroutine(StartSequence());


    }

    void Update()
    {
        if(!freezeControls)
        {
            //If Spacebar is pressed, and the cooldown has passed, drop a depth charge
            if (Time.time > nextFireTime)
            {
                if (depthChargeUses.value > 0)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        DropDepthCharge();
                        nextFireTime = Time.time + coolDownTime;
                    }
                }
            }

            SonarInput();

            Move();
        }
 
    }

    private IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.PlaySound("ShipBell");
        ToggleFreezeControls();
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

    void OnEnable()
    {
        enemiesManager.enemiesLeftEvent.AddListener(LevelOver);
    }

    private void SonarInput()
    {
        //If the game's time is greater than the next time the player is allowed to use the sonar
        if (Time.time > nextSonarTime)
        {
            sonarRechargeUI.SetActive(false); //Turn of the recharging message
            sonarActiveUI.SetActive(true); //Turn on the active message

            //If the player presses 'F'
            if (Input.GetKeyDown(KeyCode.F))
            {
                //This code is for the game function allowing the player to ping a specific area of the map
                //Right now, I have it so the ping area is so large that it just pings the whole map
                Vector3 mousePos = Input.mousePosition;  //Vector 3 that gets the mouse position on the screen (y value will be 0)
                Vector3 worldSpacePosition = cam.ScreenToWorldPoint(mousePos); //Gets the mouse position in worldspace

                Instantiate(sonarPingObject, new Vector3(worldSpacePosition.x, 0, worldSpacePosition.z), transform.rotation); //Instantiate the sonar ping object, which is an invisible sphere object
                AudioManager.Instance.PlaySound("Sonar"); //Play sonar sound
                nextSonarTime = Time.time + sonarCoolDown; //Set the next time the player can use the sonar, which is the current time plus the cooldown time
                sonarRechargeUI.SetActive(true); //Turn on the UI object that says "Recharging"
                sonarActiveUI.SetActive(false); //Turn off the UI object that says "Sonar Actice"
            }
        }
    }

    private void LevelOver(int _amount)
    {
        if(_amount <= 0)
        {
            canDie = false;
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
        depthChargeUses.value -= 1;
        
    }

    private void ToggleFreezeControls()
    {
        if(freezeControls)
        {
            freezeControls = false;
        }
        else
        {
            if (inSonarMode)
            {
                ExitSonarMode();
            }
            freezeControls = true;
        }
        
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

            Instantiate(explosion, transform.position, transform.rotation);


            //If the lives are greater than 0, restart the level
            //If the lives are 0, end game
            if (livesManager.value > 0)
            {
                levelManager.ResetCurrentLevel();
            }
            else
            {
                Debug.Log("You Lose!");
                //Generate loss UI
                levelManager.Loss();
            }

            this.enabled = false;
        }
        
    }

    private void EnterSonarMode()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        sonarUIHolder.SetActive(true);
    }

    public void ExitSonarMode()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        sonarUIHolder.SetActive(false);
    }


    //Debug Info -----------------------------------------------------

    public void ResetSpeed()
    {
        movementSpeed = 13f;
    }
    public void IncreaseSpeed()
    {
        movementSpeed += 50f;
        rotationSpeed += .75f;
    }

    public void DecreaseSpeed()
    {
        movementSpeed -= 50f;
        rotationSpeed -= .75f;
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
