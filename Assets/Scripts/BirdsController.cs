using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsController : MonoBehaviour
{
    public GameObject[] availableBirds;
    public GameObject homesAggregator;
    public Transform birdsAggregator;
    public BirdSpawnVolume birdSpawnVolume;
    public int numBirds;

    void Start()
    {
        List<GameObject> homes = FetchChildren(homesAggregator);
        for (int i = 0; i < numBirds; i++)
        {
            GameObject instantiatedBird = Instantiate(availableBirds[Random.Range(0, availableBirds.Length)], birdSpawnVolume.GetRandomPosition(), Quaternion.identity);
            instantiatedBird.name = instantiatedBird.name + $"_{i}";
            instantiatedBird.transform.SetParent(birdsAggregator, true);
            RandomFlyer script = instantiatedBird.GetComponent<RandomFlyer>();
            script.homeTarget = homes[Random.Range(0, homes.Count)].transform;

            GameObject emptyGameObject = new GameObject($"{instantiatedBird.name}_target");
            emptyGameObject.transform.SetPositionAndRotation(birdSpawnVolume.GetRandomPosition(), Quaternion.identity);
            script.flyingTarget = emptyGameObject.transform;
        }
    }

    private List<GameObject> FetchChildren(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }
}
