using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
    
    private PlayerController controls; //Controls utilizing the Input System

    public EnemiesLeftSO enemiesManager; //Reference to the enemies manager scriptable object
    [SerializeField] private PlayerStats playerStats; //Reference to the player stats SO which controls the final game score

    [SerializeField] private GameObject sonarPingObject; //Sonar ping that appears over enemy boats

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 13f; //Movement speed (Force)
    [SerializeField] private float rotationSpeed = 3f; //Rotation speed
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
    [SerializeField] private GameObject smokeScreen; //Value to clamp velocity

    [Header("Smoke Screen Info")]
    [SerializeField] private float smokeCoolDown = 3f;
    private float nextSmokeTime;
    [SerializeField] GameObject smokeActiveUI; //UI Gameobject that says that the sonar is active
    [SerializeField] GameObject smokeRechargeUI; //UI GameObject that says the sonar is recharging

    [Header("Sonar Info")]
    [SerializeField] private float sonarCoolDown = 5f;
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

        controls = new PlayerController(); //player controls using the Input System
        controls.Gameplay.DepthCharge.performed += ctx => DropDepthCharge();

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


            //If Q is pressed, and the cooldown has passed, drop a smoke screen
            if (Time.time > nextSmokeTime)
            {
                smokeRechargeUI.SetActive(false); //Turn of the recharging message
                smokeActiveUI.SetActive(true); //Turn on the active message

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    DeploySmokeScreen();
                    nextSmokeTime = Time.time + smokeCoolDown;
                    smokeRechargeUI.SetActive(true); //Turn of the recharging message
                    smokeActiveUI.SetActive(false); //Turn on the active message
                }
            }


            SonarInput(); //Check for the player pressing the sonar button

            Move(); //Check for the player rotating the boat
        }
 
    }

    //Start sequence allows the player to read the level number and enemy number
    private IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(2f); // Wait two seconds
        AudioManager.Instance.PlaySound("ShipBell"); //Play ship bell sound
        if (AudioManager.Instance.musicPlaying == false)
        {
            AudioManager.Instance.PlayRandomTrack();
        }
        ToggleFreezeControls(); //Unfreeze controls
    }


    // Update is called once per frame

    void FixedUpdate()
    {

        //Vector3 movementVector = new Vector3(playerInput.x, 0, playerInput.y) ;
        if (playerInput.x != 0 || playerInput.y != 0)
        {
            rb.AddForce(transform.forward * movementSpeed);
        }

        //Clamp the speed of the boat to a max speed so it can't accelerate forever
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

        //Change the emission rate of the particles to be dependant on the velocity of the player
        wakeParticle.rateOverTime = wakeBase * wakeMultiplier * rb.velocity.magnitude;
        frontWakeParticle.rateOverTime = wakeBase * wakeMultiplier * rb.velocity.magnitude;

    }

    private void OnCollisionEnter(Collision collision)
    {
        //If the boat collides with something other than the ground
        if(collision.gameObject.tag != "Ocean")
        {
            AudioManager.Instance.PlaySound("Collision");
        }
    }

    void OnEnable()
    {
        enemiesManager.enemiesLeftEvent.AddListener(LevelOver); //Make this script a listener for the LevelOver event from the enemiesManager Scriptable Object
        //controls.Gameplay.Enable(); //enable the input system
    }

    private void DeploySmokeScreen()
    {
        AudioManager.Instance.PlaySound("Smoke");
        StartCoroutine(SmokeScreenGeneration());
    }

    IEnumerator SmokeScreenGeneration()
    {
        Instantiate(smokeScreen, dropPosition.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Instantiate(smokeScreen, dropPosition.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Instantiate(smokeScreen, dropPosition.position, transform.rotation);
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

    //Takes in the number of enemies left when the enemiesLeftEvent is called 
    private void LevelOver(int _amount)
    {
        if(_amount <= 0) //If the amount of enemies is 0, make the player invincible so they can't die from a torpedo launched by a dead enemy
        {
            canDie = false;
        }
    }
    private void Move()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f); //Make the vector a unit vector so moving diagonally isn't faster than moving in the other directions

        Vector3 targetDirection = new Vector3(playerInput.x, 0, playerInput.y); //Creates a unit vector along the x-z plane

        float singleStep = rotationSpeed * Time.deltaTime; //Set the size of the rotation step based on the rotation speed

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f); //Create a new direction that is a step away from the current rotation

        transform.rotation = Quaternion.LookRotation(newDirection); //Rotate towards the previously calculated direction
    }

  
    private void DropDepthCharge()
    {
        GameObject droppedItem = Instantiate(depthCharge, dropPosition.position, transform.rotation); //Spawn a depth charge object
        Rigidbody rig = droppedItem.GetComponent<Rigidbody>(); //Get a reference to the depth charge's rigidbody

        rig.AddForce(-transform.forward * dropBackwardForce, ForceMode.Impulse); //Add a backward force to the depth charge
        rig.AddForce(transform.up * dropUpwardForce, ForceMode.Impulse); //Add a upward force to the depth charge
        depthChargeUses.value -= 1; //Decrease the amount of depth charges left to use
        
    }

    //Turns the bool freezeControls to the opposite of what it currently is
    private void ToggleFreezeControls()
    {
        if(freezeControls)
        {
            freezeControls = false;
        }
        else
        {
            freezeControls = true;
        }
        
    }
    public void Die()
    {
        if(canDie)
        {
            canDie = false;
            livesManager.DecreaseLives(1); //Subtract one from the lives counter
            playerStats.playerDeaths += 1;
            AudioManager.Instance.PlaySound("Explosion");

            Instantiate(explosion, transform.position, transform.rotation); //Create an explosion effect

            //If the lives are greater than 0, restart the level
            //If the lives are 0, end game
            if (livesManager.value > 0)
            {
                levelManager.ResetCurrentLevel();
            }
            else
            {
                Debug.Log("Loss!");
                //Generate loss UI
                levelManager.Loss();
            }

            Debug.Log("Disabling Boat");
            this.gameObject.SetActive(false);
        }
        
    }

    //Debug Info -----------------------------------------------------

    public void ResetSpeed()
    {
        movementSpeed = 13f;
    }

    //Increase speed and rotationspeed 
    //Rotation speed is automatically increased as well, since the boat needs to turn faster as well
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

    //Toggles player invincibility
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
