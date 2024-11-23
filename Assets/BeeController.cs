using UnityEngine;

public class BeeController : MonoBehaviour
{
    public float maxSpeed = 20.0f; // Maximum velocity
    public float startSpeed = 4.0f;
    public float pauseDuration = 1.0f; // Time to stay at a flower
    public float arcHeightFactor = 1.5f; // Height factor of the arc trajectory

    private Transform targetFlower;
    public Transform flowersParent;
    private Vector3[] controlPoints;
    private float t; // Normalized time for the path
    private float currentSpeed;
    private bool isPaused;
    private float pauseTimer;

    void Start()
    {
        PickNewFlower();
        GenerateControlPoints();
        t = 0;
        isPaused = false;
    }

    void Update()
    {
        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0)
            {
                isPaused = false;
                PickNewFlower();
                GenerateControlPoints();
                t = 0;
            }
            return;
        }

        currentSpeed = Mathf.Max(currentSpeed, startSpeed);

        // Update progress along the curve
        t += Time.deltaTime * currentSpeed / Vector3.Distance(controlPoints[0], controlPoints[2]);

        if (t >= 1.0f)
        {
            t = 1.0f;
            OnFlowerArrival();
            return;
        }

        UpdateSpeed(t);

        Vector3 targetPosition = BezierCurve(controlPoints, t);
        transform.position = targetPosition;
        transform.LookAt(BezierCurve(controlPoints, Mathf.Min(t + 0.01f, 1.0f)));
    }

    void UpdateSpeed(float t)
    {
        if (t <= 0.1f) // Accelerating phase
        {
            currentSpeed = Mathf.Lerp(startSpeed, maxSpeed, t / 0.2f);
        }
        else if (t <= 0.8f) // Constant speed phase
        {
            currentSpeed = maxSpeed;
        }
        else // Decelerating phase
        {
            currentSpeed = Mathf.Lerp(maxSpeed, 0, (t - 0.8f) / 0.2f);
        }

        Debug.Log($"Current speed: {currentSpeed}");
    }

    void OnFlowerArrival()
    {
        isPaused = true;
        pauseTimer = pauseDuration;
    }

    void PickNewFlower()
    {
        Transform currentFlower = targetFlower;
        while (currentFlower == targetFlower)
        {
            targetFlower = flowersParent.GetChild(Random.Range(0, flowersParent.childCount));
        }
    }

    void GenerateControlPoints()
    {
        controlPoints = new Vector3[3];
        controlPoints[0] = transform.position;

        // The middle point is higher to create an arc
        Vector3 midPoint = (transform.position + targetFlower.position) / 2;
        midPoint.y *= arcHeightFactor;
        controlPoints[1] = midPoint;

        controlPoints[2] = targetFlower.position;
    }

    Vector3 BezierCurve(Vector3[] points, float t)
    {
        // Quadratic Bezier: B(t) = (1-t)^2 * P0 + 2(1-t)t * P1 + t^2 * P2
        return Mathf.Pow(1 - t, 2) * points[0] +
               2 * (1 - t) * t * points[1] +
               Mathf.Pow(t, 2) * points[2];
    }
}
