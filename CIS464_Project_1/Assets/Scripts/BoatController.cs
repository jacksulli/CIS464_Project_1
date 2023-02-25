using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatController : MonoBehaviour
{
    //Private variables
    private Rigidbody rb;
    private Vector2 playerInput;

    //Movement speed (Force), rotation speed
    public float movementSpeed = 13f;
    public float rotationSpeed = 2f;

    //Maximum velocity
    public float maxVelocity = 1f;

    [SerializeField]
    private GameObject mesh;
    
    
    //-------Depth Charge----------
    //Depth Charge Game Object
    public GameObject depthCharge;

    //Position the depth charge is dropped at
    public Transform dropPosition;

    //Force values for the dropped depth charge
    public float dropBackwardForce;
    public float dropUpwardForce;

    //Cooldown
    public float coolDownTime = 2f; //This should be the time it takes a depth charge to explode
    private float nextFireTime;

    [SerializeField]
    private PlayerLivesSO livesManager;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //Get reference to the boat's rigidbody
        Cursor.visible = false; //Turn off mouse cursor
    }

    void Update()
    {
        //If Spacebar is pressed, and the cooldown has passed, drop a depth charge
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > nextFireTime)
        {
            DropDepthCharge();
            nextFireTime = Time.time + coolDownTime;
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            //Enter sonar mode

            //Play temp sound
            AudioManager.Instance.PlaySound("Sonar");
        }

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
    // Update is called once per frame

    void FixedUpdate()
    {

        //Vector3 movementVector = new Vector3(playerInput.x, 0, playerInput.y) ;
        if (playerInput.x != 0 || playerInput.y != 0)
        {
            rb.AddForce(transform.forward * movementSpeed);
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Ocean")
        {
            AudioManager.Instance.PlaySound("Collision");
        }
    }
    private void DropDepthCharge()
    {
        Debug.Log("Dropping Charge!");

        GameObject droppedItem = Instantiate(depthCharge, dropPosition.position, transform.rotation);
        Rigidbody rig = droppedItem.GetComponent<Rigidbody>();

        rig.AddForce(-transform.forward * dropBackwardForce, ForceMode.Impulse);
        rig.AddForce(transform.up * dropUpwardForce, ForceMode.Impulse);

        StartCoroutine(DepthChargeSound());
        
    }

    public void Die()
    {
        //Subtract one from the lives counter
        livesManager.DecreaseLives(1);
        Debug.Log("Boom!");
        AudioManager.Instance.PlaySound("Explosion");
        //Set the mesh invisible to mimc being destroyed
        mesh.SetActive(false);

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
            SceneManager.LoadScene(0);

        }
        
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
}
