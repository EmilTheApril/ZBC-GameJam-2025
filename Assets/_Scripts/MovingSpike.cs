using UnityEngine;
using System.Collections.Generic;

public class MovingSpike : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool loop = true;
    [SerializeField] private float waitTime = 0.5f;

    private int currentWaypointIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    private void Update()
    {
        if (waypoints.Count == 0)
            return;

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                // Move to next waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
                if (!loop && currentWaypointIndex == 0)
                {
                    enabled = false;
                    return;
                }
            }
            return;
        }

        // Move towards current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWaypoint.position,
            speed * Time.deltaTime
        );

        // Check if we've reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.01f)
        {
            isWaiting = true;
        }
    }

    // Optional: Add gizmos to visualize waypoints in the editor
    private void OnDrawGizmos()
    {
        if (waypoints.Count == 0)
            return;

        Gizmos.color = Color.blue;
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] != null)
            {
                Gizmos.DrawSphere(waypoints[i].position, 0.2f);

                // Draw lines between waypoints
                if (i < waypoints.Count - 1 && waypoints[i + 1] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
                else if (loop && waypoints[0] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
                }
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerRotate>().AttackPlayer(transform);
        }
    }
}