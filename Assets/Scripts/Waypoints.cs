using UnityEngine;

public class Waypoints : MonoBehaviour
{
    /*
     * Waypoints Script
     * Get all waypoints for enemy's path
     */
    
    public static Transform[] Points;

    private void Awake()
    {
        Points = new Transform[transform.childCount];
        for (var i = 0; i < Points.Length; i++)
        {
            Points[i] = transform.GetChild(i);
        }
    }
}
