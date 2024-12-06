using UnityEngine;
using System.Collections.Generic;

public class SticksRocksGenerator : MonoBehaviour
{
    public GameObject[] sticks;
    public GameObject[] rocks;
    public float boundingSphereRadius;
    public float worldRadius;
    public int spawnCount;
    public Transform playerPos;
    public Transform rockAggregator;
    public Transform stickAggregator;

    public float stickProbability;
    public float spawnHeightOffset;

    private Terrain terrain;
    private Vector3 center = Vector3.zero;

    private GameObject[] spawnedObjects;

    private bool ready = false;

    private void Start()
    {
        terrain = Terrain.activeTerrain;

        if (terrain == null)
        {
            Debug.LogError("No active terrain found!");
            return;
        }

        SpawnSticksAndRocks(center);
    }

    private void Update()
    {
        if (!ready) { return; }

        Vector3 player_position = playerPos.position;
        foreach (GameObject obj in spawnedObjects)
        {
            if (Vector3.Distance(obj.transform.position, player_position) <= boundingSphereRadius) // active if close to player
            {
                obj.GetComponent<Outline>().enabled = true;
            }
            else
            {
                obj.GetComponent<Outline>().enabled = false;
            }
        }
    }

    private void SpawnSticksAndRocks(Vector3 center)
    {
        List<GameObject> s_objects = new List<GameObject>();
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * worldRadius;

            randomPoint.y = Terrain.activeTerrain.SampleHeight(new Vector3(randomPoint.x, 0, randomPoint.z)) + spawnHeightOffset;


            if (randomPoint.y > 0) // Ensure it's above terrain
            {
                bool isStick = Random.value <= stickProbability;
                GameObject prefabToSpawn = isStick ? sticks[Random.Range(0, sticks.Length)] : rocks[Random.Range(0, rocks.Length)];
                GameObject inst = Instantiate(prefabToSpawn, randomPoint, prefabToSpawn.transform.rotation);
                inst.name = inst.name + $"__{i}";
                if (isStick)
                {
                    inst.transform.SetParent(stickAggregator.transform, true);
                }
                else
                {
                    inst.transform.SetParent(rockAggregator.transform, true);
                }
                // Debug.Log($"{inst.name}: {randomPoint}");
                s_objects.Add(inst);
            }
            else
            {
                Debug.LogWarning($"Object spawned below terrain: {randomPoint}.");
            }
        }
        spawnedObjects = s_objects.ToArray();
        ready = true;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the bounding sphere in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerPos.position, boundingSphereRadius);

        // Visualize the world sphere in the editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center, worldRadius);
    }

    public void DeleteGameObjectsFromList(List<GameObject> objectsToDelete)
    {
        ready = false;
        // Create a temporary list to store the remaining GameObjects
        List<GameObject> remainingPrefabs = new List<GameObject>();

        foreach (GameObject prefab in spawnedObjects)
        {
            // Add to the remaining list only if it's not in the objectsToDelete list
            if (!objectsToDelete.Contains(prefab))
            {
                remainingPrefabs.Add(prefab);
            }
        }

        // Update the instantiatedPrefabs array with the remaining prefabs
        spawnedObjects = remainingPrefabs.ToArray();

        ready = true;
    }
}
