using UnityEngine;

public class SticksRocksGenerator : MonoBehaviour
{
    public GameObject[] sticks;
    public GameObject[] rocks;
    public float boundingSphereRadius; // Radius of the bounding sphere
    public int spawnCount; // Number of objects to spawn per batch
    public Transform playerPos;

    public float stickProbability;
    public float spawnHeightOffset;

    private Vector3 lastSpawnPosition;
    private Terrain terrain;

    private void Start()
    {
        lastSpawnPosition = playerPos.position;
        terrain = Terrain.activeTerrain;

        if (terrain == null)
        {
            Debug.LogError("No active terrain found!");
            return;
        }

        SpawnSticksAndRocks(lastSpawnPosition);
    }

    private void Update()
    {
        if (Vector3.Distance(playerPos.position, lastSpawnPosition) >= boundingSphereRadius)
        {
            lastSpawnPosition = playerPos.position;
            SpawnSticksAndRocks(lastSpawnPosition);
        }
    }

    private void SpawnSticksAndRocks(Vector3 center)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * boundingSphereRadius;
            randomPoint.y = terrain.SampleHeight(randomPoint) + spawnHeightOffset;

            if (randomPoint.y > 0) // Ensure it's above terrain
            {
                GameObject prefabToSpawn = (Random.value <= stickProbability) ? sticks[Random.Range(0, sticks.Length)] : rocks[Random.Range(0, rocks.Length)];
                Instantiate(prefabToSpawn, randomPoint, prefabToSpawn.transform.rotation);
            }
            else
            {
                Debug.LogWarning($"Object spawned below terrain: {randomPoint}.");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the bounding sphere in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerPos.position, boundingSphereRadius);
    }
}
