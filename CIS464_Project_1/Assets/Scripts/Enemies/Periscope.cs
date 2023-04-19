//This script controls the periscope that appears when the submarine is about to shoot
//The periscope just looks at the player
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Periscope : MonoBehaviour
{
    private Transform player; //Reference to the player
    [SerializeField] private Transform raycastPoint;

    private bool isRotation = false;
    private EnemySubmarine enemySub;

    private void Start()
    {
        player = GetComponentInParent<EnemySubmarine>().player; //Gets a reference to the player from the submarine which it is a child of 
        enemySub = GetComponentInParent<EnemySubmarine>(); 
    }
    void Update()
    {
        TrackPlayer(); //Turns the periscope towards the player
    }

    void TrackPlayer()
    {
        transform.LookAt(player);
    }

    void LookForPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastPoint.position, transform.TransformDirection(Vector3.forward), out hit, 30f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
        }
    }
}
