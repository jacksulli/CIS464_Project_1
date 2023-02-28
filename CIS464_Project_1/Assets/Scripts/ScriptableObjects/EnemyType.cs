using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "ScriptableObjects/EnemyType")]
public class EnemyType : ScriptableObject
{
    public bool canMove = false;

    public bool homingTorpedo = false;

    public int shotCount = 1;

    public float torpedoSpeed = 3f;

    public float subSpeed = 0.5f;


}
