using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Periscope : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        player = GetComponentInParent<EnemySubmarine>().player;
    }
    void Update()
    {
        transform.LookAt(player);
    }
}
