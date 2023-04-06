//This script controls a scriptable object that determines the properties of an enemy submarine.
//When creating a new enemy submarine, create a prefab with the EnemySubmarine script.
//That script will reference an EnemyType scriptable object like this one to get all the info for that particular enemy type.
//Create one of these scriptable objects any time a new enemy type is needed, and adjust the settings accordingly.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "ScriptableObjects/EnemyType")]
public class EnemyType : ScriptableObject
{
    public string enemyDescription; //Description of the enemy for out of game reasons

    public bool canMove = false; //Can this submarine move around the map

    //DOESNT WORK RIGHT NOW
    public bool homingTorpedo = false; //Does this submarine shoot torpedoes that follow the player

    public int shotCount = 1; //The amount of torpedoes the submarine will fire in one go

    public float torpedoSpeed = 3f; //The speed of the torpedo's speed

    public float subSpeed = 0.5f; //The speed the submarine moves, if it can move

    public bool sharpShooter = false; //Does the submarine predict where the player moves and fire there


}
