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

        sun.transform.SetPositionAndRotation(new Vector3(sunX, sunY, sunZ), Quaternion.LookRotation(-sun.transform.position.normalized));

        float moonX = -sunX;
        float moonZ = -sunZ;
        float moonY = -sunY;

        moon.transform.SetPositionAndRotation(new Vector3(moonX, moonY, moonZ), Quaternion.LookRotation(-moon.transform.position.normalized));

        bool currentIsDay = sunY > 0;
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
            moon.shadows = LightShadows.None;
            sun.shadows = LightShadows.Soft;
            moon.intensity = 0;
            sun.intensity = sunIntensity;

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
            sun.shadows = LightShadows.None;
            moon.shadows = LightShadows.Soft;
            sun.intensity = 0;
            moon.intensity = moonIntensity;

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
}
