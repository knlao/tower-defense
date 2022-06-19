using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    private PathRequest[] currentPathRequests;

    public static PathRequestManager instance;
    private Pathfinding[] pathfindings;
    private int pathFindingIdx;

    private bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pathfindings = GetComponentsInChildren<Pathfinding>();
        foreach (var x in pathfindings)
        {
            x.instanceIdx = pathFindingIdx++;
        }
        pathFindingIdx = 0;
        currentPathRequests = new PathRequest[pathfindings.Length];
    }

    public void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        pathRequestQueue.Enqueue(newRequest);
        TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequests[pathFindingIdx] = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfindings[pathFindingIdx].StartFindPath(currentPathRequests[pathFindingIdx].pathStart, currentPathRequests[pathFindingIdx].pathEnd);
            //pathFindingIdx++;
            //pathFindingIdx %= pathfindings.Length;
        }
    }

    public void FinishedProcessingPath(int instanceIdx, Vector3[] path, bool success)
    {
        if (currentPathRequests[instanceIdx].callback == null) print("f");
        currentPathRequests[instanceIdx].callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    private struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[], bool> _callback)
        {
            pathStart = _pathStart;
            pathEnd = _pathEnd;
            callback = _callback;
        }
    }
}
