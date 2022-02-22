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

    private TerrainGenerator terrainGenerator;

    private void Start()
    {
        terrainGenerator = GameObject.FindWithTag("EnvironmentController").GetComponent<TerrainGenerator>();
    }

    private void Update()
    {
        MoveAnt();
    }

    private void MoveAnt()
    {
        Vector3 offset = GetBorderOffset();

        Vector3 randDir = Random.insideUnitCircle * turnDeviation;
        desiredDirection = (desiredDirection + randDir + offset * 100f).normalized;

        Vector3 desiredSteeringForce = (desiredDirection - transform.up) * turnSpeed;
        Vector3 direction = transform.up + desiredSteeringForce * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime * acceleration);
        transform.position += currentSpeed * Time.deltaTime * transform.up;
    }

    //Move away from the borders of the map
    private Vector3 GetBorderOffset()
    {
        Vector3 normal = Vector3.zero;

        if(transform.position.x < terrainGenerator.terrainBounds.min.x + 1)
        {
            normal += Vector3.right;
        }
        else if (transform.position.x > terrainGenerator.terrainBounds.max.x - 1)
        {
            normal += Vector3.left;
        }
        if (transform.position.y < terrainGenerator.terrainBounds.min.y + 1)
        {
            normal += Vector3.up;
        }
        else if (transform.position.y > terrainGenerator.terrainBounds.max.y - 1)
        {
            normal += Vector3.down;
        }

        normal = normal.normalized;
        if(normal != Vector3.zero)
        {
            Vector3 incoming = Vector3.Dot(transform.up, Vector3.Cross(normal, Vector3.forward)) > 0 ? new Vector3(-transform.up.y, transform.up.x) : new Vector3(transform.up.y, -transform.up.x);
            if(Vector3.Dot(normal, transform.up) > 0)
            {
                return normal;
            }
            else
            {
                return incoming;
            }
        }
        return Vector3.zero;
    }
}
