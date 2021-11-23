using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public GridController grid;

    private void Start()
    {
        grid = GetComponent<GridController>();
    }

    public List<Vector3> FindPath(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = grid.GetNodeFromPoint(startPos);
        Node endNode = grid.GetNodeFromPoint(endPos);
        //print("start n : " + startNode.pos + " , end n : " + endNode.pos);

        startNode.parent = startNode;

        bool pathSuccess = false;
        if (startNode.walkable && endNode.walkable)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while(openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                openSet.RemoveAt(0);
                closedSet.Add(currentNode);

                if(currentNode == endNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach(Node neighbour in currentNode.neighbours)
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;

                    int neighbourCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (neighbourCost < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = neighbourCost;
                        neighbour.hCost = GetDistance(neighbour, endNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
                openSet = openSet.OrderBy(x => x.gCost + x.hCost).ToList();
            }
        }

        List<Vector3> waypoints = new List<Vector3>();
        if (pathSuccess)
        {
            Node currentNode = endNode;
            while(currentNode.parent != currentNode)
            {
                waypoints.Add(currentNode.worldPosition);
                currentNode = currentNode.parent;
            }
        }
        waypoints.Reverse();
        return waypoints;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.pos.x - nodeB.pos.x);
        int dstY = Mathf.Abs(nodeA.pos.y - nodeB.pos.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
