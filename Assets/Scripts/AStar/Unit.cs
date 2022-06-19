using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    /*
     * Unit Script
     * The NEW Enemy Script
     */

    public Transform target;
    private float speed = 5;

    private Vector3[] path;
    private int targetIndex;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Finish").transform;
        GetNewPath();
    }

    public void GetNewPath()
    {
        PathRequestManager.instance.RequestPath(transform.position, target.position, OnPathFound);
    }

    private void Update()
    {
        speed = GetComponent<Enemy>().speed;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {

        try
        {
            if (pathSuccessful)
            {
                path = newPath;
                StopCoroutine(nameof(FollowPath));
                targetIndex = 0;
                StartCoroutine(nameof(FollowPath));
            }
            else
            {
                Debug.LogError("Cannot find path");
                StopCoroutine(nameof(FollowPath));
                Debug.LogError("Why blocking my path?");
                Destroy(gameObject);
            }
        }
        catch (MissingReferenceException e)
        {
            print("we got an error, but nothing really happened: " + e.Message);
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length) // Hit END
                {
                    EndPath();
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void EndPath()
    {
        PlayerStats.Lives--;
        //WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * 0.2f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
