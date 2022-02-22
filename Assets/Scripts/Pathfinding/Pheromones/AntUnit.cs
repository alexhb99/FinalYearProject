using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntUnit : MonoBehaviour
{
    public float moveSpeed;
    public float acceleration;
    public float turnSpeed;
    public float turnDeviation;

    private float currentSpeed;
    private Vector3 desiredDirection;

    private void Update()
    {
        MoveAnt();
    }

    private void MoveAnt()
    {
        Vector3 randDir = Random.insideUnitCircle * turnDeviation;
        desiredDirection = (desiredDirection + randDir).normalized;

        Vector3 desiredSteeringForce = (desiredDirection - transform.up) * turnSpeed;
        Vector3 direction = transform.up + desiredSteeringForce * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime * acceleration);
        transform.position += currentSpeed * Time.deltaTime * transform.up;
    }
}
