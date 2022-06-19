using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCheckingRequestManager : MonoBehaviour
{
    private Queue<PathCheckingRequest> pathRequestQueue = new Queue<PathCheckingRequest>();
    private PathCheckingRequest currentPathRequest;

    public static PathCheckingRequestManager instance;
    private PathChecker pathfinding;

    private bool isProcessingPath;

    public Transform pathStart, pathEnd;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pathfinding = GetComponent<PathChecker>();
    }

    public void RequestPath(Action<bool> callback)
    {
        PathCheckingRequest newRequest = new PathCheckingRequest(pathStart.position, pathEnd.position, callback);
        pathRequestQueue.Enqueue(newRequest);
        TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;

            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(bool success)
    {
        currentPathRequest.callback(success);
        isProcessingPath = false;
        TryProcessNext();
    }

    private struct PathCheckingRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<bool> callback;

        public PathCheckingRequest(Vector3 _pathStart, Vector3 _pathEnd, Action<bool> _callback)
        {
            pathStart = _pathStart;
            pathEnd = _pathEnd;
            callback = _callback;
        }
    }
}
