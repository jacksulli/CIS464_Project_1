using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoatController : MonoBehaviour
{
    //Private variables
<<<<<<< Updated upstream
    private Rigidbody rb;
    private Vector2 playerInput;
    public LevelManager levelManager;
    public GameObject explosion;
    private Camera cam;

<<<<<<< HEAD
    public EnemiesLeftSO enemiesManager;

    [SerializeField] private GameObject sonarPingObject;
=======
=======
    private Rigidbody rb; //Reference to player rigidbody
    private Vector2 playerInput; //Vector2 to determine which direction to move player
    public LevelManager levelManager; //Reference to the level's levelmanager
    public GameObject explosion; //Explosion particle effect for when the boat is destroyed
    private Camera cam; //Main camera
    private bool canDie = true; //Controls whether player is invincible or not
    private bool freezeControls = true; //Controls whether controls are frozen or not

    public EnemiesLeftSO enemiesManager; //Reference to the enemies manager scriptable object

    [SerializeField] private GameObject sonarPingObject; //Sonar ping that appears over enemy boats
>>>>>>> Stashed changes
>>>>>>> Jack

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
<<<<<<< HEAD
    

    //Sonar info
    private bool inSonarMode = false;
    //Cooldown
    public float sonarCoolDown = 5f; //This should be the time it takes a depth charge to explode
    private float nextSonarTime;
    [SerializeField] GameObject sonarUIHolder;
    [SerializeField] GameObject sonarActiveUI;
    [SerializeField] GameObject sonarRechargeUI;



    private bool canDie = true;

    public FloatVariable depthChargeUses;
    private bool freezeControls = true;

=======
<<<<<<< Updated upstream
=======
    
    //Cooldown
    public float sonarCoolDown = 5f; //This should be the time it takes a depth charge to explode
    private float nextSonarTime;  //Private variable to determine when the sonar can be used
    [SerializeField] GameObject sonarUIHolder; //Sonar UI gameobject
    [SerializeField] GameObject sonarActiveUI; //UI Gameobject that says that the sonar is active
    [SerializeField] GameObject sonarRechargeUI; //UI GameObject that says the sonar is recharging

>>>>>>> Stashed changes
>>>>>>> Jack


    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        cam = Camera.main;
=======
<<<<<<< Updated upstream
>>>>>>> Jack
        rb = GetComponent<Rigidbody>(); //Get reference to the boat's rigidbody
        Cursor.visible = false; //Turn off mouse cursor
        Cursor.lockState = CursorLockMode.Locked;

        wakeParticle = transform.GetChild(2).GetComponent<ParticleSystem>().emission;
        frontWakeParticle = transform.GetChild(3).GetComponent<ParticleSystem>().emission; //Change this to search by name
<<<<<<< HEAD

        depthChargeUses.value = 3;

        StartCoroutine(StartSequence());


=======
=======
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


>>>>>>> Stashed changes
>>>>>>> Jack
    }

    void Update()
    {
<<<<<<< HEAD
        if(!freezeControls)
=======
<<<<<<< Updated upstream
        //If Spacebar is pressed, and the cooldown has passed, drop a depth charge
        if(Time.time > nextFireTime)
>>>>>>> Jack
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
<<<<<<< HEAD

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (inSonarMode)
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
            if (inSonarMode)
            {
                if (Time.time > nextSonarTime)
                {
                    sonarRechargeUI.SetActive(false);
                    sonarActiveUI.SetActive(true);
                    
                    if (Input.GetButtonDown("Fire1"))
                    {
                        Vector3 mousePos = Input.mousePosition;
                        Vector3 worldSpacePosition = cam.ScreenToWorldPoint(mousePos);
                        Debug.Log(worldSpacePosition);

                        Instantiate(sonarPingObject, new Vector3(worldSpacePosition.x, 0, worldSpacePosition.z), transform.rotation);
                        //Play temp sound
                        AudioManager.Instance.PlaySound("Sonar");
                        nextSonarTime = Time.time + sonarCoolDown;
                        sonarRechargeUI.SetActive(true);
                        sonarActiveUI.SetActive(false);
                    }
                }
                
            }

            Move();
=======
            
=======
        //If controls are not frozen
        if(!freezeControls) 
        {
            DepthChargeInput(); //Checks if player can drop depth charge

            SonarInput(); //Checks if player can use sonar

            Move(); //Moves boat
>>>>>>> Stashed changes
>>>>>>> Jack
        }
 
    }

<<<<<<< HEAD
    private IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.PlaySound("ShipBell");
        ToggleFreezeControls();
=======
<<<<<<< Updated upstream
        if(Input.GetKeyDown(KeyCode.F))
        {
            //Enter sonar mode

            //Play temp sound
            AudioManager.Instance.PlaySound("Sonar");
        }

        Move();
>>>>>>> Jack
    }


    // Update is called once per frame

=======
    //Fixed Update runes ever 0.02 seconds (configurable in preferences)
    //In this function we put code that we want to always run consistently regardless of framerate
>>>>>>> Stashed changes
    void FixedUpdate()
    {

        //Add force to the player's forward position
        //This allows the boat to have a turning radius when it turns, rather than just snapping
        if (playerInput.x != 0 || playerInput.y != 0)
        {
            rb.AddForce(transform.forward * movementSpeed);
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity); //Clamp the velocity to the max velocity so the boat can't infinitely speed up

        //Changes the wake to be directly tied to the player movement. As the player movement speed increases, the wake increases. 
        //When the player movement speed is 0, there is no wake
        wakeParticle.rateOverTime = wakeBase * wakeMultiplier * rb.velocity.magnitude;
        frontWakeParticle.rateOverTime = wakeBase * wakeMultiplier * rb.velocity.magnitude;

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Play collision sound if the boat hits something
        if(collision.gameObject.tag != "Ocean")
        {
            AudioManager.Instance.PlaySound("Collision");
        }
    }

<<<<<<< HEAD
    void OnEnable()
    {
        enemiesManager.enemiesLeftEvent.AddListener(LevelOver);
    }

=======
<<<<<<< Updated upstream
=======
    void OnEnable()
    {
        //Become a listener for the enemiesManager
        //Whenever that event triggers, the LevelOver function in this script is called
        enemiesManager.enemiesLeftEvent.AddListener(LevelOver); 
    }


    //Start sequence pauses the player for two seconds so the Level number and enemies number can display
    private IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.PlaySound("ShipBell");
        ToggleFreezeControls(); //Unfreeze player controls
    }

    //This function makes it so the player can't die after winning the level
    //Takes in an amount of enenies from the enemy manager event, and if the enemy count is 0, it makes the player invincible
>>>>>>> Jack
    private void LevelOver(int _amount)
    {
        if(_amount <= 0)
        {
            canDie = false;
        }
    }
<<<<<<< HEAD
=======
    
    //Takes in a player input and drops a depth charge
    private void DepthChargeInput()
    {
        //Checks if enough time has passed to drop a new depth charge
        if (Time.time > nextFireTime)
        {
            //If there are still depth charges to drop
            if (depthChargeUses.value > 0)
            {
                //If spacebar is pressed
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    DropDepthCharge();
                    nextFireTime = Time.time + coolDownTime;
                }
            }
        }
    }

    //Takes in a player input and pings the sonar
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

    //This function just rotates the player based on keyboard inputs
    //Code taken and modified from a github and reddit post
>>>>>>> Stashed changes
>>>>>>> Jack
    private void Move()
    {
        playerInput.x = Input.GetAxis("Horizontal"); //If A, D, Left Arrow, or Right Arrow are pressed, this value becomes 1, -1 (or 0 when nothing is pressed)
        playerInput.y = Input.GetAxis("Vertical"); //Same as above except for W,S, Top arrow, down arrow
        playerInput = Vector2.ClampMagnitude(playerInput, 1f); //Make the vector a unit vector so its magnitude is always constant (Magnitude = 1)

        Vector3 targetDirection = new Vector3(playerInput.x, 0, playerInput.y); //Target direction is based on which key the player has pressed

        // The step size is equal to speed times frame time.
        float singleStep = rotationSpeed * Time.deltaTime; //Step size for how much the player will rotate each frame, since move is in the Update Function

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f); //Gets the direction that the player will rotate to this frame

        transform.rotation = Quaternion.LookRotation(newDirection); //Rotate to that direction
    }

  
    //This function spawns the depth charge and flings it away from the boat
    private void DropDepthCharge()
    {
        GameObject droppedItem = Instantiate(depthCharge, dropPosition.position, transform.rotation); //Instantiate the depth charge gameObject
        Rigidbody rig = droppedItem.GetComponent<Rigidbody>(); //Get the rigidbody component from the depth charge object

        //Add a forward and upward force the depth charge to fling it away from the boat
        rig.AddForce(-transform.forward * dropBackwardForce, ForceMode.Impulse);
        rig.AddForce(transform.up * dropUpwardForce, ForceMode.Impulse);
<<<<<<< HEAD
        depthChargeUses.value -= 1;
=======

<<<<<<< Updated upstream
        StartCoroutine(DepthChargeSound());
>>>>>>> Jack
        
    }

    private void ToggleFreezeControls()
    {
<<<<<<< HEAD
        if(freezeControls)
=======
        //Subtract one from the lives counter
        livesManager.DecreaseLives(1);
        Debug.Log("Boom!");
        AudioManager.Instance.PlaySound("Explosion");
        //Set the mesh invisible to mimc being destroyed
        mesh.SetActive(false);
        rb.isKinematic = true;

        Instantiate(explosion, transform.position, transform.rotation);
=======
        depthChargeUses.value -= 1; //Decrease the amount of depth charges available to use by 1
        
    }

    //This function toggles the bool freezeControls between true and false
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

    //This function is called when the player dies
    public void Die()
    {
        if(canDie)
        {
        
            livesManager.DecreaseLives(1); //Subtract one from the lives counter

            AudioManager.Instance.PlaySound("Explosion"); //Play explosion sound

            Instantiate(explosion, transform.position, transform.rotation); //Instantiate the explosion effect game object

            //If the lives are greater than 0, restart the level
            //If the lives are 0, end game
            if (livesManager.value > 0)
            {
                levelManager.ResetCurrentLevel();
            }
            else
            {
                Debug.Log("You Lose!");
                
                levelManager.Loss(); //Generate loss UI
            }

            this.enabled = false; //Turn off this script so the player can't keep dying after they're dead
        }
>>>>>>> Stashed changes
        

<<<<<<< Updated upstream
        //If the lives are greater than 0, restart the level
        //If the lives are 0, end game
        if (livesManager.value > 0)
=======

    //Debug Info -----------------------------------------------------

    //Resets the player speed to the default
    public void ResetSpeed()
    {
        movementSpeed = 13f;
    }

    //Increases the boat speed
    public void IncreaseSpeed()
    {
        movementSpeed += 50f;
        rotationSpeed += .75f;
    }

    //Decreases the boat speed
    public void DecreaseSpeed()
    {
        movementSpeed -= 50f;
        rotationSpeed -= .75f;
    }

    //Toggles whether the player is invincible
    public void ToggleInvincibility()
    {
        if(canDie)
>>>>>>> Stashed changes
>>>>>>> Jack
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
