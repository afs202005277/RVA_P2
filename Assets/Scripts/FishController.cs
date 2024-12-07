using UnityEngine;

public class FishController : MonoBehaviour
{
    [Range(0.5f, 5.0f)]
    public float maxSpeed;

    [HideInInspector]
    public GameObject[] waypoints;
    [HideInInspector]
    public int currentWaypointIndex;

    private int targetWaypointIndex;
    private float targetMargin = 1.0f;
    private Vector3 targetPosition, direction;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.5f, maxSpeed);

        targetWaypointIndex = CalculateTargetWaypointIndex();
        targetPosition = RandomPointInWaypoint(waypoints[targetWaypointIndex]);
        direction = targetPosition - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    // Update is called once per frame
    void Update()
    {
        direction = targetPosition - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, direction, speed * Time.deltaTime, 0.0f));
        transform.Translate(0, 0, speed * Time.deltaTime);
        //Debug.DrawLine(transform.position, targetPosition, Color.red);
        if (Mathf.Abs(Vector3.Distance(transform.position, targetPosition)) < targetMargin)
        {
            speed = Random.Range(0.5f, maxSpeed);

            currentWaypointIndex = targetWaypointIndex;
            targetWaypointIndex = CalculateTargetWaypointIndex();
            targetPosition = RandomPointInWaypoint(waypoints[targetWaypointIndex]);
            direction = targetPosition - transform.position;
        }
    }

    int CalculateTargetWaypointIndex()
    {
        if (currentWaypointIndex == waypoints.Length - 1) return waypoints.Length - 2;
        if (currentWaypointIndex == 0) return 1;

        int[] increments = new int[] { -1, 1 };
        int randomIncrement = increments[Random.Range(0, increments.Length)];
        
        return currentWaypointIndex + randomIncrement;
    }

    Vector3 RandomPointInWaypoint(GameObject waypoint)
    {
        Bounds waypointBounds = waypoint.GetComponent<BoxCollider>().bounds;
        return new Vector3(
            Random.Range(waypointBounds.min.x, waypointBounds.max.x),
            Random.Range(waypointBounds.min.y, waypointBounds.max.y),
            Random.Range(waypointBounds.min.z, waypointBounds.max.z)
        );
    }
}
