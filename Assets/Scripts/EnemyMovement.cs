using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    /*
     * Enemy Movement Script
     * The movement of the enemy (path following)
     * Get waypoints from Waypoint.Points
     */
    
    private Transform _target;
    private int _wayPointIdx = 0;
    private Enemy _enemy;

    private void Start()
    {
        _target = Waypoints.Points[0];
        _enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        var dir = _target.position - transform.position;
        transform.Translate(dir.normalized * _enemy.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, _target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }

        _enemy.speed = _enemy.startSpeed;
    }

    private void GetNextWaypoint()
    {
        if (_wayPointIdx >= Waypoints.Points.Length - 1)
        {
            EndPath();
            return;
        }
        _target = Waypoints.Points[++_wayPointIdx];
    }

    private void EndPath()
    {
        PlayerStats.Lives--;
        Destroy(gameObject);
    }
}
