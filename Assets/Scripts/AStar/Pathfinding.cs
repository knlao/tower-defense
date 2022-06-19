using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    /*
     * A* Pathfinding
     * 
     * OPEN // the set of nodes to be evaluated
     * CLOSED // the set of nodes already evaluated
     * add the start node to OPEN
     * 
     * loop
	 *     current = node in OPEN with the lowest f_cost
	 *     remove current from OPEN
	 *     add current to CLOSED
     * 
	 *     if current is the target node // path has been found
	 * 	    return
     * 
	 *     foreach neighbour of the current node
	 * 	    if neighbour is not traversable or neighbour is in CLOSED
	 * 		    skip to the next neighbour
     * 
	 * 	    if new path to neighbour is shorter or neighbour is not in OPEN
	 * 		    set f_cost of neighbour
	 * 		    set parent of neighbour to current
	 * 		    if neighbour is not in OPEN
	 * 			    add neighbour to OPEN
     */

    public static Pathfinding instance;

    public int straightCost = 10;
    public int diagonalCost = 14;

    public bool isProcessingPath;

    public Transform target;

    public int instanceIdx;

    private PathRequestManager requestManager;
    private NodeGrid grid;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        grid = FindObjectOfType<NodeGrid>();
        requestManager = FindObjectOfType<PathRequestManager>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        isProcessingPath = true;
        StartCoroutine(FindPath(startPos, targetPos));
    }

    private IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        AStarNode startNode = grid.NodeFromWorldPoint(startPos);
        AStarNode targetNode = grid.NodeFromWorldPoint(targetPos);

        //if (startNode.walkable && targetNode.walkable)
        //{
            Heap<AStarNode> openSet = new Heap<AStarNode>(grid.MaxSize); // the set of nodes to be evaluated
            HashSet<AStarNode> closeSet = new HashSet<AStarNode>(); // the set of nodes already evaluated
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                AStarNode currentNode = openSet.RemoveFirst();

                closeSet.Add(currentNode);

                if (currentNode == targetNode) // path has been found
                {
                    //sw.Stop();
                    //UnityEngine.Debug.Log("Path found: " + sw.ElapsedMilliseconds + " ms used.");
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
        //}
        //else
        //{
        //    print("it is stuck in a node");
        //}

        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(instanceIdx, waypoints, pathSuccess);
        isProcessingPath = false;
    }

    private Vector3[] RetracePath(AStarNode startNode, AStarNode endNode)
    {
        List<AStarNode> path = new List<AStarNode>();
        AStarNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = PathToV3List(path);

        Array.Reverse(waypoints);

        return waypoints;
    }

    private Vector3[] PathToV3List(List<AStarNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();

        for (int i = 1; i < path.Count; i++)
        {
            waypoints.Add(path[i].worldPosition);
        }

        return waypoints.ToArray();
    }

    private Vector3[] SimplifyPath(List<AStarNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    private int GetDistance(AStarNode nodeA, AStarNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return diagonalCost * dstY + straightCost * (dstX - dstY);
        return diagonalCost * dstX + straightCost * (dstY - dstX);
    }

}
