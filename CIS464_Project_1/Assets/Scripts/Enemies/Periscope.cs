//This script controls the periscope that appears when the submarine is about to shoot
//The periscope just looks at the player
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Periscope : MonoBehaviour
{
    private Transform player; //Reference to the player
    [SerializeField] private Transform raycastPoint;

    public bool hasTarget = false;
    private EnemySubmarine enemySub;

    public float rotationTime = 3f; // default rotation time is 1 second

    private Transform target;
    public float currentRotationTime = 0f;
    public bool isRotating = true;

    public bool inStartSequence = true;

    private void Start()
    {
        player = GetComponentInParent<EnemySubmarine>().player; //Gets a reference to the player from the submarine which it is a child of 
        enemySub = GetComponentInParent<EnemySubmarine>(); 
    }
    void Update()
    {
        if(hasTarget)
        {
            TrackPlayer(); //Turns the periscope towards the player
        }
        else if(!inStartSequence)
        {
            LookForPlayer();
        } 
    }

    void TrackPlayer()
    {
        transform.LookAt(player);
    }

    void LookForPlayer()
    {
        
        transform.Rotate(new Vector3(0f, 360f / rotationTime * Time.deltaTime, 0f)); //Rotate the object around the Z axis

        // update the current rotation time
        currentRotationTime += Time.deltaTime;

        // check if we've completed the rotation
        if (currentRotationTime >= rotationTime)
        {
            isRotating = false;
            currentRotationTime = 0f;
        }

        RaycastHit hit;
        if (Physics.Raycast(raycastPoint.position, transform.TransformDirection(Vector3.forward), out hit, 30f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

            if (hit.collider.CompareTag("Player"))
            {
                target = hit.collider.transform;
                hasTarget = true;
                isRotating = false;
            }
        }
    }

}
