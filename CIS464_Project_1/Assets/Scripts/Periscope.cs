//This script controls the periscope that appears when the submarine is about to shoot
//The periscope just looks at the player
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Periscope : MonoBehaviour
{
    private Transform player; //Reference to the player

    private void Start()
    {
        player = GetComponentInParent<EnemySubmarine>().player; //Gets a reference to the player from the submarine which it is a child of 
    }
    void Update()
    {
        transform.LookAt(player); //Turns the periscope towards the player
    }
}
