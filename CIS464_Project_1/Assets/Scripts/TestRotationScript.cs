using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotationScript : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = (target.position - transform.position).normalized;

        targetDirection.y = 0;

        float singleStep = rotationSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 1f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
