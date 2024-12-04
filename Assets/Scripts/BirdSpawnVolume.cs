using UnityEngine;

[ExecuteAlways]
public class BirdSpawnVolume : MonoBehaviour
{
    private BoxCollider boxCollider;
    private Bounds bounds;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public Vector3 GetRandomPosition()
    {
        bounds = boxCollider.bounds;
        // Randomize position within the collider's world-space bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }
}
