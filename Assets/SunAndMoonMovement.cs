using UnityEngine;

public class SunAndMoonMovement : MonoBehaviour
{
    [Tooltip("Duration of the day in seconds (from sunrise to sunrise).")]
    public float dayDuration = 60f;

    [Tooltip("Radius of the orbit around the scene.")]
    public float orbitRadius = 100f;

    [Tooltip("Sun directional light.")]
    public Light sun;
    public int sunIntensity;

    [Tooltip("Moon directional light.")]
    public Light moon;
    public int moonIntensity;

    private float angularSpeed;

    private void Awake()
    {
        sun.shadows = LightShadows.Soft;
        moon.shadows = LightShadows.None;
    }

    void Start()
    {
        angularSpeed = 360f / dayDuration;

        if (moon == null || sun == null)
        {
            Debug.LogError("Moon/sun directional light is not assigned!");
        }
    }

    void Update()
    {
        // Calculate the angle of the sun in its orbit
        float angle = Time.time * angularSpeed;

        // Convert angle to radians for calculations
        float radians = Mathf.Deg2Rad * angle;

        // Calculate the sun's position
        float sunX = orbitRadius * Mathf.Cos(radians);
        float sunZ = orbitRadius * Mathf.Sin(radians);
        float sunY = orbitRadius * Mathf.Sin(radians); // Optional for vertical movement

        // Set the sun's position
        sun.transform.SetPositionAndRotation(new Vector3(sunX, sunY, sunZ), Quaternion.LookRotation(-sun.transform.position.normalized));

        // Calculate the moon's position (symmetric to the sun)
        float moonX = -sunX;
        float moonZ = -sunZ;
        float moonY = -sunY;

        // Set the moon's position and rotation
        moon.transform.SetPositionAndRotation(new Vector3(moonX, moonY, moonZ), Quaternion.LookRotation(-moon.transform.position.normalized));

        if (sunY > 0) // Sun is above the horizon
        {
            sun.shadows = LightShadows.Soft;
            moon.shadows = LightShadows.None;
            moon.intensity = 0;
            sun.intensity = sunIntensity;
        }
        else // Moon is above the horizon
        {
            sun.shadows = LightShadows.None;
            moon.shadows = LightShadows.Soft;
            sun.intensity = 0;
            moon.intensity = moonIntensity;
        }
    }
}
