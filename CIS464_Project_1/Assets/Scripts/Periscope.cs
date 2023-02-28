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
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
    }
}
