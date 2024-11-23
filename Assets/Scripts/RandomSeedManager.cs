using UnityEngine;

public class RandomSeedManager : MonoBehaviour
{
    public int seed = 42;

    void Start()
    {
        Random.InitState(seed); // Set the global random seed
        Debug.Log("Random seed initialized: " + seed);
    }
}
