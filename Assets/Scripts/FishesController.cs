using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishesController : MonoBehaviour
{
    public GameObject[] availableFishes;
    public GameObject waypointAggregator;
    public int numberOfFishes;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> waypoints = FetchChildren(waypointAggregator);
        for (int i = 0; i < numberOfFishes; i++)
        {
            int randomWaypointIndex = Random.Range(0, waypoints.Count);
            Bounds waypointBounds = waypoints[randomWaypointIndex].GetComponent<BoxCollider>().bounds;
            Vector3 randomWaypointPoint = new Vector3(
                Random.Range(waypointBounds.min.x, waypointBounds.max.x),
                Random.Range(waypointBounds.min.y, waypointBounds.max.y),
                Random.Range(waypointBounds.min.z, waypointBounds.max.z)
            );

            GameObject instantiatedFish = Instantiate(availableFishes[Random.Range(0, availableFishes.Length)], randomWaypointPoint, Quaternion.identity);
            instantiatedFish.name += $"_{i}";
            instantiatedFish.transform.SetParent(this.transform, true);
            float randomSize = Random.Range(0.8f, 4.2f);
            instantiatedFish.transform.localScale = new Vector3(instantiatedFish.transform.localScale.x, randomSize, randomSize);

            FishController fishScript = instantiatedFish.GetComponent<FishController>();
            fishScript.waypoints = waypoints.ToArray();
            fishScript.currentWaypointIndex = randomWaypointIndex;
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
