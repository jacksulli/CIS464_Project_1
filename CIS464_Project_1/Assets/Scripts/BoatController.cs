using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float movementSpeed = 13f;
    public float maxVelocity = 1f;

    public Rigidbody rb;
    Vector2 playerInput;
    public float rotationSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
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
}
