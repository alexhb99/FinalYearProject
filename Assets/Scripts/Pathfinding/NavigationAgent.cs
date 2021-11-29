using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationAgent : MonoBehaviour
{
    public List<Vector3> path;
    public float speed = 5;
    public bool isMoving;

    private Coroutine moveCoroutine;
    private AStar pathfinding;

    private void Start()
    {
        pathfinding = GameObject.FindWithTag("Pathfinding").GetComponent<AStar>();
        isMoving = false;
    }

    private void FixedUpdate()
    {
        if (!isMoving)
        {
            GetRandomPath();
            StartMoving();
        }
    }

    /*
    private Vector3 targetPos;
    void OnDrawGizmos()
    {
        if (path != null)
        {
            foreach (Vector3 n in path)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(n, Vector3.one * (1f));
            }
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(targetPos, Vector3.one * (1.1f));
        }
    }*/

    public void GetRandomPath()
    {
        int randX, randY;
        while (true)
        {
            randX = Random.Range(0, pathfinding.grid.size.x);
            randY = Random.Range(0, pathfinding.grid.size.y);
            //targetPos = new Vector3(randX, randY, 0);

            if (pathfinding.grid.grid[randX, randY].walkable)
            {
                break;
            }
        }

        path = pathfinding.FindPath(transform.position, new Vector3(pathfinding.grid.gridStartPos.x + randX, pathfinding.grid.gridStartPos.y + randY, 0));
    }

    public void StartMoving()
    {
        StopMoving();
        isMoving = true;
        moveCoroutine = StartCoroutine(FollowPath());
    }
    public void StopMoving()
    {
        isMoving = false;
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
    }

    IEnumerator FollowPath()
    {
        while (true)
        {
            if(path.Count == 0)
            {
                break;
            }
            Vector3 nextWaypoint = path[0];
            path.RemoveAt(0);

            while (true)
            {

                Vector2 diff = nextWaypoint - transform.position;
                if(diff.sqrMagnitude < 0.01f)
                {
                    break;
                }

                Vector3 direction = diff.normalized;
                transform.position = Vector3.Lerp(transform.position, transform.position + direction, Time.deltaTime * speed);
                transform.rotation = Tools.FaceTarget(nextWaypoint, transform.position, -90);

                yield return new WaitForEndOfFrame();
            }
        }
        isMoving = false;
        yield return null;
    }
}
