using UnityEngine;

public class RandomSeedManager : MonoBehaviour
{
    public int seed = 42;

    void Start()
    {
        Random.InitState(seed); // Set the global random seed

        // Set master volume to volume slider's default value
        AudioListener.volume = 0.5f;
    }
}
