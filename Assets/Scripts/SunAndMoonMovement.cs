using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class DayNightController : MonoBehaviour
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

    public GameObject[] firePlaces;

    public AudioSource forestSounds;
    public AudioClip nightSound;
    public AudioClip daySound;

    [Tooltip("GameObject containing all bird GameObjects as children.")]
    public GameObject birds;

    public SkyboxTransition skyboxTransition;

    private float angularSpeed;
    private bool isDay; // Tracks whether it's currently day or night
    private LightShadows moonShadows = LightShadows.Soft;
    private LightShadows sunShadows = LightShadows.Soft;
    private float horizonOffset = 1f;

    private void Awake()
    {
        moon.shadows = LightShadows.None;
        sun.shadows = LightShadows.Soft;
    }

    void Start()
    {
        angularSpeed = 360f / dayDuration;

        if (moon == null || sun == null)
        {
            Debug.LogError("Moon/sun directional light is not assigned!");
        }

        if (forestSounds == null)
        {
            Debug.LogError("ForestSounds audio source is not assigned!");
        }

        // Initialize the isDay variable based on the sun's starting position
        isDay = sun.transform.position.y > 0;
        UpdateEnvironment();
    }

    void Update()
    {
        float angle = Time.time * angularSpeed;
        float radians = Mathf.Deg2Rad * angle;

        float sunX = orbitRadius * Mathf.Cos(radians);
        float sunZ = orbitRadius * Mathf.Sin(radians);
        float sunY = orbitRadius * Mathf.Sin(radians);

        sun.transform.SetPositionAndRotation(
            new Vector3(sunX, sunY, sunZ),
            Quaternion.LookRotation(-sun.transform.position.normalized)
        );

        // Check if it is daytime
        bool currentIsDay = sunY > 0;

        if (currentIsDay)
        {
            moon = TurnOffLight(moon);
            sun = TurnOnLight(sun, sunIntensity, sunShadows);
        }
        else
        {
            moon = TurnOnLight(moon, moonIntensity, moonShadows);
            sun = TurnOffLight(sun);

            // Moon position: Opposite side, slightly above the horizon
            float moonX = -sunX;
            float moonZ = -sunZ;
            float moonY = Mathf.Max(-sunY, horizonOffset); // Ensure moon stays above the horizon

            moon.transform.SetPositionAndRotation(
                new Vector3(moonX, moonY, moonZ),
                Quaternion.LookRotation(-moon.transform.position.normalized)
            );
        }

        // Update environment when the state changes
        if (currentIsDay != isDay)
        {
            isDay = currentIsDay;
            UpdateEnvironment();
        }
    }


    private void UpdateEnvironment()
    {
        if (isDay)
        {
            skyboxTransition.TransitionToDay();
            sun = TurnOnLight(sun, sunIntensity, LightShadows.Soft);
            moon = TurnOffLight(moon);

            foreach (GameObject firePlace in firePlaces)
            {
                Transform fire = firePlace.transform.Find("Fire");
                if (fire != null)
                {
                    fire.gameObject.SetActive(false);
                }
            }

            if (birds != null)
            {
                foreach (Transform bird in birds.transform)
                {
                    AudioSource birdAudio = bird.GetComponent<AudioSource>();
                    if (birdAudio != null)
                    {
                        birdAudio.enabled = true;
                    }
                }
            }

            forestSounds.clip = daySound;
            forestSounds.Play();
        }
        else
        {
            skyboxTransition.TransitionToNight();
            moon = TurnOnLight(moon, moonIntensity, LightShadows.Soft);
            sun = TurnOffLight(sun);

            foreach (GameObject firePlace in firePlaces)
            {
                Transform fire = firePlace.transform.Find("Fire");
                if (fire != null)
                {
                    fire.gameObject.SetActive(true);
                }
            }

            if (birds != null)
            {
                foreach (Transform bird in birds.transform)
                {
                    AudioSource birdAudio = bird.GetComponent<AudioSource>();
                    if (birdAudio != null)
                    {
                        birdAudio.enabled = false;
                    }
                }
            }

            forestSounds.clip = nightSound;
            forestSounds.Play();
        }
    }

    private Light TurnOnLight(Light light, float intensity, LightShadows shadow)
    {
        light.intensity = intensity;
        light.shadows = shadow;
        return light;
    }

    private Light TurnOffLight(Light light)
    {
        light.intensity = 0;
        light.shadows = LightShadows.None;

        return light;
    }
}

