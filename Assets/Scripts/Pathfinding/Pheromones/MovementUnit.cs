using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUnit : MonoBehaviour
{
    public float moveSpeed;
    public float acceleration;
    public float randomTurnSpeed;
    private float targetTurnSpeed;
    public float turnDeviation;
    public bool drawGizmos;

    [HideInInspector]
    public float currentSpeed;
    private Vector3 desiredDirection;

    private TerrainGenerator terrainGenerator;

    private BoundsInt obstacleSensorBounds;
    private float obstacleDistance;
    Vector3Int size;
    Vector3Int halfSize;
    float offsetDistance;
    Vector3 offsetVector;

    [HideInInspector]
    public Vector3? target;
    [HideInInspector]
    public Vector3 diff;

    public void Initialize(float antMaxSpeed, float antAcceleration, float antTurnSpeed, float antRandomRotation)
    {
        moveSpeed = antMaxSpeed;
        acceleration = antAcceleration;
        randomTurnSpeed = antTurnSpeed;
        turnDeviation = antRandomRotation;

        targetTurnSpeed = randomTurnSpeed * 2;
        size = new Vector3Int(3, 3, 1);
        halfSize = new Vector3Int(size.x / 2, size.y / 2, 0);
        offsetDistance = halfSize.magnitude + 1;
        offsetVector = Vector3.zero;
        target = null;
        terrainGenerator = GameObject.FindWithTag("EnvironmentController").GetComponent<TerrainGenerator>();
    }

    public void MoveAnt(Vector3 externalSteeringForce)
    {
        UpdateScannerBounds();
        Vector3 offset = AvoidUnwalkableTerrain() + externalSteeringForce;

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

        float angle = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, randomTurnSpeed * TimeControls.timeScale);

        //Slow down when close to wall!
        float obstacleDistanceScalar = (obstacleDistance < 4f ? Mathf.Max(0.00001f, ((obstacleDistance - 1) / 4f) - 0.25f) : 1);
        float targetSpeed = Mathf.Max(0, moveSpeed * obstacleDistanceScalar);
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * TimeControls.timeScale * acceleration * 1 / obstacleDistanceScalar);
        transform.position += currentSpeed * Time.deltaTime * TimeControls.timeScale * transform.up;
    }

    private void WalkToTarget()
    {
        GetTargetDistance();

        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, targetTurnSpeed * TimeControls.timeScale);

        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime * TimeControls.timeScale * acceleration);
        transform.position += currentSpeed * Time.deltaTime * TimeControls.timeScale * transform.up;
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
            normal += Vector3.right * 100;
        }
        else if (transform.position.x > terrainGenerator.terrainBounds.max.x - 2)
        {
            normal += Vector3.left * 100;
        }
        if (transform.position.y < terrainGenerator.terrainBounds.min.y + 1)
        {
            normal += Vector3.up * 100;
        }
        else if (transform.position.y > terrainGenerator.terrainBounds.max.y - 2)
        {
            normal += Vector3.down * 100;
        }

        //Get terrain to sense
        //List<TerrainArchetype> sensedTiles = terrainGenerator.GetTerrainInBounds(obstacleSensorBounds).Where(x => x != null && x.walkSpeed == 0).ToList();

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
            TerrainArchetype tt = terrainGenerator.GetTileAtPos(pos);

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
