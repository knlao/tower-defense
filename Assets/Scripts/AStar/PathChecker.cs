using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathChecker : MonoBehaviour
{
    public static PathChecker instance;

    public int straightCost = 10;
    public int diagonalCost = 14;

    //public Transform target;

    private PathCheckingRequestManager requestManager;
    private NodeGridChecker grid;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        grid = GetComponent<NodeGridChecker>();
        requestManager = GetComponent<PathCheckingRequestManager>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        grid.CreateGrid();
        StartCoroutine(FindPath(startPos, targetPos));
    }

    private IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        AStarNode startNode = grid.NodeFromWorldPoint(startPos);
        AStarNode targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<AStarNode> openSet = new Heap<AStarNode>(grid.MaxSize); // the set of nodes to be evaluated
            HashSet<AStarNode> closeSet = new HashSet<AStarNode>(); // the set of nodes already evaluated
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                AStarNode currentNode = openSet.RemoveFirst();

                closeSet.Add(currentNode);

                if (currentNode == targetNode) // path has been found
                {
                    pathSuccess = true;
                    break;
                }

                foreach (AStarNode neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closeSet.Contains(neighbour)) continue;

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(currentNode, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }

        yield return null;
        //if (pathSuccess)
        //{
        //    //waypoints = RetracePath(startNode, targetNode);
        //    Debug.Log("PathChecker: Found");
        //}
        requestManager.FinishedProcessingPath(pathSuccess);
    }

    //private Vector3[] RetracePath(AStarNode startNode, AStarNode endNode)
    //{
    //    List<AStarNode> path = new List<AStarNode>();
    //    AStarNode currentNode = endNode;

    //    while (currentNode != startNode)
    //    {
    //        path.Add(currentNode);
    //        currentNode = currentNode.parent;
    //    }

    //    Vector3[] waypoints = SimplifyPath(path);

    //    Array.Reverse(waypoints);

    //    return waypoints;
    //}

    //private Vector3[] SimplifyPath(List<AStarNode> path)
    //{
    //    List<Vector3> waypoints = new List<Vector3>();
    //    Vector2 directionOld = Vector2.zero;

    //    for (int i = 1; i < path.Count; i++)
    //    {
    //        Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
    //        if (directionNew != directionOld)
    //        {
    //            waypoints.Add(path[i].worldPosition);
    //        }
    //        directionOld = directionNew;
    //    }

    //    return waypoints.ToArray();
    //}

    private int GetDistance(AStarNode nodeA, AStarNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return diagonalCost * dstY + straightCost * (dstX - dstY);
        return diagonalCost * dstX + straightCost * (dstY - dstX);
    }
}
