using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float moveSpeed;
    public float acceleration;
    public float turnSpeed;
    public float turnDeviation;
    public bool drawGizmos;

    private float currentSpeed;
    private Vector3 desiredDirection;

    private TerrainGenerator terrainGenerator;

    private BoundsInt obstacleSensorBounds;
    private Bounds rightBounds;
    private BoundsInt leftBounds;
    private float obstacleDistance;
    Vector3Int size;
    Vector3Int halfSize;
    float offsetDistance;
    Vector3 offsetVector;

    [HideInInspector]
    public Vector3? target;
    [HideInInspector]
    public Vector3 diff;

    private void Start()
    {
        size = new Vector3Int(3, 3, 1);
        halfSize = new Vector3Int(size.x / 2, size.y / 2, 0);
        offsetDistance = halfSize.magnitude + 1;
        offsetVector = Vector3.zero;
        target = null;
        terrainGenerator = GameObject.FindWithTag("EnvironmentController").GetComponent<TerrainGenerator>();
    }

    public void MoveAnt()
    {
        UpdateScannerBounds();
        Vector3 offset = AvoidUnwalkableTerrain();

        if(target == null)
        {
            RandomWandering(offset);
        }
        else
        {
            WalkToTarget();
        }
    }

    private void RandomWandering(Vector3 offset)
    {
        Vector3 randDir = Random.insideUnitCircle * turnDeviation;
        desiredDirection = (desiredDirection + randDir + offset).normalized;

        Vector3 desiredSteeringForce = (desiredDirection - transform.up) * turnSpeed;
        Vector3 direction = Vector3.RotateTowards(transform.up, desiredDirection, turnSpeed * (1 - Vector3.Dot(transform.up, desiredDirection)), 0f); //transform.up + desiredSteeringForce * Time.deltaTime;
        direction.z = 0;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        float targetSpeed = Mathf.Max(0, moveSpeed * (obstacleDistance < 4f ? (obstacleDistance - 1) / 4f : 1));
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * acceleration);
        transform.position += currentSpeed * Time.deltaTime * transform.up;
    }

    private void WalkToTarget()
    {
        GetTargetDistance();
        Vector3 direction = Vector3.RotateTowards(transform.up, diff, turnSpeed * (1 - Vector3.Dot(transform.up, diff)), 0f); //transform.up + desiredSteeringForce * Time.deltaTime;
        direction.z = 0;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime * acceleration);
        transform.position += currentSpeed * Time.deltaTime * transform.up;
    }

    public void GetTargetDistance()
    {
        diff = (Vector3)(target - transform.position);
        diff.z = 0;
    }

    private void UpdateScannerBounds()
    {
        Vector3Int position = new Vector3Int(Mathf.RoundToInt(transform.position.x + transform.up.x * offsetDistance) - halfSize.x, Mathf.RoundToInt(transform.position.y + transform.up.y * offsetDistance) - halfSize.y, Mathf.RoundToInt(transform.position.z));
        obstacleSensorBounds = new BoundsInt(position, size);
        leftBounds = new BoundsInt(position, halfSize + Vector3Int.one);
        rightBounds = new Bounds(transform.position + transform.up * offsetDistance, size);
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.DrawWireCube(obstacleSensorBounds.center - Vector3.one / 2f, obstacleSensorBounds.size);
            Gizmos.color = Color.red;
            
            BoundsInt.PositionEnumerator allPos = obstacleSensorBounds.allPositionsWithin;
            foreach(Vector3Int pos in allPos)
            {
                if(Vector3.Cross(pos - transform.position, transform.up * offsetDistance * 2).z < 0)
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawCube(pos, Vector3.one / 2f);
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.up * offsetDistance * 2);
            Gizmos.color = Color.white;
            Gizmos.DrawRay(transform.position, offsetVector);
        }
    }

    //Move away from borders and unwalkable terrain
    private Vector3 AvoidUnwalkableTerrain()
    {
        Vector3 normal = Vector3.zero;

        //Avoid borders
        if(transform.position.x < terrainGenerator.terrainBounds.min.x + 1)
        {
            normal += Vector3.right;
        }
        else if (transform.position.x > terrainGenerator.terrainBounds.max.x - 2)
        {
            normal += Vector3.left;
        }
        if (transform.position.y < terrainGenerator.terrainBounds.min.y + 1)
        {
            normal += Vector3.up;
        }
        else if (transform.position.y > terrainGenerator.terrainBounds.max.y - 2)
        {
            normal += Vector3.down;
        }

        //Get terrain to sense
        List<TerrainType> sensedTiles = terrainGenerator.GetTerrainInBounds(obstacleSensorBounds).Where(x => x != null && x.walkSpeed == 0).ToList();

        Vector3 closestObstacle = Vector3.positiveInfinity;
        obstacleDistance = Mathf.Infinity;
        BoundsInt.PositionEnumerator allPos = obstacleSensorBounds.allPositionsWithin;
        int leftSensorCount = 0;
        int leftSensorWallCount = 0;
        int rightSensorCount = 0;
        int rightSensorWallCount = 0;

        //Go through each tile in sensed terrain
        foreach (Vector3Int pos in allPos)
        {
            TerrainType tt = terrainGenerator.GetTileAtPos(pos);

            //Separate between left and right sensors
            if (Vector3.Cross(pos - transform.position, transform.up * offsetDistance * 2).z < 0)
            {
                leftSensorCount++;//Left
                if (tt != null && tt.walkSpeed == 0)
                {
                    leftSensorWallCount++;
                }
            }
            else
            {
                rightSensorCount++; //Right
                if (tt != null && tt.walkSpeed == 0)
                {
                    rightSensorWallCount++;
                }
            }

            //If unwalkable...
            if (tt != null && tt.walkSpeed == 0)
            {
                //Check if its the closest unwalkable tile, and set closest distance if so
                Vector3 dstToObstacle = transform.position - pos;
                if (dstToObstacle.sqrMagnitude < closestObstacle.sqrMagnitude)
                {
                    closestObstacle = dstToObstacle;
                }
                //Add to normal
                normal += dstToObstacle;
            }
        }


        if(closestObstacle.x != float.PositiveInfinity)
        {
            obstacleDistance = closestObstacle.sqrMagnitude;
        }

        normal = normal.normalized;
        offsetVector = normal;
        if (normal != Vector3.zero)
        {
            //A vector at either -90 or 90 degrees to the front facing vector (transform.up) depending on left and right unwalkable terrain counts
            Vector3 incoming = (float)leftSensorWallCount / leftSensorCount < (float)rightSensorWallCount / rightSensorCount ? new Vector3(-transform.up.y, transform.up.x) : new Vector3(transform.up.y, -transform.up.x);

            if(Vector3.Dot(normal, transform.up) <= 0 && obstacleDistance > 3f)
            {
                offsetVector = incoming;
                return incoming;
            }
            else if(obstacleDistance <= 3f)
            {
                return -transform.up;
            }
            return normal;
        }
        return Vector3.zero;
    }
}
